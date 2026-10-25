"""Judge a headless regression run: evaluate a .queries file against the HeadlessLogger log it produced.

Usage:
    python evaluate_queries.py <queries file> <log file> [--commands <commands file>]

The .commands file defaults to the .queries file's sibling with the same name. It is read only to check
that every step has at least one query.

Query file format (one query per line, # starts a comment):

    [Named Test]                        a section; the summary prints one PASS/FAIL line per section
    <step>: expect <Event> [k=v ...] [by=<later step>]
    <step>: expectNone <Event or level=LEVEL> [k=v ...] [by=<later step>]
    <step>: count <Event> [k=v ...] [by=<later step>] == n      (also >= and <=)
    all:    expect / expectNone / count ...                     the same over the whole log
    all:    order <EventA> [k=v ...] before <EventB> [k=v ...]  A first appears before B

A step's window runs from its Command.Started line to the next step's Command.Started line (or the end of
the log). by=<step> stretches it to the end of that later step.

Prints one line per named test, then a RESULT line with the run time: the time from the Script.Started
line to the moment the summary is built. Exits 0 when every named test passes and every step is covered.
"""

import argparse
import datetime
import os
import re
import sys

LINE_PATTERN = re.compile(r"^\[(\d{2}):(\d{2}):(\d{2})\.(\d{3})\] \[([A-Z]+)\] (.*)$")
STEP_LABEL_PATTERN = re.compile(r"^[A-Za-z0-9_-]+$")
COMPARISONS = ("==", ">=", "<=")
UNNAMED_SECTION = "(queries outside any [section])"


class LogEntry:
    def __init__(self, index, raw, time_of_day, level, event, keys):
        self.index = index
        self.raw = raw
        self.time_of_day = time_of_day
        self.level = level
        self.event = event
        self.keys = keys


class Matcher:
    """An event name (or level=LEVEL) plus the keys a line must carry."""

    def __init__(self, event, keys, level=None):
        self.event = event
        self.keys = keys
        self.level = level

    def matches(self, entry):
        if self.level is not None:
            if entry.level != self.level:
                return False
            if self.event is None:
                return True

        if entry.event is None or entry.event != self.event:
            return False

        for key, value in self.keys.items():
            if entry.keys.get(key) != value:
                return False

        return True

    def describe(self):
        parts = []
        if self.level is not None:
            parts.append("level=" + self.level)
        if self.event is not None:
            parts.append(self.event)
        parts.extend(key + "=" + format_value(value) for key, value in self.keys.items())
        return " ".join(parts)


class Query:
    def __init__(self, line_number, text, section, step, verb):
        self.line_number = line_number
        self.text = text
        self.section = section
        self.step = step
        self.verb = verb
        self.matcher = None
        self.second_matcher = None
        self.by = None
        self.comparison = None
        self.expected_count = None


class QueryFileError(Exception):
    pass


# --- Parsing -------------------------------------------------------------------------------------------

def tokenize(text):
    """Split on spaces outside quotes; quotes are removed and \\" and \\\\ unescaped."""
    tokens = []
    current = []
    in_quotes = False
    has_token = False
    index = 0

    while index < len(text):
        character = text[index]

        if in_quotes and character == "\\" and index + 1 < len(text):
            current.append(text[index + 1])
            index += 2
            continue

        if character == '"':
            in_quotes = not in_quotes
            has_token = True
        elif character == " " and not in_quotes:
            if has_token:
                tokens.append("".join(current))
                current = []
                has_token = False
        else:
            current.append(character)
            has_token = True

        index += 1

    if has_token:
        tokens.append("".join(current))

    return tokens


def split_key_value(token):
    equals = token.find("=")
    space = token.find(" ")

    if equals > 0 and (space < 0 or space > equals):
        return token[:equals], token[equals + 1:]

    return None


def format_value(value):
    if value == "" or any(character in value for character in ' "='):
        return '"' + value.replace("\\", "\\\\").replace('"', '\\"') + '"'
    return value


def strip_comment(line):
    in_quotes = False

    for index, character in enumerate(line):
        if character == '"':
            in_quotes = not in_quotes
        elif character == "#" and not in_quotes:
            return line[:index]

    return line


def parse_log(path):
    entries = []

    with open(path, encoding="utf-8", errors="replace") as log_file:
        for raw in log_file:
            raw = raw.rstrip("\r\n")

            if not raw:
                continue

            match = LINE_PATTERN.match(raw)

            if match is None:
                entries.append(LogEntry(len(entries), raw, None, None, None, {}))
                continue

            hours, minutes, seconds, millis, level, message = match.groups()
            time_of_day = datetime.time(int(hours), int(minutes), int(seconds), int(millis) * 1000)
            tokens = tokenize(message)
            event = tokens[0] if tokens else None
            keys = {}

            for token in tokens[1:]:
                pair = split_key_value(token)
                if pair is not None:
                    keys[pair[0]] = pair[1]

            entries.append(LogEntry(len(entries), raw, time_of_day, level, event, keys))

    return entries


def parse_matcher(tokens, line_number):
    """tokens: [Event, k=v, ...] or [level=LEVEL, ...]. Returns (Matcher, by)."""
    if not tokens:
        raise QueryFileError("line %d: missing event name" % line_number)

    event = None
    level = None
    keys = {}
    by = None

    first_pair = split_key_value(tokens[0])

    if first_pair is not None and first_pair[0] == "level":
        level = first_pair[1].upper()
    elif first_pair is not None:
        raise QueryFileError("line %d: expected an event name before %s" % (line_number, tokens[0]))
    else:
        event = tokens[0]

    for token in tokens[1:]:
        pair = split_key_value(token)

        if pair is None:
            raise QueryFileError("line %d: expected key=value, got %r" % (line_number, token))

        if pair[0] == "by":
            by = pair[1]
        elif pair[0] == "level":
            level = pair[1].upper()
        else:
            keys[pair[0]] = pair[1]

    return Matcher(event, keys, level), by


def parse_queries(path):
    queries = []
    section = UNNAMED_SECTION
    sections = []

    with open(path, encoding="utf-8") as queries_file:
        lines = queries_file.readlines()

    for line_index, raw in enumerate(lines):
        line_number = line_index + 1
        line = strip_comment(raw).strip()

        if not line:
            continue

        if line.startswith("[") and line.endswith("]"):
            section = line[1:-1].strip()
            if not section:
                raise QueryFileError("line %d: empty section name" % line_number)
            if section in sections:
                raise QueryFileError("line %d: section [%s] appears twice" % (line_number, section))
            sections.append(section)
            continue

        colon = line.find(":")

        if colon <= 0:
            raise QueryFileError('line %d: expected "<step>: <query>"' % line_number)

        step = line[:colon].strip()
        tokens = tokenize(line[colon + 1:].strip())

        if not tokens:
            raise QueryFileError("line %d: step %s has no query" % (line_number, step))

        if step != "all" and not STEP_LABEL_PATTERN.match(step):
            raise QueryFileError("line %d: bad step label %r" % (line_number, step))

        query = Query(line_number, line, section, step, tokens[0])

        if section == UNNAMED_SECTION and UNNAMED_SECTION not in sections:
            sections.append(UNNAMED_SECTION)

        rest = tokens[1:]

        if query.verb in ("expect", "expectNone"):
            query.matcher, query.by = parse_matcher(rest, line_number)
        elif query.verb == "count":
            if len(rest) < 3 or rest[-2] not in COMPARISONS:
                raise QueryFileError("line %d: count needs '<Event> [k=v...] == n'" % line_number)
            query.comparison = rest[-2]
            try:
                query.expected_count = int(rest[-1])
            except ValueError:
                raise QueryFileError("line %d: count must end with a whole number" % line_number)
            query.matcher, query.by = parse_matcher(rest[:-2], line_number)
        elif query.verb == "order":
            if step != "all":
                raise QueryFileError("line %d: order only works with all:" % line_number)
            if "before" not in rest:
                raise QueryFileError("line %d: order needs '<A> before <B>'" % line_number)
            split_at = rest.index("before")
            query.matcher, _ = parse_matcher(rest[:split_at], line_number)
            query.second_matcher, _ = parse_matcher(rest[split_at + 1:], line_number)
        else:
            raise QueryFileError("line %d: unknown query %r" % (line_number, query.verb))

        if step == "all" and query.by is not None:
            raise QueryFileError("line %d: by= has no meaning for all:" % line_number)

        queries.append(query)

    return queries, sections


def read_command_labels(path):
    labels = []

    with open(path, encoding="utf-8") as commands_file:
        for raw in commands_file:
            line = strip_comment(raw).strip()
            if not line:
                continue
            colon = line.find(":")
            if colon > 0:
                labels.append(line[:colon].strip())

    return labels


# --- Evaluating ----------------------------------------------------------------------------------------

class RunInfo:
    def __init__(self, entries):
        self.entries = entries
        self.step_order = []
        self.step_starts = {}

        for entry in entries:
            if entry.event == "Command.Started":
                label = entry.keys.get("step")
                if label is not None and label not in self.step_starts:
                    self.step_starts[label] = entry.index
                    self.step_order.append(label)

    def window(self, step, by):
        """(start, end) line indices for a step, or an error string."""
        if step == "all":
            return 0, len(self.entries)

        if step not in self.step_starts:
            return "step %s never started" % step

        end_step = by if by is not None else step

        if end_step not in self.step_starts:
            return "by=%s never started" % end_step

        start = self.step_starts[step]
        end_position = self.step_order.index(end_step)

        if end_position < self.step_order.index(step):
            return "by=%s comes before %s" % (end_step, step)

        if end_position + 1 < len(self.step_order):
            end = self.step_starts[self.step_order[end_position + 1]]
        else:
            end = len(self.entries)

        return start, end


def find_matches(entries, matcher, start, end):
    return [entry for entry in entries[start:end] if matcher.matches(entry)]


def nearest_lines(entries, matcher, start, end, limit=3):
    """Lines with the same event name, closest to the window, to help explain a failure."""
    if matcher.event is None:
        return []

    candidates = [entry for entry in entries if entry.event == matcher.event]

    def distance(entry):
        if start <= entry.index < end:
            return 0
        return min(abs(entry.index - start), abs(entry.index - (end - 1)))

    candidates.sort(key=distance)
    return candidates[:limit]


def evaluate(query, run):
    """Returns (passed, detail lines)."""
    entries = run.entries

    if query.verb == "order":
        first_a = find_matches(entries, query.matcher, 0, len(entries))
        first_b = find_matches(entries, query.second_matcher, 0, len(entries))

        if not first_a:
            return False, ["no %s in the log" % query.matcher.describe()]
        if not first_b:
            return False, ["no %s in the log" % query.second_matcher.describe()]
        if first_a[0].index < first_b[0].index:
            return True, []
        return False, ["first %s is at line %d, after first %s at line %d" % (
            query.matcher.describe(), first_a[0].index + 1, query.second_matcher.describe(), first_b[0].index + 1)]

    window = run.window(query.step, query.by)

    if isinstance(window, str):
        return False, [window]

    start, end = window
    matches = find_matches(entries, query.matcher, start, end)
    where = "lines %d-%d" % (start + 1, end) if query.step != "all" else "the whole log"

    if query.verb == "expect":
        if matches:
            return True, []
        details = ["no %s in %s" % (query.matcher.describe(), where)]
        details.extend("nearest: line %d: %s" % (entry.index + 1, entry.raw)
                       for entry in nearest_lines(entries, query.matcher, start, end))
        return False, details

    if query.verb == "expectNone":
        if not matches:
            return True, []
        details = ["%d unwanted %s in %s" % (len(matches), query.matcher.describe(), where)]
        details.extend("line %d: %s" % (entry.index + 1, entry.raw) for entry in matches[:3])
        return False, details

    if query.verb == "count":
        actual = len(matches)
        passed = {
            "==": actual == query.expected_count,
            ">=": actual >= query.expected_count,
            "<=": actual <= query.expected_count,
        }[query.comparison]
        if passed:
            return True, []
        return False, ["found %d %s in %s, expected %s %d" % (
            actual, query.matcher.describe(), where, query.comparison, query.expected_count)]

    return False, ["unknown query"]


# --- Timing --------------------------------------------------------------------------------------------

def script_started_at(entries):
    for entry in entries:
        if entry.event == "Script.Started" and entry.time_of_day is not None:
            date_text = entry.keys.get("date")
            try:
                date = datetime.datetime.strptime(date_text, "%Y-%m-%d").date() if date_text else None
            except ValueError:
                date = None
            if date is None:
                return None
            return datetime.datetime.combine(date, entry.time_of_day)
    return None


def format_duration(seconds):
    if seconds < 0:
        return "unknown (clock went backwards)"
    hours, remainder = divmod(seconds, 3600)
    minutes, seconds = divmod(remainder, 60)
    if hours >= 1:
        return "%dh %dm %.1fs" % (hours, minutes, seconds)
    if minutes >= 1:
        return "%dm %.1fs" % (minutes, seconds)
    return "%.1fs" % seconds


def format_timestamp(moment):
    return moment.strftime("%Y-%m-%d %H:%M:%S.") + "%03d" % (moment.microsecond // 1000)


# --- Main ----------------------------------------------------------------------------------------------

def run_evaluation(queries_path, log_path, commands_path=None, out=sys.stdout):
    """Evaluate and print the report. Returns the exit code (0 pass, 1 fail)."""
    def emit(text=""):
        print(text, file=out)

    try:
        queries, sections = parse_queries(queries_path)
    except QueryFileError as error:
        emit("QUERY FILE ERROR: %s" % error)
        return 1

    if not os.path.isfile(log_path):
        emit("RESULT: FAILED  (no log file at %s)" % log_path)
        emit("Duration: unknown (no Script.Started line)")
        return 1

    entries = parse_log(log_path)
    run = RunInfo(entries)
    test_name = os.path.splitext(os.path.basename(queries_path))[0]

    if commands_path is None:
        commands_path = os.path.splitext(queries_path)[0] + ".commands"

    command_labels = read_command_labels(commands_path) if os.path.isfile(commands_path) else None
    problems = []

    if command_labels is not None:
        known = set(command_labels)
        covered = set()
        for query in queries:
            for label in (query.step, query.by):
                if label is None or label == "all":
                    continue
                if label not in known:
                    problems.append("query line %d names step %r, which is not in %s" % (
                        query.line_number, label, os.path.basename(commands_path)))
                covered.add(label)
        for label in command_labels:
            if label not in covered:
                problems.append("UNVERIFIED step %r: no query proves it worked" % label)

    emit(test_name)

    passed_tests = 0
    for section in sections:
        section_queries = [query for query in queries if query.section == section]
        failures = []
        for query in section_queries:
            passed, details = evaluate(query, run)
            if not passed:
                failures.append((query, details))

        if failures:
            emit("  FAIL  %s" % section)
            for query, details in failures:
                emit("          %s" % query.text)
                for detail in details:
                    emit("            %s" % detail)
        else:
            passed_tests += 1
            emit("  PASS  %s" % section)

    emit()

    failed_commands = [entry for entry in entries if entry.event == "Command.Failed"]
    parse_errors = [entry for entry in entries if entry.event == "Command.ParseError"]
    finished = [entry for entry in entries if entry.event == "Script.Finished"]
    unity_errors = sum(1 for entry in entries if entry.event == "Unity.Error")
    unity_exceptions = sum(1 for entry in entries if entry.event == "Unity.Exception")
    notes = [entry for entry in entries if entry.event == "Command.Note" and "note" in entry.keys]

    for entry in parse_errors:
        problems.append("command file parse error: %s" % entry.keys.get("reason", entry.raw))
    for entry in failed_commands:
        problems.append("step %s failed: %s" % (entry.keys.get("step", "?"), entry.keys.get("reason", "")))

    emit("Run notes:")
    emit("  Script.Finished reason=%s" % (finished[-1].keys.get("reason", "?") if finished else "(missing: the game was killed or crashed)"))
    emit("  Unity errors logged: %d, exceptions: %d (informational)" % (unity_errors, unity_exceptions))
    for entry in notes:
        emit("  step %s: %s" % (entry.keys.get("step", "?"), entry.keys.get("note")))
    for problem in problems:
        emit("  PROBLEM: %s" % problem)

    all_passed = passed_tests == len(sections) and not problems and len(sections) > 0

    started = script_started_at(entries)
    summary_time = datetime.datetime.now()

    if started is None:
        duration_text = "Duration: unknown (no Script.Started line with a date)"
        all_passed = False
    else:
        duration_text = "Duration: %s  (Script.Started %s -> summary %s)" % (
            format_duration((summary_time - started).total_seconds()),
            format_timestamp(started),
            format_timestamp(summary_time))

    emit()
    emit("RESULT: %s (%d/%d tests passed)  %s" % (
        "PASSED" if all_passed else "FAILED", passed_tests, len(sections), duration_text))

    return 0 if all_passed else 1


def make_output_safe():
    """Log lines can hold any character; never let printing one crash the report when stdout is piped."""
    for stream in (sys.stdout, sys.stderr):
        try:
            stream.reconfigure(errors="replace")
        except (AttributeError, ValueError):
            pass


def main():
    make_output_safe()

    parser = argparse.ArgumentParser(description="Evaluate a regression test's queries against its HeadlessLogger log.")
    parser.add_argument("queries", help="the .queries file")
    parser.add_argument("log", help="the HeadlessLogs/<Name>.log file the run wrote")
    parser.add_argument("--commands", help="the .commands file (default: next to the .queries file)")
    arguments = parser.parse_args()

    sys.exit(run_evaluation(arguments.queries, arguments.log, arguments.commands))


if __name__ == "__main__":
    main()

"""Run one headless regression test end to end, then judge it.

Usage:
    python RegressionTests/run_test.py <TestName> [--graphics] [--timeout 900] [--exe <path to game exe>]

Steps:
  1. Deletes <exe dir>/HeadlessData (a headless game keeps its saves and config there, never in the player's
     real folders), so every run starts with no saves and default key bindings, and deletes the old log.
  2. Launches the game with -batchmode -nographics -regressionCommands <TestName>.commands and waits for it
     to quit (killing it at --timeout).
  3. Evaluates <TestName>.queries against HeadlessLogs/<TestName>.log and prints the PASS/FAIL report with
     the run time. Exits 0 on a pass, 1 on a fail.

--graphics drops -nographics, for when a run stalls on code that waits for a rendered frame.
"""

import argparse
import os
import shutil
import subprocess
import sys

TESTS_FOLDER = os.path.dirname(os.path.abspath(__file__))
DEFAULT_EXE = "D:/Fold Builds/Windows/Isometric Alpha.exe"
HEADLESS_DATA_FOLDER_NAME = "HeadlessData"
HEADLESS_LOGS_FOLDER_NAME = "HeadlessLogs"

sys.path.insert(0, TESTS_FOLDER)
import evaluate_queries  # noqa: E402


def read_build_timestamp(exe_path):
    exe_folder = os.path.dirname(exe_path)
    data_folder = os.path.splitext(os.path.basename(exe_path))[0] + "_Data"
    path = os.path.join(exe_folder, data_folder, "StreamingAssets", "Build-Timestamp.txt")

    try:
        with open(path, encoding="utf-8", errors="replace") as timestamp_file:
            return " ".join(timestamp_file.read().split())
    except OSError:
        return "unknown (%s not found)" % path


def remove_headless_data(exe_folder):
    target = os.path.join(exe_folder, HEADLESS_DATA_FOLDER_NAME)

    #only ever the HeadlessData folder next to the exe
    if os.path.basename(target) != HEADLESS_DATA_FOLDER_NAME or os.path.dirname(target) != exe_folder:
        raise RuntimeError("refusing to delete unexpected folder " + target)

    if os.path.isdir(target):
        shutil.rmtree(target)


def main():
    evaluate_queries.make_output_safe()

    parser = argparse.ArgumentParser(description="Run a headless regression test and evaluate its log.")
    parser.add_argument("test", help="test name: the folder under RegressionTests holding <name>.commands and <name>.queries")
    parser.add_argument("--graphics", action="store_true", help="run with -batchmode only (no -nographics)")
    parser.add_argument("--timeout", type=float, default=900, help="seconds before the game is killed (default 900)")
    parser.add_argument("--exe", default=DEFAULT_EXE, help="path to the built game exe")
    arguments = parser.parse_args()

    test_folder = os.path.join(TESTS_FOLDER, arguments.test)
    commands_path = os.path.join(test_folder, arguments.test + ".commands")
    queries_path = os.path.join(test_folder, arguments.test + ".queries")
    exe_path = os.path.abspath(arguments.exe)
    exe_folder = os.path.dirname(exe_path)
    log_folder = os.path.join(exe_folder, HEADLESS_LOGS_FOLDER_NAME)
    log_path = os.path.join(log_folder, arguments.test + ".log")
    player_log_path = os.path.join(log_folder, arguments.test + "-player.log")

    for required in (commands_path, queries_path, exe_path):
        if not os.path.isfile(required):
            print("Missing: " + required)
            return 1

    print("Test:   " + arguments.test)
    print("Build:  " + read_build_timestamp(exe_path))
    print("Exe:    " + exe_path)

    remove_headless_data(exe_folder)
    os.makedirs(log_folder, exist_ok=True)

    for stale in (log_path, player_log_path):
        if os.path.isfile(stale):
            os.remove(stale)

    command = [exe_path, "-batchmode"]
    if not arguments.graphics:
        command.append("-nographics")
    command += ["-logFile", player_log_path, "-regressionCommands", commands_path]

    print("Running the game (up to %.0fs) ..." % arguments.timeout)
    sys.stdout.flush()

    try:
        completed = subprocess.run(command, timeout=arguments.timeout)
        print("Game exited with code %d (the exit code is not used for judging)." % completed.returncode)
    except subprocess.TimeoutExpired:
        print("Game was still running after %.0fs and was killed." % arguments.timeout)

    print("Player log: " + player_log_path)
    print("Test log:   " + log_path)
    print()
    sys.stdout.flush()

    return evaluate_queries.run_evaluation(queries_path, log_path, commands_path)


if __name__ == "__main__":
    sys.exit(main())

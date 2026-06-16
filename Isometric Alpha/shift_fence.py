import re

PATH = "Assets/Resources/SpriteMaps/CampExteriors/Resources/New Perspective NWCamp.prefab"
TARGET = "FenceGrid"
DX = -12
DY = 32

with open(PATH, "r", encoding="utf-8") as f:
    lines = f.read().split("\n")

sep_re = re.compile(r'^--- !u!(\d+) &(\d+)')

docs = []
starts = [i for i, l in enumerate(lines) if sep_re.match(l)]
for n, s in enumerate(starts):
    e = starts[n + 1] if n + 1 < len(starts) else len(lines)
    docs.append({"start": s, "end": e, "anchor": sep_re.match(lines[s]).group(2),
                 "type": lines[s + 1].strip()})

anchor_name = {}
for d in docs:
    if d["type"] == "GameObject:":
        for i in range(d["start"], d["end"]):
            if lines[i].startswith("  m_Name: "):
                anchor_name[d["anchor"]] = lines[i][len("  m_Name: "):].strip()
                break

def tilemap_for(name):
    for d in docs:
        if d["type"] != "Tilemap:":
            continue
        for i in range(d["start"], d["end"]):
            mm = re.match(r'^  m_GameObject: \{fileID: (\d+)\}', lines[i])
            if mm and anchor_name.get(mm.group(1)) == name:
                return d
    raise SystemExit(f"tilemap '{name}' not found")

doc = tilemap_for(TARGET)

first_re  = re.compile(r'^(  - first: \{x: )(-?\d+)(, y: )(-?\d+)(, z: -?\d+\})\s*$')
origin_re = re.compile(r'^(  m_Origin: \{x: )(-?\d+)(, y: )(-?\d+)(, z: -?\d+\})\s*$')

count = 0
for i in range(doc["start"], doc["end"]):
    m = first_re.match(lines[i])
    if m:
        nx = int(m.group(2)) + DX
        ny = int(m.group(4)) + DY
        lines[i] = f"{m.group(1)}{nx}{m.group(3)}{ny}{m.group(5)}"
        count += 1
        continue
    mo = origin_re.match(lines[i])
    if mo:
        ox = int(mo.group(2)) + DX
        oy = int(mo.group(4)) + DY
        lines[i] = f"{mo.group(1)}{ox}{mo.group(3)}{oy}{mo.group(5)}"

print(f"shifted {count} tiles by x{DX:+d} y{DY:+d}")

with open(PATH, "w", encoding="utf-8") as f:
    f.write("\n".join(lines))

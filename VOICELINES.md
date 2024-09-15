# Voicelines

At start of game:
1. Initialize arrays of lines for every room, "room-lines"
2. Initialize array of general lines, "general-lines"
3. Initailize array of alert lines, "alert-lines"

When visiting a point:
1. Initialize temporary array of lines, "temp-lines"
2. Copy over current content of "general-lines" to "temp-lines"
3. If teacher has voice lines for room:
    Copy over current content of "room-lines[room]" to "temp-lines"
4. Pick a random line from "temp-lines", current line
5. Remove line from either "general-lines" or "room-lines[room]"
6. Play voice and print out text of current line

To fine tune random picked voice line:
    Change the ratio of lines from "general-lines" and "room-lines[room]"
    Either by adding more copies of "room-lines[room]"
    or not adding every line from "general-lines"

If "general-lines" is empty AND teacher has general lines:
    Fill "general-lines" with teacher's general lines

If "room-lines[room]" is empty AND teacher has room lines:
    Fill "room-lines[room]" with teacher's room lines

When the player makes a sound:
1. Pick a random line from "alert-lines", current line
2. Remove line from "alert-lines"
3. Play voice and print out text of current line

If "alert-lines" is empty AND teacher has alert lines:
    Fill "alert-lines" with teacher's alert lines

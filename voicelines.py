#
# voicelines.py - voiceline management tool
#
# Written by Hampus Fridholm
#
# Last updated: 2024-09-15
#

from os import listdir
from os.path import isfile, join
from pathlib import Path
import json
from sys import argv

teacher_dir = "FiveNightsAtSpetsen/Assets/Teachers/"

#
# List all the available commands for the teacher menu
#
def menu_teacher_command_help_handler(teacher_name):
    print("- select | Select line type")
    print("- print  | Print %s's voicelines"     % teacher_name)
    print("- add    | Add voiceline")
    print("- del    | Delete voiceline")
    print("- edit   | Edit voiceline")
    print("- tips   | Print tips for voicelines")
    print("- import | Import voiceline")
    print("- back   | Unselect %s"               % teacher_name)
    print("- quit   | Quit")

#
# List all the available commands for the room menu
#
def menu_room_command_help_handler(teacher_name, room_name):
    print("- print  | Print %s's %s voicelines"     % (teacher_name, room_name))
    print("- add    | Add %s voiceline"             % room_name)
    print("- del    | Delete %s voiceline"          % room_name)
    print("- edit   | Edit %s voiceline"            % room_name)
    print("- tips   | Print tips for %s voicelines" % room_name)
    print("- import | Import %s voiceline"          % room_name)
    print("- back   | Unselect %s"                  % room_name)
    print("- quit   | Quit")

#
# List all the available commands for the teachers menu
#
def menu_line_type_command_help_handler(teacher_name, line_type):
    if(line_type == "rooms"):
        print("- select | Select room")

    print("- print  | Print %s's %s voicelines"     % (teacher_name, line_type))
    print("- add    | Add %s voiceline"             % line_type)
    print("- del    | Delete %s voiceline"          % line_type)
    print("- edit   | Edit %s voiceline"            % line_type)
    print("- tips   | Print tips for %s voicelines" % line_type)
    print("- import | Import %s voiceline"          % line_type)
    print("- back   | Unselect %s"                  % line_type)
    print("- quit   | Quit")

#
# List all the available commands for the teachers menu
#
def menu_teachers_command_help_handler():
    print("- select | Select teacher")
    print("- print  | Print teacher's voicelines")
    print("- add    | Add voiceline")
    print("- del    | Delete voiceline")
    print("- edit   | Edit voiceline")
    print("- tips   | Print tips for voicelines")
    print("- import | Import voiceline")
    print("- quit   | Quit")

#
# Get the different generic room names
#
def generic_room_names_get():
    generic_rooms_dir = join(teacher_dir, "Generic", "Rooms")

    room_names = []

    for file in listdir(generic_rooms_dir):
        file_path = join(generic_rooms_dir, file)

        if isfile(file_path):
            room_name = Path(file_path).stem

            room_names.append(room_name)

    return room_names

#
# Get the different generic line types
#
def generic_line_types_get():
    generic_dir = join(teacher_dir, "Generic")

    line_types = []

    for file in listdir(generic_dir):
        file_path = join(generic_dir, file.lower())

        line_type = Path(file_path).stem

        line_types.append(line_type)

    return line_types

#
# Get a list of generic voicelines
#
def generic_lines_get(line_type, room_name = None):
    if(line_type == "rooms"):
        file_path = join(teacher_dir, "Generic", "Rooms", room_name + ".txt")

    else:
        file_path = join(teacher_dir, "Generic", line_type + ".txt")

    try:
        with open(file_path, "r") as file:
            return [line.strip() for line in file.readlines()]

    except (ValueError, FileNotFoundError):
        return []

#
# Get an array of the teacher's names
#
def teacher_names_get():
    teacher_names = []

    for file in listdir(teacher_dir):
        file_path = join(teacher_dir, file)

        if isfile(file_path):
            teacher_name = Path(file_path).stem

            teacher_names.append(teacher_name)

    return teacher_names

#
# Save a teacher json object to file
#
def teacher_json_save(teacher_name, teacher_json):
    file_path = join(teacher_dir, teacher_name + ".json")

    teacher_json = teacher_json_format(teacher_name, teacher_json)

    with open(file_path, "w", encoding="utf-8") as json_file:
        json_file.write(json.dumps(teacher_json, indent=2, ensure_ascii=False))

#
# Load a teacher json object from file
#
def teacher_json_load(teacher_name):
    file_path = join(teacher_dir, teacher_name + ".json")

    try:
        with open(file_path, "r") as json_file:
            return json.load(json_file)

    except (ValueError, FileNotFoundError):
        return {}

#
# Correctly format the teacher's voicelines in a specified room
#
def teacher_room_lines_format(teacher_name, room_name, room_lines):
    for index, room_line in enumerate(room_lines):
        line_voice = "%s-rooms-%s-line-%d.mov" % (teacher_name, room_name, index)

        room_lines[index]["voice"] = line_voice
        room_lines[index]["text"]  = room_lines[index]["text"].strip()

    return room_lines

#
# Correctly format the teacher's voicelines in all rooms
#
def teacher_rooms_json_format(teacher_name, rooms_json):
    empty_rooms = []

    for room_name, room_lines in rooms_json.items():
        if(not room_lines):
            empty_rooms.append(room_name)

        else:
            rooms_json[room_name] = teacher_room_lines_format(teacher_name, room_name, room_lines)

    # Deleting empty rooms without voicelines
    for room_name in empty_rooms:
        del rooms_json[room_name]

    return rooms_json

#
# Correctly format the teacher's other voicelines of a specified type
#
def teacher_other_lines_format(teacher_name, line_type, other_lines):
    for index, line_json in enumerate(other_lines):
        line_voice = "%s-%s-line-%d.mov" % (teacher_name, line_type, index)

        other_lines[index]["voice"] = line_voice
        other_lines[index]["text"]  = other_lines[index]["text"].strip()

    return other_lines

#
# Correctly format the teacher's voicelines
#
def teacher_json_format(teacher_name, teacher_json):
    empty_types = []

    for line_type, type_json in teacher_json.items():
        if(not type_json):
            empty_types.append(line_type)

        elif(line_type == "rooms"):
            teacher_json["rooms"] = teacher_rooms_json_format(teacher_name, type_json)

        else:
            teacher_json[line_type] = teacher_other_lines_format(teacher_name, line_type, type_json)

    # Deleting empty voiceline types without voicelines
    for line_type in empty_types:
        del teacher_json[line_type]

    return teacher_json

#
# Input voiceline type
#
def line_type_input(line_types, command_strings):
    if(len(command_strings) >= 1):
        line_type = command_strings[0]

    else:
        for line_type in line_types:
            print("- %s" % line_type)

        line_type = input("Type: ").strip().lower()

    return line_type

#
# Input room name
#
def room_name_input(room_names, command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

    else:
        for room_name in room_names:
            print("- %s" % room_name)

        room_name = input("Room: ").strip().lower()

    return room_name

#
# Input teacher's name
#
def teacher_name_input(teacher_names, command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        for teacher_name in teacher_names:
            print("- %s" % teacher_name)

        teacher_name = input("Teacher: ").strip().lower()

    return teacher_name

#
# Print the teacher's voicelines in specified room
#
def teacher_room_lines_print(room_name, room_lines):
    print("  - %s" % room_name)
    
    for index, room_line in enumerate(room_lines):
        print("    - %01d: %s" % (index, room_line["text"]))

#
# Print the teacher's voicelines in the different rooms
#
def teacher_rooms_json_print(rooms_json, command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

        if(room_name not in rooms_json.keys()):
            print("Unknown room '%s'." % room_name)
            return

        room_lines = rooms_json[room_name]

        print("- rooms")

        teacher_room_lines_print(room_name, room_lines)

    else:
        print("- rooms")

        for room_name, room_lines in rooms_json.items():
            teacher_room_lines_print(room_name, room_lines)

#
# Print the teacher's other voicelines of specified type
#
def teacher_other_lines_print(line_type, other_lines):
    print("- %s" % line_type)

    for index, line_json in enumerate(other_lines):
        print("  - %01d: %s" % (index, line_json["text"]))

#
# Print all of the teacher's voicelines
#
def teacher_json_print(teacher_json):
    for line_type, type_json in teacher_json.items():
        if(line_type == "rooms"):
            teacher_rooms_json_print(type_json, [])

        else:
            teacher_other_lines_print(line_type, type_json)

#
# Print some or all of the specified teacher's voicelines
#
# Refactor this like tips
#
def teacher_command_print_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    if(len(command_strings) == 0):
        teacher_json_print(teacher_json)

    else:
        line_type = command_strings[0]

        if(line_type not in teacher_json.keys()):
            print("Unknown line type: '%s'." % line_type)
            return


        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


        if(line_type == "rooms"):
            teacher_rooms_json_print(teacher_json["rooms"], next_command_strings)

        else:
            teacher_other_lines_print(line_type, teacher_json[line_type])

#
# Print tips for room
#
def room_tips_print(room_name, generic_lines):
    print("  - %s" % room_name)

    for index, line in enumerate(generic_lines):
        print("    - %02d: %s" % (index, line))

#
# Print tips for all rooms
#
def rooms_tips_print(command_strings):
    room_names = generic_room_names_get()

    if(len(command_strings) >= 1):
        room_name = command_strings[0]

        if(room_name not in room_names):
            print("Unknown room '%s'." % command)
            return

        generic_lines = generic_lines_get("rooms", room_name)
    
        print("- rooms")

        room_tips_print(room_name, generic_lines)

    else:
        print("- rooms")

        for room_name in room_names:
            generic_lines = generic_lines_get("rooms", room_name)
        
            room_tips_print(room_name, generic_lines)

#
# Print tips for other lines
#
def other_tips_print(line_type, generic_lines):
    print("- %s" % line_type)

    for index, line in enumerate(generic_lines):
        print("  - %02d: %s" % (index, line))

#
# Print all tips
#
def all_tips_print():
    line_types = generic_line_types_get()

    for line_type in line_types:
        if(line_type == "rooms"):
            rooms_tips_print([])

        else:
            generic_lines = generic_lines_get(line_type)

            other_tips_print(line_type, generic_lines)

#
# Print out tips for voicelines
#
def command_tips_handler(command_strings):
    line_types = generic_line_types_get()

    if(len(command_strings) == 0):
        all_tips_print()

    else:
        line_type = command_strings[0]

        if(line_type not in line_types):
            print("Unknown line type: '%s'." % line_type)
            return


        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


        if(line_type == "rooms"):
            rooms_tips_print(next_command_strings)

        else:
            generic_lines = generic_lines_get(line_type)

            other_tips_print(line_type, generic_lines)

#
# Import a generic voiceline to teacher's specified room
#
# Maybe: Remove generic_lines as argument and create local variable instead
#
def teacher_room_lines_line_import(room_name, room_lines, generic_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        print("- rooms")

        room_tips_print(room_name, generic_lines)

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("No inputted index")
            return room_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("Index must be an integer")
        return room_lines


    if(line_index < 0 or line_index >= len(generic_lines)):
        print("Index out of range")
        return room_lines


    line_text = generic_lines[line_index]

    room_lines.append({"text": line_text})

    print("Imported line: '%s'" % line_text)


    return room_lines

#
# Import a generic voiceline to one of teacher's rooms
#
def teacher_rooms_json_line_import(rooms_json, command_strings):
    room_names = generic_room_names_get()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("No inputted room")
        return rooms_json

    if(room_name not in room_names):
        print("Unknown room: '%s'." % room_name)
        return rooms_json


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    generic_lines = generic_lines_get("rooms", room_name)

    room_lines = rooms_json.get(room_name, [])

    rooms_json[room_name] = teacher_room_lines_line_import(room_name, room_lines, generic_lines, next_command_strings)


    return rooms_json

#
# Import a generic voiceline to one of teacher's other lines
#
def teacher_other_lines_line_import(line_type, other_lines, generic_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        other_tips_print(line_type, generic_lines)
        
        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("No inputted index")
            return other_lines

    
    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("Index must be an integer")
        return other_lines


    if(line_index < 0 or line_index >= len(generic_lines)):
        print("Index out of range")
        return other_lines


    line_text = generic_lines[line_index]

    other_lines.append({"text": line_text})

    print("Imported line: '%s'" % line_text)


    return other_lines

#
# Import a voiceline for a specified teacher
#
def teacher_command_import_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return

    line_types = generic_line_types_get()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("No inputted line type")
        return

    if(line_type not in line_types):
        print("Unknown line type: '%s'." % line_type)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_import(teacher_json["rooms"], next_command_strings)

    else:
        generic_lines = generic_lines_get(line_type)

        other_lines = teacher_json.get(line_type, [])

        teacher_json[line_type] = teacher_other_lines_line_import(line_type, other_lines, generic_lines, next_command_strings)


    teacher_json_save(teacher_name, teacher_json)

#
# Add a voiceline to one of teacher's rooms
#
def teacher_rooms_json_line_add(teacher_name, rooms_json, command_strings):
    room_names = generic_room_names_get()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("No inputted room")
        return rooms_json

    if(room_name not in room_names):
        print("Unknown room: '%s'." % room_name)
        return rooms_json


    room_lines = rooms_json.get(room_name, [])

    teacher_room_lines_print(room_name, room_lines)

    line_text = input("Text: ").strip()

    if(not line_text):
        print("No inputted line text")
        return rooms_json


    room_lines = rooms_json.get(room_name, [])

    room_lines.append({"text": line_text})

    rooms_json[room_name] = room_lines

    print("Added line: '%s'" % line_text)


    return rooms_json

#
# Add another voiceline to teacher
#
def teacher_other_lines_line_add(teacher_name, line_type, other_lines):

    teacher_other_lines_print(line_type, other_lines)

    line_text = input("Text: ").strip()

    if(not line_text):
        print("No inputted line text")
        return other_lines


    other_lines.append({"text": line_text})

    print("Added line: '%s'" % line_text)

    return other_lines

#
# Add a voiceline for a specified teacher
#
def teacher_command_add_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    line_types = generic_line_types_get()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("No inputted line type")
        return

    if(line_type not in line_types):
        print("Unknown line type: '%s'." % line_type)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_add(teacher_name, teacher_json["rooms"], next_command_strings)

    else:
        other_lines = teacher_json.get(line_type, [])

        teacher_json[line_type] = teacher_other_lines_line_add(teacher_name, line_type, other_lines)


    teacher_json_save(teacher_name, teacher_json)

#
# Edit one of the teacher's voicelines in a specified room
#
# Rename this function
#
def teacher_room_lines_line_edit(room_name, room_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        teacher_room_lines_print(room_name, room_lines)

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("No inputted index")
            return room_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("Index must be an integer")
        return room_lines


    if(line_index < 0 or line_index >= len(room_lines)):
        print("Index out of range")
        return room_lines


    line_text = input("Text: ").strip()

    if(not line_text):
        print("No inputted line text")
        return room_lines


    room_lines[line_index]["text"] = line_text

    print("Edited line: '%s'" % line_text)

    return room_lines

#
# Edit one of the teacher's voicelines in a room
#
# Rename this function
#
def teacher_rooms_json_line_edit(rooms_json, command_strings):
    room_names = rooms_json.keys()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("No inputted room")
        return rooms_json

    if(room_name not in room_names):
        print("Unknown room: '%s'." % room_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    rooms_json[room_name] = teacher_room_lines_line_edit(room_name, rooms_json[room_name], next_command_strings)

    return rooms_json

#
# Edit one of the teacher's other voicelines of a specified type
#
# Rename this function
#
def teacher_other_lines_line_edit(line_type, other_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        teacher_other_lines_print(line_type, other_lines)

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("No inputted index")
            return other_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("Index must be an integer")
        return other_lines


    if(line_index < 0 or line_index >= len(other_lines)):
        print("Index out of range")
        return other_lines


    line_text = input("Text: ").strip()

    if(not line_text):
        print("No inputted line text")
        return other_lines


    other_lines[line_index]["text"] = line_text

    print("Edited line: '%s'" % line_text)

    return other_lines

#
# Delete one of the teacher's voicelines in a specified room
#
# Rename this function
#
def teacher_room_lines_line_del(room_name, room_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        teacher_room_lines_print(room_name, room_lines)

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("No inputted index")
            return room_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("Index must be an integer")
        return room_lines


    if(line_index < 0 or line_index >= len(room_lines)):
        print("Index out of range")
        return room_lines


    line_text = room_lines[line_index]["text"]

    del room_lines[line_index]

    print("Deleted line: '%s'" % line_text)


    return room_lines

#
# Delete one of the teacher's voicelines in a room
#
# Rename this function
#
def teacher_rooms_json_line_del(rooms_json, command_strings):
    room_names = rooms_json.keys()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("No inputted room")
        return rooms_json

    if(room_name not in room_names):
        print("Unknown room: '%s'." % room_name)
        return rooms_json


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    rooms_json[room_name] = teacher_room_lines_line_del(room_name, rooms_json[room_name], next_command_strings)

    return rooms_json

#
# Delete one of the teacher's other voicelines of a specified type
#
# Rename this function
#
def teacher_other_lines_line_del(line_type, other_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        teacher_other_lines_print(line_type, other_lines)

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("No inputted index")
            return other_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("Index must be an integer")
        return other_lines

    
    if(line_index < 0 or line_index >= len(other_lines)):
        print("Index out of range")
        return other_lines


    line_text = other_lines[line_index]["text"]

    del other_lines[line_index]

    print("Deleted line: '%s'" % line_text)


    return other_lines

#
# Edit one of the teacher's voicelines
#
def teacher_command_edit_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    line_types = teacher_json.keys()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("No inputted line type")
        return

    if(line_type not in line_types):
        print("Unknown line type: '%s'." % line_type)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_edit(teacher_json["rooms"], next_command_strings)

    else:
        teacher_json[line_type] = teacher_other_lines_line_edit(line_type, teacher_json[line_type], next_command_strings)


    teacher_json_save(teacher_name, teacher_json)

#
# Delete one of the teacher's voicelines
#
def teacher_command_del_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    line_types = teacher_json.keys()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("No inputted line type")
        return

    if(line_type not in line_types):
        print("Unknown line type: '%s'." % line_type)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_del(teacher_json["rooms"], next_command_strings)

    else:
        teacher_json[line_type] = teacher_other_lines_line_del(line_type, teacher_json[line_type], next_command_strings)


    teacher_json_save(teacher_name, teacher_json)

#
# Parse the inputted command in the teacher menu
#
def menu_teacher_command_parse(teacher_name, command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    if(command == "quit"):
        exit()

    elif(command == "back"):
        print("Unselected %s" % teacher_name)

        menu_teachers_routine()

    elif(command == "help"):
        menu_teacher_command_help_handler(teacher_name)

    else:
        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

        full_command_strings = [teacher_name] + next_command_strings

        command_handler(command, full_command_strings)

#
# This is the routine for the teacher menu
#
def menu_teacher_routine(teacher_name):
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_teacher_command_parse(teacher_name, command_string)

#
#
#
def teacher_rooms_command_select_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    room_names = teacher_json["rooms"].keys()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("No inputted room")
        return

    if(room_name not in room_names):
        print("Unknown room: '%s'." % room_name)
        return

    print("Selected %s's %s voicelines" % (teacher_name, room_name))
    menu_room_routine(teacher_name, room_name)

#
#
#
def teacher_command_select_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    line_types = teacher_json.keys()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("No inputted line type")
        return

    if(line_type not in line_types):
        print("Unknown line type: '%s'." % line_type)
        return


    if(line_type == "rooms"):
        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

        teacher_rooms_command_select_handler(teacher_name, next_command_strings)


    print("Selected %s's %s voicelines" % (teacher_name, line_type))
    menu_line_type_routine(teacher_name, line_type)

#
# Select a teacher and go to the teacher menu
#
def command_select_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_select_handler(teacher_name, next_command_strings)


    print("Selected teacher: %s" % teacher_name)
    menu_teacher_routine(teacher_name)

#
#
#
def command_edit_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return
        

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_edit_handler(teacher_name, next_command_strings)

#
#
#
def command_del_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return

        
    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_del_handler(teacher_name, next_command_strings)

#
#
#
def command_import_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return


    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_import_handler(teacher_name, next_command_strings)

#
#
#
def command_add_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return

        
    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_add_handler(teacher_name, next_command_strings)

#
#
#
def command_print_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return

        
    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_print_handler(teacher_name, next_command_strings)

#
# Parse the inputted command in the room menu
#
def menu_room_command_parse(teacher_name, room_name, command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    if(command == "quit"):
        exit()

    if(command == "back"):
        print("Unselected %s" % room_name)

        menu_line_type_routine(teacher_name, "rooms")

    elif(command == "help"):
        menu_room_command_help_handler(teacher_name, room_name)

    else:
        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

        full_command_strings = [teacher_name, "rooms", room_name] + next_command_strings

        command_handler(command, full_command_strings)

#
#
#
def menu_room_routine(teacher_name, room_name):
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_room_command_parse(teacher_name, room_name, command_string)

#
# command and next_command_strings are divided,
# so that the caller can choose which level to use this function from
#
def command_handler(command, next_command_strings):
    if(command == "select"):
        command_select_handler(next_command_strings)

    elif(command == "del"):
        command_del_handler(next_command_strings)

    elif(command == "add"):
        command_add_handler(next_command_strings)

    elif(command == "edit"):
        command_edit_handler(next_command_strings)

    elif(command == "print"):
        command_print_handler(next_command_strings)

    elif(command == "tips"):
        command_tips_handler(next_command_strings)

    elif(command == "import"):
        command_import_handler(next_command_strings)

    else:
        print("Unknown command '%s'. Type 'help' for help." % command)

#
# Parse the inputted command in the teachers menu
#
def menu_line_type_command_parse(teacher_name, line_type, command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    if(command == "quit"):
        exit()

    elif(command == "back"):
        print("Unselected %s voicelines" % line_type)

        menu_teacher_routine(teacher_name)

    elif(command == "help"):
        menu_line_type_command_help_handler(teacher_name, line_type)

    else:
        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

        full_command_strings = [teacher_name, line_type] + next_command_strings

        command_handler(command, full_command_strings)

#
#
#
def menu_line_type_routine(teacher_name, line_type):
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_line_type_command_parse(teacher_name, line_type, command_string)

#
# Parse the inputted command in the teachers menu
#
def menu_teachers_command_parse(command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    if(command == "quit"):
        exit()

    elif(command == "help"):
        menu_teachers_command_help_handler()

    else:
        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

        command_handler(command, next_command_strings)

#
# This is the routine for the teachers menu
#
def menu_teachers_routine():
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_teachers_command_parse(command_string)

#
# This is like the main function
#
if(__name__ == "__main__"):
    next_command_strings = argv[1:] if len(argv) >= 2 else []

    if(len(argv) > 1):
        command_select_handler(argv[1:])

    else:
        menu_teachers_routine()

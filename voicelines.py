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
def teacher_name_input(command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        teacher_names = teacher_names_get()

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
def menu_teacher_command_print_handler(teacher_name, command_strings):
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
            print("Unknown room '%s'." % command_strings[0])
            return

        print("- rooms")

        generic_lines = generic_lines_get("rooms", room_name)
    
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
        room_tips_print(room_lines, generic_lines)

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

    room_lines.append({"text": line_text, "voice": ""})

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

    other_lines.append({"text": line_text, "voice": ""})

    print("Imported line: '%s'" % line_text)


    return other_lines

#
# Import a voiceline for a specified teacher
#
def menu_teacher_command_import_handler(teacher_name, command_strings):
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
def teacher_rooms_json_line_add(rooms_json, command_strings, line_text):
    room_names = generic_room_names_get()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("No inputted room")
        return rooms_json

    if(room_name not in room_names):
        print("Unknown room: '%s'." % room_name)
        return rooms_json


    room_lines = rooms_json.get(room_name, [])

    room_lines.append({"text": line_text, "voice": ""})

    rooms_json[room_name] = room_lines

    print("Added line: '%s'" % line_text)


    return rooms_json

#
# Add another voiceline to teacher
#
def teacher_other_lines_line_add(line_type, other_lines, line_text):
    other_lines.append({"text": line_text, "voice": ""})

    print("Added line: '%s'" % line_text)

    return other_lines

#
# Add a voiceline for a specified teacher
#
def menu_teacher_command_add_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    line_text = input("Text: ").strip()

    if(not line_text):
        print("No inputted line text")
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
        teacher_json["rooms"] = teacher_rooms_json_line_add(teacher_json["rooms"], next_command_strings, line_text)

    else:
        other_lines = teacher_json.get(line_type, [])

        teacher_json[line_type] = teacher_other_lines_line_add(line_type, other_lines, line_text)


    teacher_json_save(teacher_name, teacher_json)

#
# Correctly format the teacher's voicelines in a specified room
#
def teacher_room_lines_format(teacher_name, room_name, room_lines):
    for index, room_line in enumerate(room_lines):
        line_voice = "%s-rooms-%s-line-%d.mov" % (teacher_name, room_name, index)

        room_lines[index]["voice"] = line_voice
        room_lines[index]["text"] = room_lines[index]["text"].strip()

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
# Set one of the teacher's voicelines in a specified room
#
def teacher_room_lines_line_set(room_name, room_lines, command_strings):
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

    return room_lines

#
# Set one of the teacher's voicelines in a room
#
def teacher_rooms_json_line_set(rooms_json, command_strings):
    room_names = rooms_json.keys()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("No inputted room")
        return rooms_json

    if(room_name not in room_names):
        print("Unknown room: '%s'." % room_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    rooms_json[room_name] = teacher_room_lines_line_set(room_name, rooms_json[room_name], next_command_strings)

    return rooms_json

#
# Set one of the teacher's other voicelines of a specified type
#
def teacher_other_lines_line_set(line_type, other_lines, command_strings):
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

    return other_lines

#
# Delete one of the teacher's voicelines in a specified room
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
# Set one of the teacher's voicelines
#
def menu_teacher_command_set_handler(teacher_name, command_strings):
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
        teacher_json["rooms"] = teacher_rooms_json_line_set(teacher_json["rooms"], next_command_strings)

    else:
        teacher_json[line_type] = teacher_other_lines_line_set(line_type, teacher_json[line_type], next_command_strings)


    teacher_json_save(teacher_name, teacher_json)

#
# Delete one of the teacher's voicelines
#
def menu_teacher_command_del_handler(teacher_name, command_strings):
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
# List all the available commands for the teacher menu
#
def menu_teacher_command_help_handler():
    print("- print  | Print lines")
    print("- add    | Add line")
    print("- del    | Delete line")
    print("- set    | Set line")
    print("- tips   | Print line tips")
    print("- import | Import line")
    print("- select | Select another teacher")
    print("- format | Format json file")
    print("- back   | Unselect teacher")
    print("- quit   | Quit")

def menu_teacher_command_format_handler(teacher_name):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return

    teacher_json_save(teacher_name, teacher_json)

#
# Parse the inputted command in the teacher menu
#
def menu_teacher_command_parse(teacher_name, command_string):
    command_strings = command_string.split(" ")

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(command_strings[0] == "quit"):
        exit()

    elif(command_strings[0] == "back"):
        print("Unselected teacher: %s" % teacher_name)

        menu_teachers()

    elif(command_strings[0] == "help"):
        menu_teacher_command_help_handler()

    elif(command_strings[0] == "print"):
        menu_teacher_command_print_handler(teacher_name, next_command_strings)

    elif(command_strings[0] == "tips"):
        command_tips_handler(next_command_strings)

    elif(command_strings[0] == "add"):
        menu_teacher_command_add_handler(teacher_name, next_command_strings)

    elif(command_strings[0] == "import"):
        menu_teacher_command_import_handler(teacher_name, next_command_strings)

    elif(command_strings[0] == "format"):
        menu_teacher_command_format_handler(teacher_name)

    elif(command_strings[0] == "set"):
        menu_teacher_command_set_handler(teacher_name, next_command_strings)

    elif(command_strings[0] == "del"):
        menu_teacher_command_del_handler(teacher_name, next_command_strings)

    elif(command_strings[0] == "select"):
        menu_teachers_command_select_handler(next_command_strings)

    else:
        print("Unknown command '%s'. Type 'help' for help." % command_string)

#
# This is the routine for the teacher menu
#
def menu_teacher(teacher_name):
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_teacher_command_parse(teacher_name, command_string)


# Teachers menu

#
# List all the teachers
#
def menu_teachers_command_list_handler():
    teacher_names = teacher_names_get()
    
    for teacher_name in teacher_names:
        print("- %s" % teacher_name)

#
# Select a teacher and go to the teacher menu
#
def menu_teachers_command_select_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return


    if(teacher_name in teacher_names):
        print("Selected teacher: %s" % teacher_name)

        menu_teacher(teacher_name)

    else:
        print("Teacher '%s' doesn't exist" % teacher_name)

#
# Format the voicelines for a teacher or for all teachers
#
def menu_teachers_command_format_handler(command_strings):
    teacher_names = teacher_names_get()
    
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

        if(teacher_name not in teacher_names):
            print("Teacher '%s' doesn't exist" % teacher_name)
            return

        menu_teacher_command_format_handler(teacher_name)

    else:
        for teacher_name in teacher_names:
            menu_teacher_command_format_handler(teacher_name)

#
#
#
def menu_teachers_command_set_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return
        

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    menu_teacher_command_set_handler(teacher_name, next_command_strings)

#
#
#
def menu_teachers_command_del_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return

        
    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    menu_teacher_command_del_handler(teacher_name, next_command_strings)

#
#
#
def menu_teachers_command_import_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return


    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    menu_teacher_command_import_handler(teacher_name, next_command_strings)

#
#
#
def menu_teachers_command_add_handler(command_strings):
    teacher_name = teacher_name_input(command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return

        
    teacher_names = teacher_names_get()

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    menu_teacher_command_add_handler(teacher_name, next_command_strings)

#
#
#
def menu_teachers_command_print_handler(command_strings):
    teacher_name = teacher_name_input(command_strings)

    if(not teacher_name):
        print("No inputted teacher")
        return

        
    teacher_names = teacher_names_get()

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    menu_teacher_command_print_handler(teacher_name, next_command_strings)

#
# List all the available commands for the teachers menu
#
def menu_teachers_command_help_handler():
    print("- list   | List teachers")
    print("- select | Select teacher")
    print("- print  | Print lines")
    print("- add    | Add line")
    print("- del    | Delete line")
    print("- set    | Set line")
    print("- tips   | Print line tips")
    print("- import | Import line")
    print("- format | Format json files")
    print("- quit   | Quit")

#
# Parse the inputted command in the teachers menu
#
def menu_teachers_command_parse(command_string):
    command_strings = command_string.split(" ")

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(command_strings[0] == "quit"):
        exit()

    elif(command_strings[0] == "help"):
        menu_teachers_command_help_handler()

    elif(command_strings[0] == "list"):
        menu_teachers_command_list_handler()

    elif(command_strings[0] == "select"):
        menu_teachers_command_select_handler(next_command_strings)

    elif(command_strings[0] == "format"):
        menu_teachers_command_format_handler(next_command_strings)

    elif(command_strings[0] == "del"):
        menu_teachers_command_del_handler(next_command_strings)

    elif(command_strings[0] == "add"):
        menu_teachers_command_add_handler(next_command_strings)

    elif(command_strings[0] == "set"):
        menu_teachers_command_set_handler(next_command_strings)

    elif(command_strings[0] == "print"):
        menu_teachers_command_print_handler(next_command_strings)

    elif(command_strings[0] == "tips"):
        command_tips_handler(next_command_strings)

    elif(command_strings[0] == "import"):
        menu_teachers_command_import_handler(next_command_strings)

    else:
        print("Unknown command '%s'. Type 'help' for help." % command_strings[0])

#
# This is the routine for the teachers menu
#
def menu_teachers():
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_teachers_command_parse(command_string)

#
# This is like the main function
#
if(__name__ == "__main__"):
    if(len(argv) > 1):
        menu_teacher(argv[1])

    else:
        menu_teachers()

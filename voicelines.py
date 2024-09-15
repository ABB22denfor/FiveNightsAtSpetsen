#
# voicelines.py - voiceline management tool
#
# Written by Hampus Fridholm
#
# Last updated: 2024-09-14
#

from os import listdir
from os.path import isfile, join
from pathlib import Path
import json
import sys

debug = False

teacher_dir = "FiveNightsAtSpetsen/Assets/Teachers/"

#
# Get a list of generic voicelines in specified room
#
def generic_room_lines_get(command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

    else:
        room_name = input("Room: ").strip().lower()

    file_path = join(teacher_dir, "Generic", "Rooms", room_name + ".txt")

    try:
        with open(file_path, "r") as file:
            return [line.strip() for line in file.readlines()]

    except (ValueError, FileNotFoundError):
        return []

#
# Get a list of other generic voicelines
#
def generic_other_lines_get(line_type):
    file_path = join(teacher_dir, "Generic", line_type + ".txt")

    try:
        with open(file_path, "r") as file:
            return [line.strip() for line in file.readlines()]

    except (ValueError, FileNotFoundError):
        return []

#
# Get a list of generic voicelines
#
def generic_lines_get(command_strings):
    if(len(command_strings) >= 1):
        line_type = command_strings[0]
    
    else:
        line_type = input("Type: ").strip().lower()


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    if(line_type == "rooms"):
        return generic_room_lines_get(next_command_strings)

    else:
        return generic_other_lines_get(line_type)

#
# Get an array of the teacher names
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
# Print the teacher's voicelines in the different rooms
#
def teacher_rooms_json_print(rooms_json, command_strings):
    if(len(command_strings) >= 1):
        try:
            room_name = command_strings[1]

            room_lines = rooms_json[room_name]

            print("- rooms")

            teacher_room_lines_print(room_name, room_lines)

        except Exception as exception:
            if(debug):
                print(exception)
            print("Unknown room '%s'." % command_strings[1])

    else:
        print("- rooms")

        for room_name, room_lines in rooms_json.items():
            print("  - %s" % room_name)
            
            for index, room_line in enumerate(room_lines):
                print("    - %01d: %s" % (index, room_line["text"]))

#
# Print the teacher's voicelines in specified room
#
def teacher_room_lines_print(room_name, room_lines):
    print("  - %s" % room_name)
    
    for index, room_line in enumerate(room_lines):
        print("    - %01d: %s" % (index, room_line["text"]))

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
            teacher_rooms_json_print(type_json, {})

        else:
            teacher_other_lines_print(line_type, type_json)

#
# Print some or all of the specified teacher's voicelines
#
def menu_teacher_command_print_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("Teacher '%s' has no information" % teacher_name)
        return


    if(len(command_strings) == 0):
        teacher_json_print(teacher_json)
        return

    else:
        line_type = command_strings[0]

        try:
            next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

            if(line_type == "rooms"):
                teacher_rooms_json_print(teacher_json["rooms"], next_command_strings)

            else:
                teacher_other_lines_print(line_type, teacher_json[line_type])

        except Exception as exception:
            if(debug):
                print(exception)
            print("Unknown line type: '%s'." % line_type)

#
# Import a generic voiceline to teacher's specified room
#
def teacher_room_lines_line_import(room_lines, generic_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        line_index_string = input("Index: ").strip().lower()


    try:
        room_lines.append(generic_lines[int(line_index_string)])

    except Exception as exception:
        if(debug):
            print(exception)
        print("No line with index [%s]" % line_index_string)

    return room_lines

#
# Import a generic voiceline to one of teacher's rooms
#
def teacher_rooms_json_line_import(teacher_name, command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

    else:
        room_name = input("Room: ").strip().lower()

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    try:
        generic_lines = generic_lines_get(["rooms"])

        rooms_json[room_name] = teacher_room_lines_line_import(rooms_json[room_name], generic_lines, next_command_strings)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown room: '%s'." % room_name)

    return rooms_json

#
# Import a generic voiceline to one of teacher's other lines
#
def teacher_other_lines_line_import(teacher_name, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        line_index_string = input("Index: ").strip().lower()

    try:
        room_lines.append(generic_lines[int(line_index_string)])

    except Exception as exception:
        if(debug):
            print(exception)
        print("No line with index [%s]" % line_index_string)

    return other_lines

#
# Import a voiceline for a specified teacher
#
def menu_teacher_command_import_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(len(command_strings) >= 1):
        line_type = command_strings[0]

    else:
        line_type = input("Type: ").strip().lower()

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    try:
        if(line_type == "rooms"):
            teacher_json["rooms"] = teacher_rooms_json_line_import(teacher_json["rooms"], next_command_strings)

        else:
            teacher_json[line_type] = teacher_other_lines_line_import(teacher_json[line_type], next_command_strings)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown line type: '%s'." % line_type)

    teacher_json_save(teacher_name, teacher_json)

#
# Add a voiceline to one of teacher's rooms
#
def teacher_rooms_json_line_add(rooms_json, command_strings, line_text):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

    else:
        room_name = input("Room: ").strip().lower()


    try:
        line_json = {
            "text" : line_text,
            "voice": ""
        }

        teacher_json["rooms"][room_name].append(line_json)

        print("Added line in room '%s'." % room_name)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown room: '%s'." % room_name)

    return rooms_json

#
# Add another voiceline to teacher
#
def teacher_other_lines_line_add(other_lines, line_text):
    try:
        line_json = {
            "text" : line_text,
            "voice": ""
        }

        teacher_json[line_type].append(line_json)

        print("Added line in '%s'." % line_type)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown line type: '%s'." % line_type)

    return other_lines

#
# Add a voiceline for a specified teacher
#
def menu_teacher_command_add_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    line_text = input("Text: ").strip()

    if(len(command_strings) >= 1):
        line_type = command_strings[0]

    else:
        line_type = input("Type: ").strip().lower()


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_add(teacher_json["rooms"], next_command_strings, line_text)

    else:
        teacher_json[line_type] = teacher_other_lines_line_add(teacher_json[line_type], line_text)

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
    for room_name, room_lines in rooms_json.items():
        rooms_json[room_name] = teacher_room_lines_format(teacher_name, room_name, room_lines)

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
    for line_type, type_json in teacher_json.items():
        if(line_type == "rooms"):
            teacher_json["rooms"] = teacher_rooms_json_format(teacher_name, type_json)

        else:
            teacher_json[line_type] = teacher_other_lines_format(teacher_name, line_type, type_json)

    return teacher_json

#
# Edit one of the teacher's voicelines in a specified room
#
def teacher_room_lines_line_set(room_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        line_index_string = input("Index: ").strip().lower()

    try:
        line_text = input("Text: ").strip()

        room_lines[int(line_index_string)]["text"] = line_text

    except Exception as exception:
        if(debug):
            print(exception)
        print("No line with index [%s]" % line_index_string)

    return room_lines

#
# Edit one of the teacher's voicelines in a room
#
def teacher_rooms_json_line_set(rooms_json, command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

    else:
        room_name = input("Room: ").strip().lower()

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    try:
        rooms_json[room_name] = teacher_room_lines_line_set(rooms_json[room_name], next_command_strings)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown room: '%s'." % room_name)

    return rooms_json

#
# Edit one of the teacher's other voicelines of a specified type
#
def teacher_other_lines_line_set(other_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        line_index_string = input("Index: ").strip().lower()

    try:
        line_text = input("Text: ").strip()

        other_lines[int(line_index_string)]["text"] = line_text

    except Exception as exception:
        if(debug):
            print(exception)
        print("No line with index [%s]" % line_index_string)

    return other_lines

#
# Delete one of the teacher's voicelines in a specified room
#
def teacher_room_lines_line_del(room_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        line_index_string = input("Index: ").strip().lower()

    try:
        del room_lines[int(line_index_string)]

    except Exception as exception:
        if(debug):
            print(exception)
        print("No line with index [%s]" % line_index_string)

    return room_lines

#
# Delete one of the teacher's voicelines in a room
#
def teacher_rooms_json_line_del(rooms_json, command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

    else:
        room_name = input("Room: ").strip().lower()

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    try:
        rooms_json[room_name] = teacher_room_lines_line_del(rooms_json[room_name], next_command_strings)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown room: '%s'." % room_name)

    return rooms_json

#
# Edit one of the teacher's other voicelines of a specified type
#
def teacher_other_lines_line_del(other_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        line_index_string = input("Index: ").strip().lower()

    try:
        del other_lines[int(line_index_string)]

    except Exception as exception:
        if(debug):
            print(exception)
        print("No line with index [%s]" % line_index_string)

    return other_lines

#
# Edit one of the teacher's voicelines
#
def menu_teacher_command_set_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(len(command_strings) >= 1):
        line_type = command_strings[0]

    else:
        line_type = input("Type: ").strip().lower()

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    try:
        if(line_type == "rooms"):
            teacher_json["rooms"] = teacher_rooms_json_line_set(teacher_json["rooms"], next_command_strings)

        else:
            teacher_json[line_type] = teacher_other_lines_line_set(teacher_json[line_type], next_command_strings)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown line type: '%s'." % line_type)

    teacher_json_save(teacher_name, teacher_json)

#
# Delete one of the teacher's voicelines
#
def menu_teacher_command_del_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(len(command_strings) >= 1):
        line_type = command_strings[0]

    else:
        line_type = input("Type: ").strip().lower()

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    try:
        if(line_type == "rooms"):
            teacher_json["rooms"] = teacher_rooms_json_line_del(teacher_json["rooms"], next_command_strings)

        else:
            teacher_json[line_type] = teacher_other_lines_line_del(teacher_json[line_type], next_command_strings)

    except Exception as exception:
        if(debug):
            print(exception)
        print("Unknown line type: '%s'." % line_type)

    teacher_json_save(teacher_name, teacher_json)

#
# List all the available commands for the teacher menu
#
def menu_teacher_command_help_handler():
    print("- print  | Display teacher's lines")
    print("- format | Format json file")
    print("- add    | Add a line")
    print("- del    | Delete a line")
    print("- set    | Edit a line")
    print("- import | Import a line")
    print("- select | Select another teacher")
    print("- back   | Unselect teacher")
    print("- quit   | Quit the program")

def menu_teacher_command_format_handler(teacher_name):
    teacher_json = teacher_json_load(teacher_name)

    teacher_json_save(teacher_name, teacher_json)

#
# Parse the inputted command in the teacher menu
#
def menu_teacher_command_parse(teacher_name, command_string):
    command_strings = command_string.split(" ")

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}


    if(command_strings[0] == "quit"):
        exit()

    elif(command_strings[0] == "back"):
        menu_teachers()

    elif(command_strings[0] == "help"):
        menu_teacher_command_help_handler()

    elif(command_strings[0] == "print"):
        menu_teacher_command_print_handler(teacher_name, next_command_strings)

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
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        teacher_name = input("Teacher: ").strip().lower()

    teacher_names = teacher_names_get()
    
    if(teacher_name in teacher_names):
        menu_teacher(teacher_name)

    else:
        print("Teacher '%s' doesn't exist" % teacher_name)

#
# Format the voicelines for a teacher or for all teachers
#
def menu_teachers_command_format_handler(command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[1]

        teacher_names = teacher_names_get()

        if(teacher_name not in teacher_names):
            print("Teacher '%s' doesn't exist" % teacher_name)
            return

        menu_teacher_command_format_handler(teacher_name)

    else:
        teacher_names = teacher_names_get()
    
        for teacher_name in teacher_names:
            menu_teacher_command_format_handler(teacher_name)

def menu_teachers_command_set_handler(command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        teacher_name = input("Teacher: ").strip().lower()
        
    teacher_names = teacher_names_get()

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    menu_teacher_command_set_handler(teacher_name, next_command_strings)

def menu_teachers_command_del_handler(command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        teacher_name = input("Teacher: ").strip().lower()
        
    teacher_names = teacher_names_get()

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    menu_teacher_command_del_handler(teacher_name, next_command_strings)

def menu_teachers_command_add_handler(command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        teacher_name = input("Teacher: ").strip().lower()
        
    teacher_names = teacher_names_get()

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    menu_teacher_command_add_handler(teacher_name, next_command_strings)

def menu_teachers_command_print_handler(command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        teacher_name = input("Teacher: ").strip().lower()
        
    teacher_names = teacher_names_get()

    if(teacher_name not in teacher_names):
        print("Teacher '%s' doesn't exist" % teacher_name)
        return

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}

    menu_teacher_command_print_handler(teacher_name, next_command_strings)

#
# List all the available commands for the teachers menu
#
def menu_teachers_command_help_handler():
    print("- list   | List teachers")
    print("- select | Select a teacher")
    print("- format | Format json files")
    print("- print  | Display teacher's lines")
    print("- add    | Add a line")
    print("- del    | Delete a line")
    print("- set    | Edit a line")
    print("- quit   | Quit the program")

#
# Parse the inputted command in the teachers menu
#
def menu_teachers_command_parse(command_string):
    command_strings = command_string.split(" ")

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else {}


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
        generic_lines = generic_lines_get(next_command_strings)

        for index, line in enumerate(generic_lines):
            print("%01d: %s" % (index, line))

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

if(__name__ == "__main__"):
    args = sys.argv[1:]

    if(len(args) > 0):
        menu_teacher(args[0])

    else:
        menu_teachers()

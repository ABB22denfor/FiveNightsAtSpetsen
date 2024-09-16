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

# Section 01: Command parsing

#
# List all the available commands for the teacher menu
#
def menu_teacher_command_help_handler(teacher_name):
    print("\nYou have selected the teacher '%s'\n" % teacher_name)

    print("- select | Select voiceline type")
    print("- print  | Print %s's voicelines"     % teacher_name)
    print("- add    | Add voiceline")
    print("- del    | Delete voiceline")
    print("- edit   | Edit voiceline")
    print("- tips   | Print tips for voicelines")
    print("- import | Import voiceline")
    print("- back   | Unselect %s"               % teacher_name)
    print("- quit   | Quit")

    print("")

#
# List all the available commands for the room menu
#
def menu_room_command_help_handler(teacher_name, room_name):
    print("\nYou have selected the teacher '%s's room '%s'\n" % (teacher_name, room_name))

    print("- print  | Print %s's %s voicelines"     % (teacher_name, room_name))
    print("- add    | Add %s voiceline"             % room_name)
    print("- del    | Delete %s voiceline"          % room_name)
    print("- edit   | Edit %s voiceline"            % room_name)
    print("- tips   | Print tips for %s voicelines" % room_name)
    print("- import | Import %s voiceline"          % room_name)
    print("- back   | Unselect %s"                  % room_name)
    print("- quit   | Quit")

    print("")

#
# List all the available commands for the teachers menu
#
def menu_line_type_command_help_handler(teacher_name, line_type):
    print("\nYou have selected the teacher '%s's voiceline type '%s'\n" % (teacher_name, line_type))

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

    print("")

#
# List all the available commands for the teachers menu
#
def menu_teachers_command_help_handler():
    print("\nYou have not selected a teacher\n")

    print("- select | Select teacher")
    print("- print  | Print teacher's voicelines")
    print("- add    | Add voiceline")
    print("- del    | Delete voiceline")
    print("- edit   | Edit voiceline")
    print("- tips   | Print tips for voicelines")
    print("- import | Import voiceline")
    print("- quit   | Quit")

    print("")

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

    elif(command == "import"):
        command_import_handler(next_command_strings)

    else:
        print("\nUnknown command '%s'. Type 'help' for help.\n" % command)


# Section 02: Input functions

#
# Input voiceline type
#
def line_type_input(line_types, command_strings):
    if(len(command_strings) >= 1):
        line_type = command_strings[0]

    else:
        print("\nThese are the voiceline types you can select:")

        for line_type in line_types:
            print("- %s" % line_type)

        print("")

        line_type = input("Type: ").strip().lower()

    return line_type

#
# Input room name
#
def room_name_input(room_names, command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

    else:
        print("\nThese are the rooms you can select:")

        for room_name in room_names:
            print("- %s" % room_name)

        print("")

        room_name = input("Room: ").strip().lower()

    return room_name

#
# Input teacher's name
#
def teacher_name_input(teacher_names, command_strings):
    if(len(command_strings) >= 1):
        teacher_name = command_strings[0]

    else:
        print("\nThese are the teachers you can select:")

        for teacher_name in teacher_names:
            print("- %s" % teacher_name)

        print("")

        teacher_name = input("Teacher: ").strip().lower()

    return teacher_name


# Section 03: Command print

#
#
#
def command_print_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("\nNo inputted teacher\n")
        return
        
    if(teacher_name not in teacher_names):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_print_handler(teacher_name, next_command_strings)

#
# Print some or all of the specified teacher's voicelines
#
# Refactor this like tips
#
def teacher_command_print_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    if(len(command_strings) == 0):
        teacher_json_print(teacher_name, teacher_json)

    else:
        line_type = command_strings[0]

        if(line_type not in teacher_json.keys()):
            print("\nUnknown voiceline type: '%s'.\n" % line_type)
            return


        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


        if(line_type == "rooms"):
            print("")
            teacher_rooms_json_print(teacher_json["rooms"], next_command_strings)

        else:
            print("")
            teacher_other_lines_print(line_type, teacher_json[line_type])

#
# Print the teacher's voicelines in the different rooms
#
def teacher_rooms_json_print(rooms_json, command_strings):
    if(len(command_strings) >= 1):
        room_name = command_strings[0]

        if(room_name not in rooms_json.keys()):
            print("\nUnknown room '%s'.\n" % room_name)
            return

        room_lines = rooms_json[room_name]

        print("rooms")

        teacher_room_lines_print(room_name, room_lines)

    else:
        print("rooms")

        for room_name, room_lines in rooms_json.items():
            teacher_room_lines_print(room_name, room_lines)

#
# Print the teacher's voicelines in specified room
#
def teacher_room_lines_print(room_name, room_lines):
    print("* %s" % room_name)
    
    for index, room_line in enumerate(room_lines):
        print("  - %02d: %s" % (index, room_line["text"]))

    print("")

#
# Print the teacher's other voicelines of specified type
#
def teacher_other_lines_print(line_type, other_lines):
    print("%s" % line_type)

    for index, other_line in enumerate(other_lines):
        print("- %02d: %s" % (index, other_line["text"]))

    print("")

#
# Print all of the teacher's voicelines
#
def teacher_json_print(teacher_name, teacher_json):
    print("\nThese are %s's voicelines:\n" % teacher_name)

    for line_type, type_json in teacher_json.items():
        if(line_type == "rooms"):
            teacher_rooms_json_print(type_json, [])

        else:
            teacher_other_lines_print(line_type, type_json)


# Section 04: Command tips

#
# Print out tips for voicelines
#
def command_tips_handler(command_strings):
    line_types = generic_line_types_get()

    if(len(command_strings) == 0):
        print("\nSpecify voiceline type for tips\n")
        return

    else:
        line_type = command_strings[0]

        if(line_type not in line_types):
            print("\nUnknown voiceline type: '%s'.\n" % line_type)
            return


        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


        if(line_type == "rooms"):
            rooms_tips_print(next_command_strings)

        else:
            generic_lines = generic_lines_get(line_type)

            if(not generic_lines):
                print("\nThere are no voiceline tips for %s" % line_type)
                return

            print("\nThese are some voiceline tips for %s:" % line_type)

            for index, line in enumerate(generic_lines):
                print("- %02d: %s" % (index, line))

            print("")

#
# Print tips for all rooms
#
def rooms_tips_print(command_strings):
    room_names = generic_room_names_get()

    if(len(command_strings) >= 1):
        room_name = command_strings[0]

        if(room_name not in room_names):
            print("\nUnknown room '%s'.\n" % room_name)
            return

        generic_lines = generic_lines_get("rooms", room_name)
    
        if(not generic_lines):
            print("\nThere are no voiceline tips for room %s" % room_name)
            return

        print("\nThese are some voiceline tips for room %s:" % room_name)

        for index, line in enumerate(generic_lines):
            print("- %02d: %s" % (index, line))

        print("")

    else:
        print("\nThese are some voiceline tips for rooms:\n")

        for room_name in room_names:
            generic_lines = generic_lines_get("rooms", room_name)
        
            print("* %s" % room_name)

            for index, line in enumerate(generic_lines):
                print("  - %02d: %s" % (index, line))

            print("")


# Section 05: Command import

#
#
#
def command_import_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("\nNo inputted teacher\n")
        return


    if(teacher_name not in teacher_names):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_import_handler(teacher_name, next_command_strings)

#
# Import a voiceline for a specified teacher
#
def teacher_command_import_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return

    line_types = generic_line_types_get()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("\nNo inputted voiceline type\n")
        return

    if(line_type not in line_types):
        print("\nUnknown voiceline type: '%s'.\n" % line_type)
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
# Import a generic voiceline to teacher's specified room
#
# Maybe: Remove generic_lines as argument and create local variable instead
#
def teacher_room_lines_line_import(room_name, room_lines, generic_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        print("\nSelect a generic %s voiceline to import:" % room_name)

        for index, line in enumerate(generic_lines):
            print("- %02d: %s" % (index, line))

        print("")

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("\nNo inputted index\n")
            return room_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("\nIndex must be an integer\n")
        return room_lines


    if(line_index < 0 or line_index >= len(generic_lines)):
        print("\nIndex out of range\n")
        return room_lines


    line_text = generic_lines[line_index]

    room_lines.append({"text": line_text})

    print("\nImported voiceline: '%s'\n" % line_text)


    return room_lines

#
# Import a generic voiceline to one of teacher's rooms
#
def teacher_rooms_json_line_import(rooms_json, command_strings):
    room_names = generic_room_names_get()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("\nNo inputted room\n")
        return rooms_json

    if(room_name not in room_names):
        print("\nUnknown room: '%s'.\n" % room_name)
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
        print("\nSelect a generic %s voiceline to import:" % line_type)

        for index, line in enumerate(generic_lines):
            print("- %02d: %s" % (index, line))

        print("")
        
        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("\nNo inputted index\n")
            return other_lines

    
    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("\nIndex must be an integer\n")
        return other_lines


    if(line_index < 0 or line_index >= len(generic_lines)):
        print("\nIndex out of range\n")
        return other_lines


    line_text = generic_lines[line_index]

    other_lines.append({"text": line_text})

    print("\nImported voiceline: '%s'\n" % line_text)


    return other_lines


# Section 06: Command add

#
#
#
def command_add_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("\nNo inputted teacher\n")
        return

        
    if(teacher_name not in teacher_names):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_add_handler(teacher_name, next_command_strings)

#
# Add a voiceline for a specified teacher
#
def teacher_command_add_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    line_types = generic_line_types_get()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("\nNo inputted voiceline type\n")
        return

    if(line_type not in line_types):
        print("\nUnknown voiceline type: '%s'.\n" % line_type)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_add(teacher_name, teacher_json["rooms"], next_command_strings)

    else:
        other_lines = teacher_json.get(line_type, [])

        teacher_json[line_type] = teacher_other_lines_line_add(teacher_name, line_type, other_lines)


    teacher_json_save(teacher_name, teacher_json)

#
# Add a voiceline to one of teacher's rooms
#
def teacher_rooms_json_line_add(teacher_name, rooms_json, command_strings):
    room_names = generic_room_names_get()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("\nNo inputted room\n")
        return rooms_json

    if(room_name not in room_names):
        print("\nUnknown room: '%s'.\n" % room_name)
        return rooms_json


    room_lines = rooms_json.get(room_name, [])

    print("")

    if(room_lines):
        print("These are %s's existing %s voicelines:" % (teacher_name, room_name))

        for index, room_line in enumerate(room_lines):
            print("- %02d: %s" % (index, room_line["text"]))

    else:
        print("%s have no existing %s voicelines" % (teacher_name, room_name))

    print("")

    line_text = input("Text: ").strip()

    if(not line_text):
        print("\nNo inputted text\n")
        return rooms_json


    room_lines = rooms_json.get(room_name, [])

    room_lines.append({"text": line_text})

    rooms_json[room_name] = room_lines

    print("\nAdded voiceline: '%s'\n" % line_text)

    return rooms_json

#
# Add another voiceline to teacher
#
def teacher_other_lines_line_add(teacher_name, line_type, other_lines):
    print("")

    if(other_lines):
        print("These are %s's existing %s voicelines:" % (teacher_name, line_type))

        for index, other_line in enumerate(other_lines):
            print("- %02d: %s" % (index, other_line["text"]))

    else:
        print("%s have no existing %s voicelines" % (teacher_name, line_type))

    print("")

    line_text = input("Text: ").strip()

    if(not line_text):
        print("\nNo inputted text\n")
        return other_lines


    other_lines.append({"text": line_text})

    print("\nAdded voiceline: '%s'\n" % line_text)

    return other_lines


# Section 07: Command edit

#
#
#
def command_edit_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("\nNo inputted teacher\n")
        return
        

    if(teacher_name not in teacher_names):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_edit_handler(teacher_name, next_command_strings)

#
# Edit one of the teacher's voicelines
#
def teacher_command_edit_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    line_types = teacher_json.keys()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("\nNo inputted voiceline type\n")
        return

    if(line_type not in line_types):
        print("\nUnknown voiceline type: '%s'.\n" % line_type)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_edit(teacher_name, teacher_json["rooms"], next_command_strings)

    else:
        teacher_json[line_type] = teacher_other_lines_line_edit(teacher_name, line_type, teacher_json[line_type], next_command_strings)


    teacher_json_save(teacher_name, teacher_json)

#
# Edit one of the teacher's voicelines in a room
#
# Rename this function
#
def teacher_rooms_json_line_edit(teacher_name, rooms_json, command_strings):
    room_names = rooms_json.keys()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("\nNo inputted room\n")
        return rooms_json

    if(room_name not in room_names):
        print("\nUnknown room: '%s'.\n" % room_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    rooms_json[room_name] = teacher_room_lines_line_edit(teacher_name, room_name, rooms_json[room_name], next_command_strings)

    return rooms_json

#
# Edit one of the teacher's voicelines in a specified room
#
# Rename this function
#
def teacher_room_lines_line_edit(teacher_name, room_name, room_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        print("\nSelect one of %s's %s voicelines to edit:" % (teacher_name, room_name))

        for index, room_line in enumerate(room_lines):
            print("- %02d: %s" % (index, room_line["text"]))

        print("")

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("\nNo inputted index\n")
            return room_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("\nIndex must be an integer\n")
        return room_lines


    if(line_index < 0 or line_index >= len(room_lines)):
        print("\nIndex out of range\n")
        return room_lines


    line_text = input("Text: ").strip()

    if(not line_text):
        print("\nNo inputted text\n")
        return room_lines


    room_lines[line_index]["text"] = line_text

    print("\nEdited voiceline: '%s'\n" % line_text)

    return room_lines

#
# Edit one of the teacher's other voicelines of a specified type
#
# Rename this function
#
def teacher_other_lines_line_edit(teacher_name, line_type, other_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        print("\nSelect one of %s's %s voicelines to edit:" % (teacher_name, line_type))

        for index, other_line in enumerate(other_lines):
            print("- %02d: %s" % (index, other_line["text"]))

        print("")

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("\nNo inputted index\n")
            return other_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("\nIndex must be an integer\n")
        return other_lines


    if(line_index < 0 or line_index >= len(other_lines)):
        print("\nIndex out of range\n")
        return other_lines


    line_text = input("Text: ").strip()

    if(not line_text):
        print("\nNo inputted text\n")
        return other_lines


    other_lines[line_index]["text"] = line_text

    print("\nEdited voiceline: '%s'\n" % line_text)

    return other_lines


# Section 08: Command delete

#
#
#
def command_del_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("\nNo inputted teacher\n")
        return

        
    if(teacher_name not in teacher_names):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_del_handler(teacher_name, next_command_strings)

#
# Delete one of the teacher's voicelines
#
def teacher_command_del_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    if(not teacher_json):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    line_types = teacher_json.keys()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("\nNo inputted voiceline type\n")
        return

    if(line_type not in line_types):
        print("\nUnknown voiceline type: '%s'.\n" % line_type)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(line_type == "rooms"):
        teacher_json["rooms"] = teacher_rooms_json_line_del(teacher_name, teacher_json["rooms"], next_command_strings)

    else:
        teacher_json[line_type] = teacher_other_lines_line_del(teacher_name, line_type, teacher_json[line_type], next_command_strings)


    teacher_json_save(teacher_name, teacher_json)

#
# Delete one of the teacher's voicelines in a room
#
# Rename this function
#
def teacher_rooms_json_line_del(teacher_name, rooms_json, command_strings):
    room_names = rooms_json.keys()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("\nNo inputted room\n")
        return rooms_json

    if(room_name not in room_names):
        print("\nUnknown room: '%s'.\n" % room_name)
        return rooms_json


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    rooms_json[room_name] = teacher_room_lines_line_del(teacher_name, room_name, rooms_json[room_name], next_command_strings)

    return rooms_json

#
# Delete one of the teacher's voicelines in a specified room
#
# Rename this function
#
def teacher_room_lines_line_del(room_name, room_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        print("\nSelect one of %s's %s voicelines to delete:" % (teacher_name, room_name))

        for index, room_line in enumerate(room_lines):
            print("- %02d: %s" % (index, room_line["text"]))

        print("")

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("\nNo inputted index\n")
            return room_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("\nIndex must be an integer\n")
        return room_lines


    if(line_index < 0 or line_index >= len(room_lines)):
        print("\nIndex out of range\n")
        return room_lines


    line_text = room_lines[line_index]["text"]

    del room_lines[line_index]

    print("\nDeleted voiceline: '%s'\n" % line_text)

    return room_lines

#
# Delete one of the teacher's other voicelines of a specified type
#
# Rename this function
#
def teacher_other_lines_line_del(teacher_name, line_type, other_lines, command_strings):
    if(len(command_strings) >= 1):
        line_index_string = command_strings[0]

    else:
        print("\nSelect one of %s's %s voicelines to delete:" % (teacher_name, line_type))

        for index, other_line in enumerate(other_lines):
            print("- %02d: %s" % (index, other_line["text"]))

        print("")

        line_index_string = input("Index: ").strip().lower()

        if(not line_index_string):
            print("\nNo inputted index\n")
            return other_lines


    try:
        line_index = int(line_index_string)

    except Exception as exception:
        print("\nIndex must be an integer\n")
        return other_lines

    
    if(line_index < 0 or line_index >= len(other_lines)):
        print("\nIndex out of range\n")
        return other_lines


    line_text = other_lines[line_index]["text"]

    del other_lines[line_index]

    print("\nDeleted voiceline: '%s'\n" % line_text)

    return other_lines


# Section 09: Command select

#
# Select a teacher and go to the teacher menu
#
def command_select_handler(command_strings):
    teacher_names = teacher_names_get()

    teacher_name = teacher_name_input(teacher_names, command_strings)

    if(not teacher_name):
        print("\nNo inputted teacher\n")
        return

    if(teacher_name not in teacher_names):
        print("\nTeacher '%s' doesn't exist\n" % teacher_name)
        return


    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

    teacher_command_select_handler(teacher_name, next_command_strings)


    print("\nSelected teacher: '%s'\n" % teacher_name)

    menu_teacher_routine(teacher_name)

#
#
#
def teacher_command_select_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    line_types = teacher_json.keys()

    line_type = line_type_input(line_types, command_strings)

    if(not line_type):
        print("\nNo inputted voiceline type")
        return

    if(line_type not in line_types):
        print("\nUnknown voiceline type: '%s'." % line_type)
        return


    if(line_type == "rooms"):
        next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []

        teacher_rooms_command_select_handler(teacher_name, next_command_strings)


    print("\nSelected teacher '%s's '%s' voicelines\n" % (teacher_name, line_type))

    menu_line_type_routine(teacher_name, line_type)

#
#
#
def teacher_rooms_command_select_handler(teacher_name, command_strings):
    teacher_json = teacher_json_load(teacher_name)

    room_names = teacher_json["rooms"].keys()

    room_name = room_name_input(room_names, command_strings)

    if(not room_name):
        print("\nNo inputted room")
        return

    if(room_name not in room_names):
        print("\nUnknown room: '%s'." % room_name)
        return

    print("\nSelected teacher '%s's '%s' voicelines\n" % (teacher_name, room_name))

    menu_room_routine(teacher_name, room_name)


# Section 10: File and format functions

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
    for index, other_line in enumerate(other_lines):
        line_voice = "%s-%s-line-%d.mov" % (teacher_name, line_type, index)

        other_lines[index]["voice"] = line_voice
        other_lines[index]["text"]  = other_line["text"].strip()

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


# Section 11: Menu routines

#
# This is the routine for the teachers menu
#
def menu_teachers_routine():
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_teachers_command_parse(command_string)

#
# Parse the inputted command in the teachers menu
#
def menu_teachers_command_parse(command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(command == "quit"):
        exit()

    elif(command == "back"):
        print("\nYou can't go any further back\n");

    elif(command == "help"):
        menu_teachers_command_help_handler()

    elif(command == "tips"):
        command_tips_handler(next_command_strings)

    else:
        command_handler(command, next_command_strings)

#
# This is the routine for the teacher menu
#
def menu_teacher_routine(teacher_name):
    while True:
        command_string = input("$ ").strip().lower()

        if(command_string != ""):
            menu_teacher_command_parse(teacher_name, command_string)

#
# Parse the inputted command in the teacher menu
#
def menu_teacher_command_parse(teacher_name, command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(command == "quit"):
        exit()

    elif(command == "back"):
        print("\nUnselected %s\n" % teacher_name)

        menu_teachers_routine()

    elif(command == "help"):
        menu_teacher_command_help_handler(teacher_name)

    elif(command == "tips"):
        command_tips_handler(next_command_strings)

    else:
        full_command_strings = [teacher_name] + next_command_strings

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
def menu_line_type_command_parse(teacher_name, line_type, command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(command == "quit"):
        exit()

    elif(command == "back"):
        print("\nUnselected %s voicelines\n" % line_type)

        menu_teacher_routine(teacher_name)

    elif(command == "help"):
        menu_line_type_command_help_handler(teacher_name, line_type)

    elif(command == "tips"):
        tips_command_strings = [line_type] + next_command_strings

        command_tips_handler(tips_command_strings)

    else:
        full_command_strings = [teacher_name, line_type] + next_command_strings

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
# Parse the inputted command in the room menu
#
def menu_room_command_parse(teacher_name, room_name, command_string):
    command_strings = command_string.split(" ")

    command = command_strings[0]

    next_command_strings = command_strings[1:] if len(command_strings) >= 2 else []


    if(command == "quit"):
        exit()

    if(command == "back"):
        print("\nUnselected %s\n" % room_name)

        menu_line_type_routine(teacher_name, "rooms")

    elif(command == "help"):
        menu_room_command_help_handler(teacher_name, room_name)

    elif(command == "tips"):
        tips_command_strings = ["rooms", room_name] + next_command_strings

        command_tips_handler(tips_command_strings)

    else:
        full_command_strings = [teacher_name, "rooms", room_name] + next_command_strings

        command_handler(command, full_command_strings)

#
# This is like the main function
#
if(__name__ == "__main__"):
    next_command_strings = argv[1:] if len(argv) >= 2 else []

    if(len(argv) > 1):
        command_select_handler(argv[1:])

    else:
        menu_teachers_routine()

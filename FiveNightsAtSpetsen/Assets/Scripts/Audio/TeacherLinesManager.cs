/*
 * Manage teacher's voicelines, by
 * - providing voicelines for different events
 * - pick new voiceliens as often as possible
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-18
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherLinesManager
{
  private TeacherLinesObject teacherLines;

  private TeacherLinesObject workingLines;

  /*
   *
   */
  public TeacherLinesManager(string teacherName)
  {
    teacherLines.Load(teacherName)

    if(!teacherLines)
    {
      Debug.Log("Failed to load teacher's voicelines");

      this.enabled = false;
    }

    // Initializing all working lines with teacher's lines
    workingLines = teacherLines;
  }

  /*
   * When the teacher enters a room, he should 
   * 60% of the time say something about the room, and
   * 40% of the rime say something general
   */
  public Voiceline GetRoomEnteredVoiceline(string roomName)
  {
    int randomProcent = random.Next(100);

    if(randomProcent < 60)
    {
      voiceline = GetRoomVoiceline(roomName);
    }
    else
    {
      voiceline = GetGeneralVoiceline();
    }
  }

  /*
   *
   */
  public Voiceline GetRoomVoiceline(string roomName)
  {
    List<Voiceline> teacherRoomLines;

    // If the teacher doesn't have voicelines for room
    if(!teacherLines.rooms.TryGetValue(roomName, out teacherRoomLines) ||
        teacherRoomLines.Count == 0 ||
       !workingLines.rooms.ContainsKey(roomName))
    {
      Debug.LogWarning($"Teacher doesn't have voicelines for room '{roomName}'");
      
      return null;
    }
      
    // Refill the working voicelines if it is empty
    if(workingLines.rooms[roomName].Count == 0)
    {
      workingLines.rooms[roomName] = teacherLines.rooms[roomName];
    }

    // Pop a random working voiceline
    int index = random.Next(workingLines.rooms[roomName].Count);

    Voiceline voiceline = workingLines.rooms[roomName][index];

    workingLines.rooms[roomName].removeAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetGeneralVoiceline(void)
  {
    // If the teacher doesn't have general voicelines
    if(teacherLines.general.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have general voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.general.Count == 0)
    {
      workingLines.general = teacherLines.general;
    }

    // Pop a random working voiceline
    int index = random.Next(workingLines.general.Count);

    Voiceline voiceline = workingLines.general[index];

    workingLines.general.removeAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetSpottingVoiceline(void)
  {
    // If the teacher doesn't have spotting voicelines
    if(teacherLines.spotting.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have spotting voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.spotting.Count == 0)
    {
      workingLines.spotting = teacherLines.spotting;
    }

    // Pop a random working voiceline
    int index = random.Next(workingLines.spotting.Count);

    Voiceline voiceline = workingLines.spotting[index];

    workingLines.spotting.removeAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetCapturingVoiceline(void)
  {
    // If the teacher doesn't have capturing voicelines
    if(teacherLines.capturing.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have capturing voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.capturing.Count == 0)
    {
      workingLines.capturing = teacherLines.capturing;
    }

    // Pop a random working voiceline
    int index = random.Next(workingLines.capturing.Count);

    Voiceline voiceline = workingLines.capturing[index];

    workingLines.capturing.removeAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetHearingVoiceline(void)
  {
    // If the teacher doesn't have hearing voicelines
    if(teacherLines.hearing.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have hearing voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.hearing.Count == 0)
    {
      workingLines.hearing = teacherLines.hearing;
    }

    // Pop a random working voiceline
    int index = random.Next(workingLines.hearing.Count);

    Voiceline voiceline = workingLines.hearing[index];

    workingLines.hearing.removeAt(index);

    return voiceline;
  }
}

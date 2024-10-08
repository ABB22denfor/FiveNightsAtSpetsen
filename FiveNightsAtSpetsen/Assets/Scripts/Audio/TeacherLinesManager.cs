/*
 * Manage teacher's voicelines, by
 * - providing voicelines for different events
 * - pick new voiceliens as often as possible
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-24
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
   * Initialize the voicelines manager
   * - load teacher's voicelines
   * - initialize working voicelines variable
   */
  public TeacherLinesManager(string teacherName)
  {
    if(teacherName == "")
    {
      Debug.LogWarning("No teacher name was supplied");

      return;
    }

    teacherLines = TeacherLinesObject.Load(teacherName);

    if(teacherLines == null)
    {
      Debug.LogWarning("Failed to load teacher's voicelines");

      return;
    }

    workingLines = TeacherLinesObject.Load(teacherName);
  }

  /*
   * When the teacher enters a room, he should 
   * 60% of the time say something about the room, and
   * 40% of the rime say something general
   */
  public Voiceline GetRoomEnteredVoiceline(string roomName)
  {
    int randomProcent = Random.Range(0, 100);

    Voiceline voiceline = null;

    if(randomProcent < 60)
    {
      voiceline = GetRoomVoiceline(roomName);
    }

    if(voiceline == null)
    {
      voiceline = GetGeneralVoiceline();
    }

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetRoomVoiceline(string roomName)
  {
    Debug.Log("Getting room voiceline");

    if(teacherLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    if(teacherLines.rooms == null)
    {
      Debug.LogWarning("Teacher doesn't have rooms voicelines");

      return null;
    }

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
      workingLines.rooms[roomName].AddRange(teacherLines.rooms[roomName]);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.rooms[roomName].Count);

    Voiceline voiceline = workingLines.rooms[roomName][index];

    workingLines.rooms[roomName].RemoveAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetGeneralVoiceline()
  {
    Debug.Log("Getting general voiceline");

    if(teacherLines == null || workingLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    // If the teacher doesn't have general voicelines
    if(teacherLines.general == null ||
       teacherLines.general.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have general voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.general.Count == 0)
    {
      workingLines.general.AddRange(teacherLines.general);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.general.Count);

    Voiceline voiceline = workingLines.general[index];

    workingLines.general.RemoveAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetSpottingVoiceline()
  {
    Debug.Log("Getting spotting voiceline");

    if(teacherLines == null || workingLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    // If the teacher doesn't have spotting voicelines
    if(teacherLines.spotting == null ||
       teacherLines.spotting.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have spotting voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.spotting.Count == 0)
    {
      workingLines.spotting.AddRange(teacherLines.spotting);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.spotting.Count);

    Voiceline voiceline = workingLines.spotting[index];

    workingLines.spotting.RemoveAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetChasingVoiceline()
  {
    Debug.Log("Getting chasing voiceline");

    if(teacherLines == null || workingLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    // If the teacher doesn't have chasing voicelines
    if(teacherLines.chasing == null ||
       teacherLines.chasing.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have chasing voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.chasing.Count == 0)
    {
      workingLines.chasing.AddRange(teacherLines.chasing);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.chasing.Count);

    Voiceline voiceline = workingLines.chasing[index];

    workingLines.chasing.RemoveAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetCapturingVoiceline()
  {
    Debug.Log("Getting capturing voiceline");

    if(teacherLines == null || workingLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    // If the teacher doesn't have capturing voicelines
    if(teacherLines.capturing == null ||
       teacherLines.capturing.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have capturing voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.capturing.Count == 0)
    {
      workingLines.capturing.AddRange(teacherLines.capturing);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.capturing.Count);

    Voiceline voiceline = workingLines.capturing[index];

    workingLines.capturing.RemoveAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetHearingVoiceline()
  {
    Debug.Log("Getting hearing voiceline");

    if(teacherLines == null || workingLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    // If the teacher doesn't have hearing voicelines
    if(teacherLines.hearing.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have hearing voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.hearing.Count == 0)
    {
      workingLines.hearing.AddRange(teacherLines.hearing);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.hearing.Count);

    Voiceline voiceline = workingLines.hearing[index];

    workingLines.hearing.RemoveAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetAlertVoiceline()
  {
    Debug.Log("Getting alert voiceline");

    if(teacherLines == null || workingLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    // If the teacher doesn't have hearing voicelines
    if(teacherLines.alert.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have alert voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.alert.Count == 0)
    {
      workingLines.alert.AddRange(teacherLines.alert);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.alert.Count);

    Voiceline voiceline = workingLines.alert[index];

    workingLines.alert.RemoveAt(index);

    return voiceline;
  }

  /*
   *
   */
  public Voiceline GetMovingVoiceline()
  {
    Debug.Log("Getting moving voiceline");

    if(teacherLines == null || workingLines == null)
    {
      Debug.LogWarning("Teacher's voicelines are not loaded");

      return null;
    }

    // If the teacher doesn't have hearing voicelines
    if(teacherLines.moving.Count == 0)
    {
      Debug.LogWarning("Teacher doesn't have moving voicelines");
      
      return null;
    }

    // Refill the working voicelines if it is empty
    if(workingLines.moving.Count == 0)
    {
      workingLines.moving.AddRange(teacherLines.moving);
    }

    // Pop a random working voiceline
    int index = Random.Range(0, workingLines.moving.Count);

    Voiceline voiceline = workingLines.moving[index];

    workingLines.moving.RemoveAt(index);

    return voiceline;
  }
}

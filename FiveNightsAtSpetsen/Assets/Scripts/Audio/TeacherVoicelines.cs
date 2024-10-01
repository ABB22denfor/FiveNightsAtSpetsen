/*
 * Make teacher say different voicelines depending on
 * - what the teacher is doing
 * - which room the teacher is located in
 *
 * This script expects the following events to be triggered:
 * - OnTeacherEnteredRoom
 * - OnPlayerSpotted
 * - OnPlayerCaptured
 * - OnPlayerMadeSound
 *
 * Created by Oliver Sarebro
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-10-01
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TeacherVoiceline))]
[RequireComponent(typeof(TeacherPlayerMemory))]
public class TeacherVoicelines : MonoBehaviour
{
  private Teacher teacher;

  private TeacherLinesManager linesManager;
  private TeacherVoiceline    teacherVoiceline;

  private TeacherPlayerMemory teacherMemory;

  /*
   * When the script is enabled,
   * add event handlers and initialize voiceliens
   */
  void OnEnable()
  {
    Debug.Log("TeacherVoicelines.cs enabled");

    teacher          = gameObject.GetComponent<Teacher>();
    teacherVoiceline = gameObject.GetComponent<TeacherVoiceline>();
    teacherMemory    = gameObject.GetComponent<TeacherPlayerMemory>();

    // Initializing teacher's voiceline manager
    linesManager = new TeacherLinesManager(teacher.teacherName);

    // Adding handlers for teacher's events
    EventsManager.Instance.teacherEvents.OnTeacherEnteredRoom += OnTeacherEnteredRoom;
    EventsManager.Instance.teacherEvents.OnPlayerSpotted      += OnPlayerSpotted;
    EventsManager.Instance.teacherEvents.OnPlayerCaptured     += OnPlayerCaptured;
    EventsManager.Instance.teacherEvents.OnPlayerMadeSound    += OnPlayerMadeSound;
  }

  /*
   * When the script is disabled, remove event handlers
   */
  void OnDisable()
  {
    Debug.Log("TeacherVoicelines.cs disabled");

    // Removing handlers for teacher's events
    EventsManager.Instance.teacherEvents.OnTeacherEnteredRoom -= OnTeacherEnteredRoom;
    EventsManager.Instance.teacherEvents.OnPlayerSpotted      -= OnPlayerSpotted;
    EventsManager.Instance.teacherEvents.OnPlayerCaptured     -= OnPlayerCaptured;
    EventsManager.Instance.teacherEvents.OnPlayerMadeSound    -= OnPlayerMadeSound;
  }

  /*
   * When the teacher enters a room
   *
   * The teacher shouldn't say a room voiceline,
   * if it is alert, or has seen the player once
   *
   * Note: Ask Oliver to create this event
   */
  private void OnTeacherEnteredRoom(TeacherRoomPath room, bool isTarget)
  {
    string roomName = room.id;

    Debug.Log("Teacher entered room: " + roomName);

    if(teacher.isAlert || teacher.hasSeenPlayer)
    {
      Debug.Log("Teacher should not say a room voiceline");
      return;
    }

    Voiceline voiceline = linesManager.GetRoomEnteredVoiceline(roomName);

    if(voiceline != null)
    {
      teacherVoiceline.StopVoiceline();

      teacherVoiceline.StartVoiceline(voiceline);
    }
  }

  /*
   * When the teacher spotts the player
   */
  private void OnPlayerSpotted()
  {
    // If the teacher knows where the player are,
    // and now spotts the player again,
    // he should not say anything extra
    if(teacherMemory.knowsWherePlayerIs) return;

    teacherMemory.SpottPlayer();

    Debug.Log("Teacher spotted player");

    // Say the spotting voiceline
    Voiceline voiceline = linesManager.GetSpottingVoiceline();

    if(voiceline != null)
    {
      teacherVoiceline.StopVoiceline();

      teacherVoiceline.StartVoiceline(voiceline);
    }
  }

  /*
   * When the teacher captures the player
   *
   * If teacher already has captured the player,
   * no more voiceline should be said
   */
  private void OnPlayerCaptured()
  {
    if(teacherMemory.hasCapturedPlayer) return;

    teacherMemory.CapturePlayer();

    Debug.Log("Teacher captured player");

    Voiceline voiceline = linesManager.GetCapturingVoiceline();

    if(voiceline != null)
    {
      teacherVoiceline.StopVoiceline();

      teacherVoiceline.StartVoiceline(voiceline);
    }
  }

  /*
   * When the teacher hears a sound
   *
   * If the teacher is aware of the player, he should say a hearing-voiceline
   * Else, he should say an alert-voiceline
   *
   * If the teacher knows where the player are,
   * and now spotts the player again,
   * he should not say anything extra
   */
  private void OnPlayerMadeSound(TeacherRoomPath room)
  {
    Debug.Log("Teacher heard a sound");

    Voiceline voiceline = null;

    if(teacherMemory.knowsAboutPlayer)
    {
      voiceline = linesManager.GetHearingVoiceline();
    }
    else
    {
      voiceline = linesManager.GetAlertVoiceline();
    }

    teacherMemory.HearSound();

    if(voiceline != null)
    {
      teacherVoiceline.StopVoiceline();

      teacherVoiceline.StartVoiceline(voiceline);
    }
  }
}

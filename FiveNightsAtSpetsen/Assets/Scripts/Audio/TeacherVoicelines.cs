/*
 * Make teacher say different voicelines depending on
 * - what the teacher is doing
 * - which room the teacher is located in
 *
 * This script expects the following events to be triggered:
 * - OnRoomEntered
 * - OnPlayerSpotted
 * - OnPlayerCaptured
 * - OnPlayerHeard
 *
 * Created by Oliver Sarebro
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-18
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherVoicelines : MonoBehaviour
{
  private Teacher teacher;

  private TeacherLinesManager linesManager;
  private TeacherAudioManager audioManager;

  /*
   * When the script is enabled,
   * add event handlers and initialize voiceliens
   */
  void OnEnable()
  {
    teacher      = gameObject.GetComponent<Teacher>();
    audioManager = gameObject.GetComponent<TeacherAudioManager>();

    // Adding handlers for teacher's events
    EventsManager.Instance.teacherEvents.OnRoomEntered    += OnRoomEntered;
    EventsManager.Instance.teacherEvents.OnPlayerSpotted  += OnPlayerSpotted;
    EventsManager.Instance.teacherEvents.OnPlayerCaptured += OnPlayerCaptured;
    EventsManager.Instance.teacherEvents.OnPlayerHeard    += OnPlayerHeard;

    // Initializing teacher's voiceline manager
    linesManager = new TeacherLinesManager(teacher.teacherName);
  }

  /*
   * When the script is disabled, remove event handlers
   */
  void OnDisable()
  {
    // Removing handlers for teacher's events
    EventsManager.Instance.teacherEvents.OnRoomEntered    -= OnRoomEntered;
    EventsManager.Instance.teacherEvents.OnPlayerSpotted  -= OnPlayerSpotted;
    EventsManager.Instance.teacherEvents.OnPlayerCaptured -= OnPlayerCaptured;
    EventsManager.Instance.teacherEvents.OnPlayerHeard    -= OnPlayerHeard;
  }

  /*
   * Say a voiceline, by both playing up the audio and writing the text
   */
  private void SayVoiceline(Voiceline voiceline)
  {
    audioManager.Play(voiceline.audio);

    Debug.Log("Voiceline: " + voiceline.text);
  }

  /*
   * When the teacher enters a room
   *
   * The teacher shouldn't say a room voiceline,
   * if it is alert, or has seen the player once
   *
   * Note: Ask Oliver to create this event
   */
  private void OnRoomEntered(string roomName)
  {
    Debug.Log("Teacher entered room: " + roomName);

    if(teacher.isAlert || teacher.hasSeenPlayer)
    {
      Debug.Log("Teacher should not say a room voiceline");
      return;
    }

    Voiceline voiceline = linesManager.GetRoomEnteredVoiceline(roomName);

    if(voiceline != null)
    {
      SayVoiceline(voiceline);
    }
  }

  /*
   * When the teacher spotts the player
   */
  private void OnPlayerSpotted()
  {
    Debug.Log("Teacher spotted player");

    Voiceline voiceline = linesManager.GetSpottingVoiceline();

    if(voiceline != null)
    {
      SayVoiceline(voiceline);
    }
  }

  /*
   * When the teacher captures the player
   */
  private void OnPlayerCaptured()
  {
    Debug.Log("Teacher captured player");

    Voiceline voiceline = linesManager.GetCapturingVoiceline();

    if(voiceline != null)
    {
      SayVoiceline(voiceline);
    }
  }

  /*
   * When the teacher hears a sound
   */
  private void OnPlayerHeard()
  {
    Debug.Log("Teacher heard a sound");

    Voiceline voiceline = linesManager.GetHearingVoiceline();

    if(voiceline != null)
    {
      SayVoiceline(voiceline);
    }
  }
}

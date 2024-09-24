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
    EventsManager.Instance.teacherEvents.OnTeacherEnteredRoom += OnTeacherEnteredRoom;
    EventsManager.Instance.teacherEvents.OnPlayerSpotted      += OnPlayerSpotted;
    EventsManager.Instance.teacherEvents.OnPlayerCaptured     += OnPlayerCaptured;
    EventsManager.Instance.teacherEvents.OnPlayerMadeSound    += OnPlayerMadeSound;

    // Initializing teacher's voiceline manager
    linesManager = new TeacherLinesManager(teacher.teacherName);
  }

  /*
   * When the script is disabled, remove event handlers
   */
  void OnDisable()
  {
    // Removing handlers for teacher's events
    EventsManager.Instance.teacherEvents.OnTeacherEnteredRoom -= OnTeacherEnteredRoom;
    EventsManager.Instance.teacherEvents.OnPlayerSpotted      -= OnPlayerSpotted;
    EventsManager.Instance.teacherEvents.OnPlayerCaptured     -= OnPlayerCaptured;
    EventsManager.Instance.teacherEvents.OnPlayerMadeSound    -= OnPlayerMadeSound;
  }

  /*
   * Say a voiceline, by both playing up the audio and writing the text
   *
   * Start the talking animation, when the voiceline is being said
   *
   * Stop the talking animation, after the voiceline is said
   */
  private void SayVoiceline(Voiceline voiceline)
  {
    audioManager.Play(voiceline.audio);

    // animator.isTalking = true;

    Debug.Log("Voiceline: " + voiceline.text);

    // animator.isTalking = false;
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
   *
   * If teacher already has captured the player,
   * no more voiceline should be said
   */
  private void OnPlayerCaptured()
  {
    // If teacher already has captured the player
    // if()

    Debug.Log("Teacher captured player");

    Voiceline voiceline = linesManager.GetCapturingVoiceline();

    if(voiceline != null)
    {
      SayVoiceline(voiceline);
    }
  }

  /*
   * When the teacher hears a sound
   *
   * If the teacher is aware of the player, he should say a hearing-voiceline
   * Else, he should say an alert-voiceline
   */
  private void OnPlayerMadeSound(TeacherRoomPath room)
  {
    string roomName = room.id;

    Debug.Log("Teacher heard a sound");

    // Voiceline voiceline = linesManager.GetHearingVoiceline();
    Voiceline voiceline = linesManager.GetAlertVoiceline();

    if(voiceline != null)
    {
      SayVoiceline(voiceline);
    }
  }
}

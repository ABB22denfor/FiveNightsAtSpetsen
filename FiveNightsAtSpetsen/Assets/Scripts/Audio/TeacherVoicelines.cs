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
using TMPro;
using UnityEngine;

public class TeacherVoicelines : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI subtitleText = default;

  private Teacher teacher;

  private TeacherLinesManager linesManager;
  private TeacherVoiceline    teacherVoiceline;

  /*
   * When the script is enabled,
   * add event handlers and initialize voiceliens
   */
  void OnEnable()
  {
    Debug.Log("TeacherVoicelines.cs enabled");

    teacher = gameObject.GetComponent<Teacher>();

    teacherVoiceline = gameObject.AddComponent<TeacherVoiceline>();

    if(subtitleText)
    {
      subtitleText.text = "";

      teacherVoiceline.subtitleText = subtitleText;
    }

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
    Debug.Log("TeacherVoicelines.cs disabled");

    Destroy(gameObject.GetComponent<TeacherVoiceline>());

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
    Debug.Log("Teacher spotted player");

    Voiceline voiceline = linesManager.GetSpottingVoiceline();

    if(voiceline != null)
    {
      teacherVoiceline.StopVoiceline();

      teacherVoiceline.StartVoiceline(voiceline);
    }
  }

  private bool playerHasBeenCaptured = false;

  /*
   * When the teacher captures the player
   *
   * If teacher already has captured the player,
   * no more voiceline should be said
   */
  private void OnPlayerCaptured()
  {
    // If teacher already has captured the player
    if(playerHasBeenCaptured) return;

    playerHasBeenCaptured = true;


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
   */
  private void OnPlayerMadeSound(TeacherRoomPath room)
  {
    string roomName = room.id;

    Debug.Log("Teacher heard a sound");

    // Voiceline voiceline = linesManager.GetHearingVoiceline();
    Voiceline voiceline = linesManager.GetAlertVoiceline();

    if(voiceline != null)
    {
      teacherVoiceline.StopVoiceline();

      teacherVoiceline.StartVoiceline(voiceline);
    }
  }
}

/*
 * TeacherVoiceline.cs - Start and stop saying voiceline
 * - write out subtitles
 * - play the audio
 * - animate the teacher's mouth
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-24
 */

using UnityEngine;
using System.Collections;

public class TeacherVoiceline : MonoBehaviour
{
  private TeacherAudioManager audioManager;

  private Coroutine processCoroutine;
  private bool      isProcessRunning = false;

  /*
   * When this component is enabled,
   * - add a TeacherAudioManager component to teacher
   */
  void OnEnable()
  {
    Debug.Log("TeacherVoiceline.cs enabled");

    audioManager = gameObject.AddComponent<TeacherAudioManager>();
  }

  /*
   * When this component is disabled
   * - destroy the TeacherAudioManager
   */
  void OnDisable()
  {
    Debug.Log("TeacherVoiceline.cs disabled");

    Destroy(gameObject.GetComponent<TeacherAudioManager>());
  }

  /*
   * Start a voiceline
   * - write out subtitles
   * - play the audio
   * - animate the teacher's mouth
   */
  public void StartVoiceline(Voiceline voiceline)
  {
    if(audioManager == null)
    {
      Debug.LogWarning("AudioManager is undefined");

      return;
    }

    if(voiceline == null)
    {
      Debug.LogWarning($"Voiceline is undefined");

      return;
    }

    Debug.Log("Starting voiceline");

    audioManager.Play(voiceline.audio);

    // animator.isTalking = true;

    Debug.Log("Voiceline: " + voiceline.text);

    processCoroutine = StartCoroutine(Process());
  }

  /*
   * Stop the voiceline
   */
  public void StopVoiceline()
  {
    if (processCoroutine != null)
    {
      Debug.Log("Stopping voiceline");

      isProcessRunning = false;

      StopCoroutine(processCoroutine);

      OnProcessStopped();

      processCoroutine = null;
    }
  }

  /*
   * This is the process that is ran when the voiceline is being said
   */
  private IEnumerator Process()
  {
    isProcessRunning = true;

    while (isProcessRunning)
    {
      // Your process code here
      yield return null;
    }

    OnProcessStopped();
  }

  /*
   * This is ran after the voiceline has stopped
   * - stop talking animation
   */
  private void OnProcessStopped()
  {
    Debug.Log("Voiceline process stopped");

    audioManager.Stop();

    // animator.isTalking = false;
  }
}

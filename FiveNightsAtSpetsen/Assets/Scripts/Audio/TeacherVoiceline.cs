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

using TMPro;
using UnityEngine;
using System.Collections;

public class TeacherVoiceline : MonoBehaviour
{
  public  TextMeshProUGUI subtitleText  = null;
  private int             subtitleIndex = 0;

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

    // audioManager.Play(voiceline.audio);
    audioManager.Play("test");

    // animator.isTalking = true;

    Debug.Log("Voiceline: " + voiceline.text);

    // If subtitles has been setup, assign the voiceline text to it
    if(subtitleText)
    {
      subtitleText.text = voiceline.text;
    }

    processCoroutine = StartCoroutine(Process(voiceline.text));
  }

  /*
   * Stop the voiceline
   */
  public void StopVoiceline()
  {
    Debug.Log("Stopping voiceline");

    if (processCoroutine != null)
    {
      isProcessRunning = false;

      StopCoroutine(processCoroutine);

      OnProcessStopped();

      processCoroutine = null;
    }
  }

  /*
   * This is the process that is ran when the voiceline is being said
   *
   * Fix: Either commit to only use this Coroutine for subtitles,
   * or add something else in it.
   */
  private IEnumerator Process(string voicelineText)
  {
    isProcessRunning = true;

    if(subtitleText)
    {
      subtitleText.text = "";
      subtitleIndex = 0;
    }

    while(isProcessRunning)
    {
      if(subtitleText && subtitleIndex < voicelineText.Length)
      {
        subtitleText.text += voicelineText[subtitleIndex++];
      }
      else
      {
        isProcessRunning = false;
      }

      yield return new WaitForSeconds((float) 0.05);
    }

    // Display the subtitle for 1 extra second
    yield return new WaitForSeconds((float) 1.0);

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
    
    if(subtitleText)
    {
      subtitleText.text = "";
      subtitleIndex = 0;
    }
  }
}

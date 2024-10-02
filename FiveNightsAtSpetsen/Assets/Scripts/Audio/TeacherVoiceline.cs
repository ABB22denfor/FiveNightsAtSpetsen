/*
 * TeacherVoiceline.cs - Start and stop saying voiceline
 * - write out subtitles
 * - play the audio
 * - animate the teacher's mouth
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-10-01
 */

using TMPro;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TeacherAudioManager))]
public class TeacherVoiceline : MonoBehaviour
{
  [SerializeField]
  private float subtitleLetterDelay = 0.05f; // Delay between each letter

  [SerializeField]
  private TextMeshProUGUI subtitleText  = null;

  private TeacherAudioManager audioManager;
  private Animator            animator;

  private Coroutine talkingCoroutine;
  public  bool      isTalking = false;

  /*
   * When this component is enabled,
   * - add a TeacherAudioManager component to teacher
   */
  void OnEnable()
  {
    Debug.Log("TeacherVoiceline.cs enabled");

    audioManager = gameObject.GetComponent<TeacherAudioManager>();
    animator     = gameObject.GetComponentInChildren<Animator>();

    if(subtitleText)
    {
      subtitleText.text = "";
    }
  }

  /*
   * When this component is disabled
   * - destroy the TeacherAudioManager
   */
  void OnDisable()
  {
    Debug.Log("TeacherVoiceline.cs disabled");
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

    // If the teacher is already talking,
    // stop (continuing) saying whatever it said before
    if (talkingCoroutine != null)
    {
      StopCoroutine(talkingCoroutine);
    }

    Debug.Log("Starting voiceline: " + voiceline.text);

    audioManager.Play(voiceline.audio);

    animator?.SetBool("isTalking", true);

    this.isTalking = true;

    talkingCoroutine = StartCoroutine(TalkingVoiceline(voiceline.text));
  }

  /*
   * Stop the voiceline
   */
  public void StopVoiceline()
  {
    Debug.Log("Stopping teacher from talking");

    if (talkingCoroutine != null)
    {
      StopCoroutine(talkingCoroutine);

      AfterTeacherHasTalked();
    }
  }

  /*
   * This is the process that is ran when the voiceline is being said
   *
   * Fix: Either commit to only use this Coroutine for subtitles,
   * or add something else in it.
   */
  private IEnumerator TalkingVoiceline(string voicelineText)
  {
    if(subtitleText)
    {
      subtitleText.text = "";
    }

    for(int index = 0; index < voicelineText.Length; index++)
    {
      if(subtitleText)
      {
        subtitleText.text += voicelineText[index];
      }

      yield return new WaitForSeconds(subtitleLetterDelay);
    }

    // Wait until the audio has stopped playing
    while(audioManager?.audioSource?.isPlaying ?? true)
    {
      yield return new WaitForSeconds(0.1f);
    }

    AfterTeacherHasTalked();
  }

  /*
   * This is ran after the voiceline has stopped
   * - stop talking animation
   */
  private void AfterTeacherHasTalked()
  {
    Debug.Log("Teacher stopped talking");

    this.isTalking   = false;
    talkingCoroutine = null;


    audioManager.Stop();

    animator?.SetBool("isTalking", false);
    
    if(subtitleText)
    {
      subtitleText.text = "";
    }
  }
}

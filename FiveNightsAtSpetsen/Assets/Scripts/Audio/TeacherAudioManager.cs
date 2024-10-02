/*
 * Manage teacher's voiceline audio, by
 * - loading the audio files
 * - start playing voiceline audio
 * - stop  playing voiceline audio
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-10-01
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class TeacherAudioManager : MonoBehaviour
{
  [SerializeField]
  private float audioMinDistance =  1.0f;
  
  [SerializeField]
  private float audioMaxDistance = 50.0f;

  public AudioSource audioSource;

  private Dictionary<string, AudioClip> audioClips;

  /*
   * When this script is activated,
   * - create an AudioSource component
   * - load AudioClips for the teacher's voicelines
   */
  void OnEnable()
  {
    Debug.Log("TeacherAudioManager.cs enabled");

    Teacher teacher = gameObject.GetComponent<Teacher>();

    audioSource = gameObject.AddComponent<AudioSource>();

    ConfigureAudioSource();

    // If the teacher has a name to work with, start the audio manager
    if(teacher != null && teacher.teacherName != null)
    {
      LoadAudioClips(teacher.teacherName);
    }
  }

  /*
   *
   */
  private void ConfigureAudioSource()
  {
    // Set the spatial blend to 3D
    audioSource.spatialBlend = 1.0f;

    // Set the volume rolloff mode to Linear
    audioSource.rolloffMode = AudioRolloffMode.Linear;

    audioSource.minDistance = audioMinDistance;  // Full volume at this distance
    audioSource.maxDistance = audioMaxDistance; // Volume will be zero at this distance
  }

  /*
   * When this component is disabled,
   * - destroy the AudioSource component
   */
  void OnDisable()
  {
    Debug.Log("TeacherAudioManager.cs disabled");

    Destroy(gameObject.GetComponent<AudioSource>());
  }

  /*
   * Get teacher's voiceline audio clips folder name
   */
  private static string GetAudioClipsFolder(string teacherName)
  {
    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

    return textInfo.ToTitleCase(teacherName) + "Voicelines";
  }

  /*
   * Load AudioClips for the teacher's voicelines
   */
  private void LoadAudioClips(string teacherName)
  {
    Debug.Log($"Loading '{teacherName}'s audio files");

    string audioClipsFolder = GetAudioClipsFolder(teacherName);


    audioClips = new Dictionary<string, AudioClip>();

    foreach(AudioClip clip in Resources.LoadAll<AudioClip>(audioClipsFolder))
    {
      Debug.Log($"Loaded audio file: '{clip.name}'");

      audioClips[clip.name] = clip;
    }
  }

  /*
   * Start playing a voiceline from the voiceline file name
   */
  public void Play(string fileName)
  {
    if(audioClips == null)
    {
      Debug.LogWarning("Audio clips not loaded");

      return;
    }

    if(fileName == null)
    {
      Debug.LogWarning("No audio file was supplied");

      return;
    }

    if(audioClips.TryGetValue(fileName, out AudioClip clip))
    {
      Debug.Log($"Start playing audio: '{fileName}'");

      audioSource.clip = clip;

      audioSource.Play();
    }
    else
    {
      Debug.LogWarning($"Audio clip '{fileName}' not found");
    }
  }

  /*
   * Stop playing the voiceline
   */
  public void Stop()
  {
    Debug.Log($"Stop playing audio: '{audioSource.clip}'");

    audioSource.Stop();
  }
}

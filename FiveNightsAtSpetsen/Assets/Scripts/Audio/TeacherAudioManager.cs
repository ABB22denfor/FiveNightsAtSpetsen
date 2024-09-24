/*
 * Manage teacher's voiceline audio, by
 * - loading the audio files
 * - start playing voiceline audio
 * - stop  playing voiceline audio
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-24
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherAudioManager : MonoBehaviour
{
  private AudioSource audioSource;

  private Dictionary<string, AudioClip> audioClips;

  private string teacherFolder = "Audio/Sound/Teachers";

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

    // If the teacher has a name to work with, start the audio manager
    if(teacher != null && teacher.teacherName != null)
    {
      Debug.Log("Loading audio clips for teacher: " + teacher.teacherName);

      LoadAudioClips(teacher.teacherName);
    }
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
   * Load AudioClips for the teacher's voicelines
   */
  private void LoadAudioClips(string teacherName)
  {
    audioClips = new Dictionary<string, AudioClip>();


    string audioFolderPath = Path.Combine(Application.dataPath, teacherFolder, teacherName);

    string[] files = Directory.GetFiles(audioFolderPath, "*.wav");
    
    foreach(string file in files)
    {
      string fileName = Path.GetFileNameWithoutExtension(file);

      audioClips[fileName] = Resources.Load<AudioClip>(file);
    }
  }

  /*
   * Start playing a voiceline from the voiceline file name
   */
  public void Play(string fileName)
  {
    if(fileName == null)
    {
      Debug.LogWarning("No voiceline audio file was supplied");

      return;
    }

    if(audioClips.TryGetValue(fileName, out AudioClip clip))
    {
      Debug.Log("Started playing voiceline audio");

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
    Debug.Log("Stopped playing voiceline audio");

    audioSource.Stop();
  }
}

/*
 * Manage teacher's voiceline audio, by
 * - loading the audio files
 * - playing up the audio on request for voiceline
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-18
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherAudioManager
{
  private AudioSource audioSource;

  private Dictionary<string, AutioClip> audioClips;

  private static string teacherFolder = "Audio/Sound/Teachers";

  /*
   *
   */
  public TeacherAudioManager(string teacherName)
  {
    audioSource = gameObject.AddComponent<AudioSource>();

    audioClips = new Dictionary<string, AudioClip>();


    string audioFolderPath = Path.Combine(Application.Path, teacherFolder, teacherName);

    string[] files = Directory.GetFiles(audioFolderPath, "*.wav");
    
    foreach(string file in files)
    {
      string fileName = Path.GetFileNameWithoutExtension(file);

      audioClips[fileName] = Resources.Load<AudioClip>(file);
    }
  }

  /*
   *
   */
  public void Play(string fileName)
  {
     if (audioClips.TryGetValue(fileName, out AudioClip clip))
     {
       audioSource.PlayOneShot(clip);
     }
     else
     {
       Debug.LogWarning($"Audio clip '{fileName}' not found");
     }
  }
}

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
  void Awake()
  {
    audioSource = gameObject.AddComponent<AudioSource>();

    Teacher teacher = gameObject.GetComponent<Teacher>();

    // If the teacher has a name to work with, start the audio manager
    if(teacher != null && teacher.teacherName != null)
    {
      Debug.Log("Loading audio clips for teacher: " + teacher.teacherName);

      LoadAudioClips(teacher.teacherName);
    }
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
   * Play up a voiceline from the voiceline file name
   */
  public void Play(string fileName)
  {
    if(fileName == null) return;

    if(audioClips.TryGetValue(fileName, out AudioClip clip))
    {
      audioSource.PlayOneShot(clip);
    }
    else
    {
      Debug.LogWarning($"Audio clip '{fileName}' not found");
    }
  }
}

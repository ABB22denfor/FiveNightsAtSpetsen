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
  private string audioFileExtension = "*.mp3";

  private AudioSource audioSource;

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
    // Audio/Sound/Teachers/lars
    string teacherResourcesFolder = "Audio/Sound/Teachers/" + teacherName;

    string audioFolderPath = Application.dataPath + "/Resources/" + teacherResourcesFolder;


    Debug.Log($"Searching for audio files in: '{audioFolderPath}'");

    if(!Directory.Exists(audioFolderPath))
    {
      Debug.LogWarning($"'{teacherName}' doesn't have an audio folder");

      return;
    }


    Debug.Log($"Loading '{teacherName}'s audio files");

    audioClips = new Dictionary<string, AudioClip>();


    string[] files = Directory.GetFiles(audioFolderPath, audioFileExtension);
    
    foreach(string file in files)
    {
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

      // Audio/Sound/Teachers/lars/lars-alert-line-0
      string resourcesFileName = teacherResourcesFolder + "/" + fileNameWithoutExtension;

      Debug.Log($"Loading audio file: '{resourcesFileName}'");

      AudioClip clip = Resources.Load<AudioClip>(resourcesFileName);

      if(clip != null)
      {
        Debug.Log($"Loaded audio file: '{resourcesFileName}'");

        string fileName = Path.GetFileName(file);

        Debug.Log($"Storing audio file as: '{fileName}'");

        audioClips[fileName] = clip;
      }
      else
      {
        Debug.LogError($"Failed to load audio file: '{resourcesFileName}'");
      }
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
      Debug.LogWarning("No voiceline audio file was supplied");

      return;
    }

    if(audioClips.TryGetValue(fileName, out AudioClip clip))
    {
      Debug.Log($"Started playing voiceline audio: '{fileName}'");

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
    Debug.Log($"Stopped playing voiceline audio: '{audioSource.clip}'");

    audioSource.Stop();
  }
}

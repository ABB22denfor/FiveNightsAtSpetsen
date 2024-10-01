/*
 * Deserialized teacher's json voicelines object
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-10-01
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/*
 * This object is used by many files
 */
public class Voiceline
{
  public string text;
  public string audio;
}

/*
 * This object is just used to store voicelines
 */
public class TeacherLinesObject
{
  /*
   * These are the different types of voiceliens
   * 
   * The deserialized json object
   */
  public List<Voiceline> spotting;
  public List<Voiceline> general;
  public List<Voiceline> capturing;
  public List<Voiceline> hearing;
  public List<Voiceline> alert;

  public Dictionary<string, List<Voiceline>> rooms;

  /*
   * Get the teacher's voicelines json file name
   */
  private static string GetJsonFileName(string teacherName)
  {
    return teacherName + "-voicelines";
  }

  /*
   * Get the serialized json text asset (file)
   */
  private static TextAsset GetJsonTextAsset(string teacherName)
  {
    string fileName = GetJsonFileName(teacherName);

    return Resources.Load<TextAsset>(fileName);
  }

  /*
   * This is the load method, to load a teacher's json voicelines
   */
  public static TeacherLinesObject Load(string teacherName)
  {
    TextAsset textAsset = GetJsonTextAsset(teacherName);

    if(textAsset)
    {
      Debug.Log($"Loading teacher '{teacherName}' voiceline json file");

      return JsonConvert.DeserializeObject<TeacherLinesObject>(textAsset.text);
    }
    else
    {
      Debug.Log($"Teacher '{teacherName}' voiceline json file doesn't exist");

      return null;
    }
  }
}

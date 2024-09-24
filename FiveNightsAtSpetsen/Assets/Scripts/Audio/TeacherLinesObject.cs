/*
 * Deserialized teacher's json voicelines object
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-18
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
   * This is the load method, to load a teacher's json voicelines
   */
  private static string teachersFolder  = "Teachers";

  public static TeacherLinesObject Load(string teacherName)
  {
    string fileName = teacherName + ".json";
    string filePath = Path.Combine(Application.dataPath, teachersFolder, fileName).Replace('\\', '/');

    if(File.Exists(filePath))
    {
      Debug.Log($"Loading teacher '{teacherName}' json file");

      string jsonString = File.ReadAllText(filePath);

      return JsonConvert.DeserializeObject<TeacherLinesObject>(jsonString);
    }
    else
    {
      Debug.Log($"Teacher '{teacherName}' json file doesn't exist");

      return null;
    }
  }
}

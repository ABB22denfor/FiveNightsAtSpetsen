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
[System.Serializeable]
public class Voiceline
{
  public string text;
  public string audio;
}

/*
 * This object is just used to store voicelines
 */
[System.Serializeable]
public class TeacherLinesObject
{
  /*
   * These are the different types of voiceliens
   * 
   * The deserialized json object
   */
  List<Voiceline> spotting;
  List<Voiceline> general;
  List<Voiceline> capturing;
  List<Voiceline> hearing;

  public Dictionary<string, List<Voiceline>> rooms;

  /*
   * This is the load method, to load a teacher's json voicelines
   */
  private static string teachersFolder  = "Teachers/";

  public static TeacherLinesObject Load(string teacherName)
  {
    string fileName = teacherName + ".json";
    string filePath = Path.Combine(Application.path, teachersFolder + fileName);

    if(File.Exists(filePath))
    {
      string jsonString = File.ReadAllText(filePath);

      Debug.Log("Load [ " + filePath + " ]: " + jsonString);

      return JsonConvert.DeserializeObject<TeacherLinesObject>(jsonString);
    }
    else
    {
      Debug.Log("Teacher [ " + teacherName + " ] does not exist");

      return null;
    }
  }
}

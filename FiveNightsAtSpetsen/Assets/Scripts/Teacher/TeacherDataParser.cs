using UnityEngine;
using System.Collections.Generic;

public class TeacherDataParser
{
    Container data;

    public TeacherDataParser(TextAsset json)
    {
        data = JsonUtility.FromJson<Container>(json.text);
    }
    public Dictionary<string, float> GetDelays()
    {
        Dictionary<string, float> result = new();

        foreach (var entry in data.delays)
            result.Add(entry.id, entry.delay);

        return result;
    }

    public List<(string room, int repetitions)> GetRoute()
    {
        List<(string room, int repetitions)> result = new();

        foreach (var entry in data.route)
            result.Add((entry.room, entry.repetitions));

        return result;
    }


    [System.Serializable]
    public class Container
    {
        public List<DelayEntry> delays;
        public List<RouteEntry> route;
    }

    [System.Serializable]
    public class DelayEntry
    {
        public string id;
        public float delay;
    }

    [System.Serializable]
    public class RouteEntry
    {
        public string room;
        public int repetitions;
    }
}

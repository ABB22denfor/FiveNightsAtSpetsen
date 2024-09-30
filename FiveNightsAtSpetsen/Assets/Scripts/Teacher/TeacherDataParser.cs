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

    public Dictionary<string, List<(string room, int repetitions)>> GetRoute()
    {
        Dictionary<string, List<(string room, int repetitions)>> result = new() {
            {"Main", new()},
            {"Alt", new()}
        };

        foreach (var entry in data.routes.main)
            result["Main"].Add((entry.room, entry.repetitions));
        foreach (var entry in data.routes.alt)
            result["Alt"].Add((entry.room, entry.repetitions));

        return result;
    }


    [System.Serializable]
    public class Container
    {
        public List<DelayEntry> delays;
        public Routes routes;
    }

    [System.Serializable]
    public class DelayEntry
    {
        public string id;
        public float delay;
    }

    [System.Serializable]
    public class Routes
    {
        public List<RouteEntry> main;
        public List<RouteEntry> alt;
    }

    [System.Serializable]
    public class RouteEntry {
        public string room;
        public int repetitions;
    }
}

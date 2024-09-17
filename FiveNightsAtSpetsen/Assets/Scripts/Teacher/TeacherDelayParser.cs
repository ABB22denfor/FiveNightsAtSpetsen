using UnityEngine;
using System.Collections.Generic;

public class TeacherDelayParser : MonoBehaviour
{
    public static Dictionary<string, float> Parse(TextAsset json)
    {
        Dictionary<string, float> result = new();

        Container data = JsonUtility.FromJson<Container>(json.text);

        foreach (var entry in data.delays)
            result.Add(entry.id, entry.delay);

        return result;
    }

    [System.Serializable]
    public class Entry
    {
        public string id;
        public float delay;
    }

    [System.Serializable]
    public class Container
    {
        public List<Entry> delays;
    }

}

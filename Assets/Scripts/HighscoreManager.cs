
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager
{
    private static readonly string fileName = "highscore_data.json";
    public static Dictionary<int,float> localHighscores = new Dictionary<int, float>();

    public static void LoadHighscores()
    {
        localHighscores = new Dictionary<int, float>();
        string filePath = Application.persistentDataPath + "/" + fileName;
        Debug.Log("Loading highscores from: " + filePath);
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            HighscoreData tempRead = JsonUtility.FromJson<HighscoreData>(json);
            foreach (var entry in tempRead.highscores)
                localHighscores.Add(entry.level, entry.score);
        }
        else
            Debug.LogWarning("Highscore file not found, initializing new highscores.");
    }

    public static float ReadHighscore(int level)
    {
        if (localHighscores.ContainsKey(level))
            return localHighscores[level];
        Debug.LogWarning("No highscore found for level: " + level);
        return float.MaxValue;
    }

    public static void WriteHighscore(int level, float time)
    {
        if (localHighscores.ContainsKey(level))
        {
            if (time < localHighscores[level])
                localHighscores[level] = time;
        }
        else
        {
            localHighscores.Add(level, time);
        }
        List<HighscoreEntry> entries = new List<HighscoreEntry>();
        foreach (var entry in localHighscores)
            entries.Add(new HighscoreEntry { level = entry.Key, score = entry.Value });
        string json = JsonUtility.ToJson(new HighscoreData { highscores = entries });
        string filePath = Application.persistentDataPath + "/" + fileName;
        System.IO.File.WriteAllText(filePath, json);
    }

    public static string SecondsToString(float timeInSeconds)
    {
        return string.Format("{0:D2}:{1:D2}",
                Mathf.FloorToInt(timeInSeconds / 60),
                Mathf.FloorToInt(timeInSeconds % 60));
    }
}

[System.Serializable]
public class HighscoreEntry
{
    public int level;
    public float score;
}

[System.Serializable]
public class HighscoreData
{
    public List<HighscoreEntry> highscores = new List<HighscoreEntry>();
}

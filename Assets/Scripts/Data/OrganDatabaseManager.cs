using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PartData
{
    public string meshName;
    public string displayName;
    public string shortFact;
}

[System.Serializable]
public class OrganData
{
    public string id;
    public string displayName;
    public List<PartData> parts;
}

[System.Serializable]
public class OrganDatabase
{
    public List<OrganData> organs;
}

public class OrganDatabaseManager : MonoBehaviour
{
    public static OrganDatabaseManager Instance;
    private OrganDatabase db;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else Destroy(gameObject);
    }

    void Load()
    {
        string path = Path.Combine(
            Application.streamingAssetsPath,
            "organs.json");
        if (File.Exists(path))
            db = JsonUtility.FromJson
                <OrganDatabase>(
                    File.ReadAllText(path));
        else
            Debug.LogError(
                "organs.json not found: " + path);
    }

    public string GetDisplayName(
        string organId, string meshName)
    {
        var part = FindPart(organId, meshName);
        return part != null
            ? part.displayName : meshName;
    }

    public string GetShortFact(
        string organId, string meshName)
    {
        var part = FindPart(organId, meshName);
        return part != null ? part.shortFact : "";
    }

    public List<string> GetPartNames(
        string organId)
    {
        var organ = FindOrgan(organId);
        if (organ == null)
            return new List<string>();
        var names = new List<string>();
        foreach (var p in organ.parts)
            names.Add(p.meshName);
        return names;
    }

    PartData FindPart(
        string organId, string meshName)
    {
        var organ = FindOrgan(organId);
        if (organ == null) return null;
        return organ.parts.Find(
            p => p.meshName == meshName);
    }

    OrganData FindOrgan(string organId)
    {
        if (db == null) return null;
        return db.organs.Find(
            o => o.id == organId);
    }
}
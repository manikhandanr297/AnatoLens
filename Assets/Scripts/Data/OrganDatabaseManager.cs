using System.Collections;
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
    private OrganDatabase database;

    void Awake()
    {
        // Singleton — one instance across all scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadDatabase()
    {
        string path = Path.Combine(
            Application.streamingAssetsPath,
            "organs.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            database = JsonUtility
                .FromJson<OrganDatabase>(json);
            Debug.Log("OrganDatabase loaded: " +
                database.organs.Count + " organs");
        }
        else
        {
            Debug.LogError(
                "organs.json not found at: " + path);
        }
    }

    // Get display name from mesh name
    public string GetDisplayName(
        string organId, string meshName)
    {
        OrganData organ = GetOrgan(organId);
        if (organ == null) return meshName;

        PartData part = organ.parts.Find(
            p => p.meshName == meshName);

        return part != null ? part.displayName : meshName;
    }

    // Get short fact for quick display
    public string GetShortFact(
        string organId, string meshName)
    {
        OrganData organ = GetOrgan(organId);
        if (organ == null) return "";

        PartData part = organ.parts.Find(
            p => p.meshName == meshName);

        return part != null ? part.shortFact : "";
    }

    // Get full organ data
    public OrganData GetOrgan(string organId)
    {
        if (database == null) return null;
        return database.organs.Find(
            o => o.id == organId);
    }

    // Get all part names for an organ
    public List<string> GetPartNames(string organId)
    {
        OrganData organ = GetOrgan(organId);
        if (organ == null) return new List<string>();

        List<string> names = new List<string>();
        foreach (PartData part in organ.parts)
            names.Add(part.meshName);
        return names;
    }
}
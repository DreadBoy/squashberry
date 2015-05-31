using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;
using System.IO;

public class Settings : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        public String PlayerName = "";
        public Int32 Score = 0;
        public Boolean music = true;
        public Boolean SFX = true;
    }

    public static SaveData saveData = new SaveData();

    void Awake()
    {
        loadPlayerPrefs();
    }

    public void PlayerName(String name)
    {
        saveData.PlayerName = name;
        savePlayerPrefs();
    }

    public void toggleMusic()
    {
        saveData.music = !saveData.music;
        savePlayerPrefs();
    }
    public void toggleSFX()
    {
        saveData.SFX = !saveData.SFX;
        savePlayerPrefs();
    }

    public void savePlayerPrefs()
    {
        XmlSerializer serializer = new XmlSerializer(saveData.GetType());
        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, saveData);

            PlayerPrefs.SetString("Settings", writer.ToString());
        }
    }

    public void loadPlayerPrefs()
    {
        XmlSerializer serializer = new XmlSerializer(saveData.GetType());
        using (TextReader reader = new StringReader(PlayerPrefs.GetString("Settings")))
        {
            saveData = (SaveData)serializer.Deserialize(reader);
            saveData.Score = 0;
        }
    }
}

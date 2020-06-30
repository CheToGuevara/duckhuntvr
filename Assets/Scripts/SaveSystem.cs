using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string PlayerPath()
    {
        return Application.persistentDataPath + "/PlayerData.bin";
    }

    public static string LevelPath()
    {
        return Application.persistentDataPath + "/LevelData.bin";
    }

    public static void SavePlayer (PlayerProgress playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(PlayerPath(), FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();

    }


    public static PlayerProgress LoadPlayer()
    {
        if (File.Exists(PlayerPath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PlayerPath(), FileMode.Open);
            PlayerProgress playerInfo = formatter.Deserialize(stream) as PlayerProgress;
            stream.Close();
            return playerInfo;

        }
        else
        {
            PlayerProgress newPlayerProgress = PlayerProgress.CreateNewPlayer();
            SavePlayer(newPlayerProgress);
            return newPlayerProgress;
        }

    }

    public static PlayerProgress DeleteProgress()
    {
        PlayerProgress newPlayerProgress = PlayerProgress.CreateNewPlayer();
        SavePlayer(newPlayerProgress);
        return newPlayerProgress;
    }




}

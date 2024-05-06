using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Train
    public static void SaveTrain(Train train)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.lol";
        FileStream stream = new FileStream(path, FileMode.Create);

        TrainData trainData = new TrainData(train);

        formatter.Serialize(stream, trainData);
        stream.Close();
    }

    public static TrainData LoadTrain()
    {
        string path = Application.persistentDataPath + "/player.lol";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TrainData trainData = formatter.Deserialize(stream) as TrainData;
            stream.Close();

            return trainData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion

    #region Distance
    public static void SaveDistance(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.lol";
        FileStream stream = new FileStream(path, FileMode.Create);

        DistanceData distanceData = new DistanceData(gameManager);

        formatter.Serialize(stream, distanceData);
        stream.Close();
    }

    public static DistanceData LoadDistance()
    {
        string path = Application.persistentDataPath + "/player.lol";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DistanceData distanceData = formatter.Deserialize(stream) as DistanceData;
            stream.Close();

            return distanceData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion
}

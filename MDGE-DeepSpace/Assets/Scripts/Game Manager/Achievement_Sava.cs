using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Achievement_Sava
{ //This Script Serialised the achievements
    public static void AchievementSave(Achievement_Manager achievement)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine( Application.persistentDataPath , "/achievement.lel"); //path combine just makes the file locations safer in some mobile platforms, creates file
        FileStream stream = new FileStream(path, FileMode.Create); //creates the file on the harddrive in a random location

        Achievement_data data = new Achievement_data(achievement); //pass in the achievement data values, initilaise achievement data.

        formatter.Serialize(stream, data); //converts achievment data into binary while placing inside the stream, aka the file.
        stream.Close(); //close the file
    }

    public static Achievement_data AchievementLoad()
    {
        string path = Path.Combine(Application.persistentDataPath, "/achievement.lel");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
           Achievement_data data = formatter.Deserialize(fileStream) as Achievement_data;
            fileStream.Close();
            return data;
        }
        else
        {
            return null;
        }

    }

        


}

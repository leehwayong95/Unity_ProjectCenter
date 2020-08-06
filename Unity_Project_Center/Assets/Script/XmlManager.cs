using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XmlManager: MonoBehaviour
{
    public static void XmlSave<T>(T saveData, string path)
    {
        XmlSerializer sr = new XmlSerializer(typeof(T));
        using (TextWriter tw = new StreamWriter(path))
        {
            sr.Serialize(tw, saveData);
            tw.Close();
        }
    }

    public static T XmlLoad<T>(string path)
    {
        XmlSerializer sr = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            T t = (T)sr.Deserialize(fs);
            fs.Close();

            return t;
        }
    }
}

public class SaveData
{
    public long data;
    public float limitTime;
    public int penalty;
    public int stage;

    public List<Mate> empList;
}

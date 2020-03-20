using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public static string AppDataPath
    {
        get
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/android/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = "file://" + Application.dataPath + "/Raw/iphone/";
                    break;
                default:
                    path = Application.dataPath;
                    break;
            }
            return path;
        }
    }
}

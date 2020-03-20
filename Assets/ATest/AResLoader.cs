using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class AResLoader
{
    public static string LoadProto(string filename)
    {
        Debug.Log("enter LoadProto :" + filename);
        var ta = Resources.Load<TextAsset>(filename);
        Debug.Log("Load<TextAsset> :" + ta.text);
        return ta.text;
    }
}

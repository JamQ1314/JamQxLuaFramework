using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EasyTools 
{
    [UnityEditor.Callbacks.OnOpenAssetAttribute(1)]
    public static bool ClickOnce(int instanceID, int line)
    {
        return false;
    }

    [UnityEditor.Callbacks.OnOpenAssetAttribute(2)]
    public static bool ClickTwice(int instanceID, int line)
    {
        string path = AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceID));
        string name = Application.dataPath + "/" + path.Replace("Assets/", "");

        //指定打开文件类型
        if (name.EndsWith(".shader"))
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //指定打开软件
            startInfo.FileName = @"F:\workapp\...workdown\common\sublimetext3\Sublime_Text3_Stable_Build_3176_x64_Chs\sublime_text.exe";
            startInfo.Arguments = name;
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }
        return false;
    }

}

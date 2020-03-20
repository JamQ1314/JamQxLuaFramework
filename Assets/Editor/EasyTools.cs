using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
            return OpenFile(name, "sublime_text");
        }
        if (name.EndsWith(".lua"))
        {
            return OpenFile(name, "LuaPerfect");
        }
        return false;
    }

    static bool OpenFile(string filepath,string toolName)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        string sPath = Environment.GetEnvironmentVariable(toolName);
        if (string.IsNullOrEmpty(sPath)) return false;
        //指定打开软件
        startInfo.FileName = string.Format(@"{0}\{1}.exe",sPath, toolName);
        startInfo.Arguments = filepath;
        process.StartInfo = startInfo;
        process.Start();
        return true;
    }

    [MenuItem("Assets/Create/txt file")]
    static void cTxt()
    {
        CreateTXT("txt");
    }

    [MenuItem("Assets/Create/lua file")]
    static void cLua()
    {
        CreateTXT("lua");
    }

    static void CreateTXT(string exName)
    {
        var selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(selectPath)) selectPath = "Assets";
        var path = Application.dataPath.Replace("Assets", "");
        var newFileName = string.Format("newfile_{0}.{1}", DateTime.Now.ToString("hhmmss"),exName);
        var newFilePath = selectPath + "/" + newFileName;
        var fullPath = path + newFilePath;

        string createcontent = string.Format("-- create time  {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        File.WriteAllText(fullPath, createcontent, Encoding.UTF8);

        AssetDatabase.Refresh();

        //选中新创建的文件
        var asset = AssetDatabase.LoadAssetAtPath(newFilePath, typeof(Object));
        Selection.activeObject = asset;
    }


    #region win api
    ///<summary>
    /// 该函数设置由不同线程产生的窗口的显示状态
    /// </summary>
    /// <param name="hWnd">窗口句柄</param>
    /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
    /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
    [DllImport("User32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
    /// <summary>
    ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
    ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
    /// </summary>
    /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
    /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
    [MenuItem("Assets/GetEnvirmentParams")]


    [DllImport("user32.dll")]
    public static extern int ShowWindow(int hwnd,int nCmdShow);

    private const int SW_HIDE = 0;

    private const int SW_NORMAL = 1;

    private const int SW_MAXIMIZE = 3;

    private const int SW_SHOWNOACTIVATE = 4;

    private const int SW_SHOW = 5;

    private const int SW_MINIMIZE = 6;

    private const int SW_RESTORE = 9;

    private const int SW_SHOWDEFAULT = 10;

    [DllImport("user32.dll")]
    public static extern IntPtr SetFocus(IntPtr hWnd);//设定焦点

    static void Get()
    {
        //Process[] p1 = Process.GetProcessesByName("LuaPerfect");
        Process[] p1 = Process.GetProcessesByName("sublime_text");
        UnityEngine.Debug.Log("p1.len:" + p1.Length);
        UnityEngine.Debug.Log(p1[0].ProcessName);
        var hWnd = p1[0].MainWindowHandle;
        //ShowWindowAsync(hWnd, 1);
        //
        //p1[0].Start();
        var A =  SetFocus(hWnd);
        ShowWindow((int)A, 1);
        SetForegroundWindow(A);
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ABPacker : EditorWindow
{
    private static string luaPath = Path.Combine(Application.dataPath, "Lua").Replace("\\", "/");
    private static string abPath = Path.Combine(Application.dataPath, "ABGame").Replace("\\", "/");
    private static string outPath = Path.Combine(Environment.CurrentDirectory, "AssetBundle").Replace("\\", "/");

    private static string kickExtension = ".meta";

    private static string atlas = "atlas";

    [MenuItem("AssetBundle/Mark AssetBundle Res", false, 100)]
    static void MarkRes()
    {
        Clear();
        DoOverRes(abPath);
    }

    static void DoOverRes(string srcPath,bool isAtlas = false)
    {
        DirectoryInfo info = new DirectoryInfo(srcPath);
        var files = info.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension == kickExtension) continue;
            DoMark(files[i], isAtlas);
        }
        var dirs = info.GetDirectories();
        for (int i = 0; i < dirs.Length; i++)
        {
            if (!isAtlas)
            {
                if (dirs[i].Name.ToLower() == atlas)
                    DoOverRes(dirs[i].FullName, true);
                else
                    DoOverRes(dirs[i].FullName, false);
            }
            else
            {
                DoOverRes(dirs[i].FullName, true);
            }
            
        }

    }
    /// <summary>
    /// 标记AB资源
    /// </summary>
    /// <param name="file">文件信息</param>
    /// <param name="isAtlas">是否突击资源</param>
    static void DoMark(FileInfo file,bool isAtlas = false)
    {
        //资源在项目下的路径Assets/.../...
        var abFilePath = file.FullName.Replace("\\", "/");
        var projectPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/";
        string pathBellowProject = abFilePath.Replace(projectPath, string.Empty);
        //加载资源 路径必须为 Assets/.../...
        AssetImporter importer = AssetImporter.GetAtPath(pathBellowProject);
        string abName;
        if (isAtlas)
        {
            string abFoldName = file.DirectoryName.Replace("\\", "/");
            //图集资源统一用文件夹名称
            abName = abFoldName.Replace(abPath + "/", "");
            importer.assetBundleName = abName;
        }
        else
        {
            //资源在ABGame文件夹下的路径
             abName = abFilePath.Replace(abPath + "/", "");
            //修改场景文件的后缀
            string abNameExtension = file.Extension == ".unity" ? ".Scene" : "";
            abName = abName.Replace(file.Extension, abNameExtension);
            importer.assetBundleName = abName;
            Debug.Log(string.Format("<color=yellow>==>资源路径（{0}）标记名称：{1} </color> ", pathBellowProject, abName));
            //获取文件依赖
            string[] dps = AssetDatabase.GetDependencies(pathBellowProject);

            string cmpPath = abPath.Replace(Application.dataPath, "Assets") + "/";
            if (dps.Length > 0)
            {
                for (int i = 0; i < dps.Length; i++)
                {
                    if (pathBellowProject == dps[i] || dps[i].EndsWith(".cs")|| dps[i].StartsWith(cmpPath))
                        continue;
                    Debug.Log(string.Format("资源（{0}）相关依赖：{1}  AssetPathToGUID:{2}", abName, dps[i], AssetDatabase.AssetPathToGUID(dps[i])));
                    string dpsName = "Dependencies/" + AssetDatabase.AssetPathToGUID(dps[i]);
                    AssetImporter importer2 = AssetImporter.GetAtPath(dps[i]);
                    importer2.assetBundleName = dpsName;
                }
            }
        }
    }


    [MenuItem("AssetBundle/Build AssetBundle Res",false, 100)]
    static void BuildRes()
    {
        MarkRes();
        string abOutPath = outPath + "/res";
        if (Directory.Exists(abOutPath))
            Directory.Delete(abOutPath, true);
        Directory.CreateDirectory(abOutPath);   
        BuildPipeline.BuildAssetBundles(outPath + "/res" , BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        GenFiles(outPath);
        System.Diagnostics.Process.Start(abOutPath);
        Debug.Log("build res finished");
    }

    [MenuItem("AssetBundle/Clear AssetBundle Mark",false, 100)]
    static void Clear()
    {
        string[] markedAssets = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < markedAssets.Length; i++)
        {
            Debug.Log(markedAssets[i]);
            AssetDatabase.RemoveAssetBundleName(markedAssets[i], true);
        }
        Debug.Log("clean mark finished");
    }


    [MenuItem("AssetBundle/Build AssetBundle Lua",false,200)]
    static void BuidLua()
    {
        string luaOutPath = outPath + "/lua";
        if (Directory.Exists(luaOutPath))
            Directory.Delete(luaOutPath);
        Directory.CreateDirectory(luaOutPath);

        DoOverLuaFile(luaPath, luaOutPath);
    }

    private static void DoOverLuaFile(string srcPath, string dstPath)
    {
        if (!Directory.Exists(dstPath))
            Directory.CreateDirectory(dstPath);

        DirectoryInfo info = new DirectoryInfo(srcPath);
        var files = info.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension == kickExtension) continue;

            var bytes = File.ReadAllBytes(files[i].FullName);
            //可以进行一步加密
            File.WriteAllBytes(Path.Combine(dstPath, files[i].Name), bytes);
        }
        var dirs = info.GetDirectories();
        for (int i = 0; i < dirs.Length; i++)
        {
            var _src = Path.Combine(srcPath, dirs[i].Name);
            var _dst = Path.Combine(dstPath, dirs[i].Name);
            DoOverLuaFile(_src, _dst);
        }

        Debug.Log("pack lua files finished!");
    }

    [MenuItem("AssetBundle/Gen Summary File",false,300)]
    static void GenSumFile()
    {
        GenFiles(outPath);
    }
    private static List<string> lInfo;
    static void GenFiles(string path)
    {
        lInfo = new List<string>();
        GetFileInfos(outPath);
        lInfo.Insert(0, System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        File.WriteAllLines(outPath + "/all.txt", lInfo.ToArray());
        Debug.Log("gen all.txt finished!");
    }

    static void GetFileInfos(string path)
    {
        DirectoryInfo info = new DirectoryInfo(path);

        var files = info.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            var f = files[i];
            if (f.Extension == ".manifest")
                continue;

            string name = f.FullName.Replace("\\", "/").Replace(outPath + "/", "");
            string md5 = Utils.GetMD5HashFromFile(f.FullName);
            long size = f.Length;
            string fInfo = string.Format("{0}|{1}|{2}", name, md5, size);
            lInfo.Add(fInfo);
        }

        var dirs = info.GetDirectories();
        for (int i = 0; i < dirs.Length; i++)
        {
            GetFileInfos(dirs[i].FullName);
        }
    }
}

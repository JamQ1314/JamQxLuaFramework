using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class SampleScene : MonoBehaviour
{
    private LuaEnv luaEnv;
    public TextAsset mTextAsset;
    // Start is called before the first frame update
    void Start()
    {
        luaEnv = new LuaEnv();//创建lua运行环境
        luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);

        luaEnv.DoString("require 'SampleScene'");//在()里面写lua语句
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaManager: UnitySingleton<LuaManager>
{
    private LuaEnv luaEnv = null;


    private void Start()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(CustomLoader);
    }

    void Update()
    {
        luaEnv?.Tick();
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
    private byte[] CustomLoader(ref string fpath)
    {
        return System.Text.Encoding.UTF8.GetBytes(fpath);
    }

    public void StartUp()
    {
        luaEnv.DoString("require 'main'");
    }

    public object[] DoString(string script, string chunk = "chunk", LuaTable env = null)
    {
        return luaEnv.DoString(script, chunk, env);
    }



    public object[] CallLuaFunction(string luaName, string methodName, params object[] args)
    {
        LuaTable table = luaEnv.Global.Get<LuaTable>(luaName);
        LuaFunction function = table.Get<LuaFunction>(methodName);
        var result = function.Call(args);
        function.Dispose();
        function = null;
        return result;
    }
}

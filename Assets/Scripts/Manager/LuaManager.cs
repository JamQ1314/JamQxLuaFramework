using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaManager: UnitySingleton<LuaManager>
{
    private LuaEnv luaEnv = null;
    public LuaEnv LuaEnv 
    {
        get
        {
            if (luaEnv == null)
                luaEnv = new LuaEnv();
            return luaEnv;
        }
    }


    private byte[] CustomLoader(ref string fpath)
    {
        return System.Text.Encoding.UTF8.GetBytes(fpath);
    }

    public void CallLuaFunc()
    {
    }
}

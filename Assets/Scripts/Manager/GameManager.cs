using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static LuaManager mMgrLua;
    public static ResManager mMgrRes;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Init();
    }

    void Init()
    {
        mMgrLua = LuaManager.Instance;
        mMgrRes = ResManager.Instance;
    }

    void Start()
    {
        
        //资源校验更新
    }

    void ResCheckCallBack()
    {
        //管理器启动
    }


    
}


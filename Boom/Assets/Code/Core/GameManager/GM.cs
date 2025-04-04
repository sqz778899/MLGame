﻿using System;
using UnityEngine;


public class GM: MonoBehaviour
{
    public bool IsSkipStorylineMode;
    
    public PlayerManager PlayerMgr { get; private set; }
    public InventoryManager InventoryMgr { get; private set; }
    
    public BattleManager BattleMgr{ get; private set; }
    
    public StorylineManager StorylineMgr{ get; private set; }
    

    #region 单例的加载卸载
    public static GM Root { get; private set; }
    
    void Awake()
    {
        if (Root == null)
        {
            Root = this;
            DontDestroyOnLoad(gameObject);
            GameObject.Find("CanvasQ01").GetComponent<EternalCavans>().InitData();
            PlayerMgr = Root.gameObject.AddComponent<PlayerManager>();
            InventoryMgr = Root.gameObject.AddComponent<InventoryManager>();
            BattleMgr = Root.gameObject.AddComponent<BattleManager>();
            StorylineMgr = Root.gameObject.AddComponent<StorylineManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region 鼠标设置
    [SerializeField]
    private Texture2D cursorTexture; // 鼠标图标
    [SerializeField] private Vector2 hotspot = Vector2.zero; // 点击热点的偏移（例如箭头尖的位置）

    void Start() => Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    #endregion
}
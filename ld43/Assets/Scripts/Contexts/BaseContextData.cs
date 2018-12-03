using UnityEngine;
using System.Collections;
using System;

public class BaseContextData
{
    public GameInput input;
    public GameConfig config;
    public MessageQueue queue;    
}

public class ActionContextData: BaseContextData
{
    public Player player;
    public Map map;
    public IEntityController controller;
}

public class AltarContextData: BaseContextData
{
    public Action callback;
    public AltarConfig altarConfig;
    public Player player;
    public bool sacrificeAvailable;
    public ItemConfig[] choices;
    public IUIController uiController;
    public int selectedIndex;
}

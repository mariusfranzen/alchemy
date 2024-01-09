using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[Serializable]
public static class Game
{
    public static GameMasterScript GetMasterScript()
    {
        var masterScriptGameObject = GameObject.Find("GameMaster");
        return masterScriptGameObject.GetComponent<GameMasterScript>();
    }
}

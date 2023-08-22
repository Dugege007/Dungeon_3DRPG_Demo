using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：游戏主节点
 * 创建时间：
 */

public class GameController : MonoBehaviour, IController
{
    private Camera mCamera;


    public IArchitecture GetArchiteccture()
    {
        return DungeonRPG.Interface;
    }

    private void Awake()
    {
        mCamera = Camera.main;
    }
}

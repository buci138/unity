using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网格
/// </summary>
public class Grid
{
    /// <summary>
    /// 坐标点，(0,1)(1,1)
    /// </summary>
    public Vector2 Point;

    /// <summary>
    /// 世界坐标
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// 是否有植物，如果有不能在这个点上创建植物
    /// </summary>
    public bool HavePlant;

    private PlantBase currPlantBase;
    public Grid(Vector2 point, Vector2 poistion, bool havePlant)
    {
        Point = point;
        Position = poistion;
        HavePlant = havePlant;
    }

    public PlantBase CurrPlantBase { get => currPlantBase;
        set {
            currPlantBase = value;
            if (currPlantBase==null)
            {
                HavePlant = false;
            }
            else
            {
                HavePlant = true;
            }
        }
    
    }
}

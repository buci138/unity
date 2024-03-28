using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private List<Vector2> pointList = new List<Vector2>();
    private List<Grid> GirdList = new List<Grid>();


    private void Awake()
    {
        Instance = this;
        CreatGridsBaseGird();
    }
    void Start()
    {
        // CreatGridsBaseColl();
        // CreatGridsBasePointList();
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(GetGridPointByMouse());
        }
    }
    /// <summary>
    /// 基于碰撞的形式创建网格
    /// </summary>
    private void CreatGridsBaseColl()
    {
        // 创建一个预制体网格
        GameObject prefabGird = new GameObject();
        prefabGird.AddComponent<BoxCollider2D>().size =new Vector2(1,1.5f);
        prefabGird.transform.SetParent(transform);
        prefabGird.transform.position = transform.position;
        prefabGird.name = 0 + "-" + 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
               GameObject grid =  GameObject.Instantiate<GameObject>(prefabGird,transform.position+new Vector3(1.33f*i, 1.63f*j,0), Quaternion.identity,transform);
                grid.name = i + "-" + j;
            }
        }
    }

    /// <summary>
    /// 基于坐标list的形式创建网格
    /// </summary>
    private void CreatGridsBasePointList()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                pointList.Add(transform.position + new Vector3(1.33f * i, 1.63f * j, 0));
            }
        }
    }

    /// <summary>
    /// 基于Grid脚本的形式创建网格
    /// </summary>
    private void CreatGridsBaseGird()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GirdList.Add(new Grid(new Vector2(i,j), transform.position + new Vector3(1.33f * i, 1.63f * j, 0),false));
            }
        }
    }

    /// <summary>
    /// 通过鼠标获取网格坐标点
    /// </summary>
    public Vector2 GetGridPointByMouse()
    {

        return GetGridPointByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    /// <summary>
    /// 通过世界坐标来获取一个网格坐标点
    /// </summary>
    public Vector2 GetGridPointByWorldPos(Vector2 wordlPos)
    {
        return GetGridByWorldPos(wordlPos).Position;
    }

    public Grid GetGridByWorldPos(Vector2 wordlPos)
    {
        float dis = 1000000;
        Grid grid = null;
        for (int i = 0; i < GirdList.Count; i++)
        {
            if (Vector2.Distance(wordlPos, GirdList[i].Position) < dis)
            {
                dis = Vector2.Distance(wordlPos, GirdList[i].Position);
                grid = GirdList[i];
            }
        }
        return grid;
    }


    public Grid GetGridByMouse()
    {
        return GetGridByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    /// <summary>
    /// 通过Y轴来寻找一个网格，从下往上 0开始
    /// </summary>
    /// <param name="verticalNum"></param>
    /// <returns></returns>
    public Grid GetGridByVerticalNum(int verticalNum)
    {
        for (int i = 0; i < GirdList.Count; i++)
        {
            if (GirdList[i].Point == new Vector2(8,verticalNum))
            {
              
                return GirdList[i];
            }
        }
       
        return null;
    }
}

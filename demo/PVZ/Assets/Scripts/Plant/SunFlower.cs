using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : PlantBase
{
    // 创建阳光需要的时间
    private float createSunTime = 10;

    // 变成金色需要的时间
    private float goldWantTime = 1;

    public override float MaxHp {
        get {
            return 300;
        }
    }

    protected override void OnInitForPlace()
    {
        InvokeRepeating("CreateSun", createSunTime, createSunTime);
    }

    /// <summary>
    /// 创建阳光
    /// </summary>
    private void CreateSun()
    {
        //StartCoroutine(DoCreatSun());
        StartCoroutine(ColorEF(goldWantTime, new Color(1, 0.6f, 0),0.05F, InstantiateSun));
    }

    private void InstantiateSun()
    {
        Sun sun = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Sun).GetComponent<Sun>();
        sun.transform.SetParent(transform);
        // 让阳光进行跳跃动画
        sun.InitForSunFlower(transform.position);
    }
}
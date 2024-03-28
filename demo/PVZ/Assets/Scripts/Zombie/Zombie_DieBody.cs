using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_DieBody : BaseEFObj
{
    public override string AnimationName => "Zombie_DieBody";
    public override GameObject PrefabForObjPool => GameManager.Instance.GameConf.Zombie_DieBody;

    /// <summary>
    /// 用于炸死时的初始化
    /// </summary>
    public void InitForBoomDie(Vector2 pos)
    {
        Init(pos, "Zombie_BoomDie");
    }

}

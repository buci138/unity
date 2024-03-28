using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Head : BaseEFObj
{
    public override string AnimationName => "Zombie_Head";
    public override GameObject PrefabForObjPool => GameManager.Instance.GameConf.Zombie_Head;
}

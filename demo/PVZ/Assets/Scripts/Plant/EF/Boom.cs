using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : BaseEFObj
{
    public override string AnimationName => "Boom";

    public override GameObject PrefabForObjPool => GameManager.Instance.GameConf.BoomObj;
}

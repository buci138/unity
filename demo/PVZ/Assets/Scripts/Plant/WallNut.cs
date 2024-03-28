using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : PlantBase
{
    public override float MaxHp => 4000;

    private string animationName;

    protected override void HpUpdateEvent()
    {
        float state1 = (MaxHp / 3) * 2;
        float state2 = MaxHp / 3;
        if (Hp < state1 && Hp > state2) 
        {
            // 状态1
            animationName = "WallNut_State1";
        }
        else if (Hp< state2)
        {
            // 状态2
            animationName = "WallNut_State2";
        }
        else
        {
            // 正常
            animationName = "WallNut_Idel";
        }
        animator.Play(animationName);
    }
}

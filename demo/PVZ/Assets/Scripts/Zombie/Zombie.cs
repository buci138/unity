using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Zombie : ZombieBase
{

    protected override int MaxHP => 270;

    protected override float speed => 6;

    protected override float attackValue => 100;

    protected override GameObject Prefab => GameManager.Instance.GameConf.Zombie;



    public override void InitZombieHpState()
    {
        int rangeWalk = Random.Range(1, 4);
        string walkAnimationStr = "";
        switch (rangeWalk)
        {
            case 1:
                walkAnimationStr = "Zombie_Walk1";

                break;
            case 2:
                walkAnimationStr = "Zombie_Walk2";
                break;
            case 3:
                walkAnimationStr = "Zombie_Walk3";
                break;
        }
        zombieHpState = new ZombieHpState(
            0,
            new List<int>() { MaxHP,90},
            new List<string>() { walkAnimationStr, "Zombie_LostHead" },
            new List<string>() { "Zombie_Attack", "Zombie_LostHeadAttack" },
            new List<UnityAction>() { null, CheckLostHead }
            );
    }

    public override void OnDead()
    {
        // 创建一个死亡身体
        Zombie_DieBody body = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Zombie_DieBody).GetComponent<Zombie_DieBody>();
        body.Init(animator.transform.position);
    }

    /// <summary>
    /// 检查掉头
    /// </summary>
    private void CheckLostHead()
    {
        if (!isLostHead)
        {
            // 头需要失去
            isLostHead = true;
            // 创建一个头
            Zombie_Head head = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Zombie_Head).GetComponent<Zombie_Head>();
            head.Init(animator.transform.position);
            // 状态检测
            CheckState();
        }
    }

    /// <summary>
    /// 从其他僵尸哪里初始化
    /// </summary>
    public void InitForOhterZombieCreat(float time)
    {
        // 把行走动画确定在walk3
        zombieHpState.hpLimitWalkAnimationStr[0] = "Zombie_Walk3";
        animator.Play("Zombie_Walk3", 0, time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagZombie : ZombieBase
{
    protected override int MaxHP => 270;

    protected override float speed => 4;

    protected override float attackValue => 100;

    protected override GameObject Prefab => GameManager.Instance.GameConf.FlagZombie;

    public override void InitZombieHpState()
    {
        zombieHpState = new ZombieHpState(
           0,
           new List<int>() { MaxHP, 90 },
           new List<string>() { "FlagZombie_Walk", "FlagZombie_LostHeadWalk" },
           new List<string>() { "FlagZombie_Attak", "FlagZombie_LostHeadAttack" },
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
}

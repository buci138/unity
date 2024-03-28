using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : PlantBase
{
    public override float MaxHp =>300;

    protected override int attackValue => 1800;

    protected override void OnInitForPlace()
    {
        StartCoroutine(CheckBoom());
    }

    /// <summary>
    /// 检测爆炸
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckBoom()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
            {
                // 爆炸
                Boom();
            }
        } 
    }
    private void Boom()
    {
        // 播放爆炸音效
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.Boom);
        // 找到可以被我攻击的敌人，并且附加伤害
        List<ZombieBase> zombies = ZombieManager.Instance.GetZombies(transform.position, 2.25f);
        if (zombies == null) return;
        for (int i = 0; i < zombies.Count; i++)
        {
            zombies[i].BoomHurt(attackValue);
        }
        // 生成攻击特效
        Boom boom = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.BoomObj).GetComponent<Boom>();
        boom.Init(transform.position);
        // 自身死亡
        Dead();

    }
}

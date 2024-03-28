using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Peashooter : PlantBase
{
    public override float MaxHp
    {
        get
        {
            return 300;
        }
    }

    protected override float attackCD => 1.4f;

    protected override int attackValue => 20;

    // 是否可以攻击
    private bool canAttack;

    // 创建子弹的偏移量
    private Vector3 creatBulletOffsetPos= new Vector2(0.562f,0.386F);

    protected override void OnInitForPlace()
    {
        canAttack = true;
        // 可能要攻击
        InvokeRepeating("Attack",0, 0.2f);
    }

    /// <summary>
    /// 攻击方法-循环检测
    /// </summary>
    private void Attack()
    {
        if (canAttack == false) return;

        // 从僵尸管理器 获取一个离我最近的僵尸
        ZombieBase zombie=ZombieManager.Instance.GetZombieByLineMinDistance((int)currGrid.Point.y, transform.position);
        // 没有僵尸 跳出
        if (zombie == null) return;
        // 僵尸必须在草坪上 否则跳出
        if (zombie.CurrGrid.Point.x == 8 && Vector2.Distance(zombie.transform.position, zombie.CurrGrid.Position) > 1.5f) return;
        // 如果僵尸不在我的左边，也跳出
        if (zombie.transform.position.x < transform.position.x) return;


        // 从这里开始，都是可以正常攻击的
        // 在枪口实例化一个子弹
        Bullet bullet = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Bullet1).GetComponent<Bullet>();
        bullet.transform.SetParent(transform);
        bullet.Init(attackValue, transform.position + creatBulletOffsetPos);
        CDEnter();
        canAttack = false;
    }

    /// <summary>
    /// 进入CD
    /// </summary>
    private void CDEnter()
    {
        StartCoroutine(CalCD());
    }
    /// <summary>
    /// 计算冷却时间
    /// </summary>
    IEnumerator CalCD()
    {

        yield return new WaitForSeconds(attackCD);
        canAttack = true;

    }
}

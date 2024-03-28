using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieHpState
{
    // 当前HP阶段的索引 0-~ 0代表最健康的状态
    private int currHpLimitIndex;

    // 生命值阶段 maxhp,80,30
    private List<int> hpLimit;

    // 生命值阶段的不同行走动画
    public List<string> hpLimitWalkAnimationStr;

    // 生命值阶段的不同攻击动画
    private List<string> hpLimitAttackAnimationStr;

    // 不同状态切换时候要做的事情
    private List<UnityAction> hpLimitAction;

    public ZombieHpState(int currHpLimitIndex, List<int> hpLimit, List<string> hpLimitWalkAnimationStr, List<string> hpLimitAttackAnimationStr, List<UnityAction> hpLimitAction)
    {
        this.currHpLimitIndex = currHpLimitIndex;
        this.hpLimit = hpLimit;
        this.hpLimitWalkAnimationStr = hpLimitWalkAnimationStr;
        this.hpLimitAttackAnimationStr = hpLimitAttackAnimationStr;
        this.hpLimitAction = hpLimitAction;
    }

    public void UpdateZombieHpState(int hp)
    {
        int tempIndex = 0;
        // 先确定僵尸当前应该在哪个阶段
        for (int i = 0; i < hpLimit.Count; i++)
        {
            // 如果传进来的hp小于 这个hp界限的值，意味着他可能就是我们需要的当前index
            if (hp<= hpLimit[i])
            {
                tempIndex = i;
            }
        }
        // 如果需要修改
        if (currHpLimitIndex != tempIndex)
        {
            currHpLimitIndex = tempIndex;
            // 状态切换时要做的事情
            if (hpLimitAction[currHpLimitIndex]!=null)
            {
                hpLimitAction[currHpLimitIndex]();
            }
        }
    }

    /// <summary>
    /// 获取当前行走动画
    /// </summary>
    public string GetCurrWalkAnimationStr()
    {
        return hpLimitWalkAnimationStr[currHpLimitIndex];
    }
    /// <summary>
    /// 获取当前攻击动画
    /// </summary>
    public string GetCurrAttackAnimationStr()
    {
        return hpLimitAttackAnimationStr[currHpLimitIndex];
    }
}

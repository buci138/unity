using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 关卡状态
/// </summary>
public enum LVState
{ 
    // 开始游戏
    Start,
    // 战斗中
    Figth,
    // 结束
    Over
}

public class LVManager : MonoBehaviour
{
    public static LVManager Instance;
    private LVState currLVState;

    private bool isOver;
    // 在刷新僵尸中
    private bool isUpdateZombie;

    // 当前第几天 关卡数
    private int currLV;
    // 关卡中的阶段 波数
    private int stageInLV;

    private UnityAction LVStartAction;

    public LVState CurrLVState { get => currLVState;
        set {
            currLVState = value;
            switch (currLVState)
            {
                case LVState.Start:
                    // 隐藏UI主面板
                    UIManager.Instance.SetMainPanelActive(false);
                    // 刷新僵尸秀的僵尸
                    ZombieManager.Instance.UpdateZombie(5,ZombieType.Zombie);
                    // 摄像机移动到右侧观察关卡僵尸
                    Camera_C.Instance.StartMove(LVStartCameraBackAction);

                    break;
                case LVState.Figth:
                    // 显示主面板
                    UIManager.Instance.SetMainPanelActive(true);
                    // 20秒以后刷一只僵尸
                    UpdateZombie(3, 1);
                    break;
                case LVState.Over:
                    break;
            }
        }
    }

    public int StageInLV { get => stageInLV;
        set {
            stageInLV = value;
            UIManager.Instance.UpdateStageNum(stageInLV - 1);
            if (stageInLV>2)
            {
                // 杀掉当前关卡的全部僵尸，就进入下一天
                ZombieManager.Instance.AddAllZombieDeadAction(OnAllZombieDeadAction);
                CurrLVState = LVState.Over;
            }
           
        }
    }
    public int CurrLV
    {
        get => currLV;
        set
        {
            currLV = value;
            StartLV(currLV);
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CurrLV = 1;
    }
    // 开始关卡
    public void StartLV(int lv)
    {
        if (isOver) return;

        currLV = lv;
        UIManager.Instance.UpdateDayNum(currLV);
        StageInLV = 1;
        CurrLVState = LVState.Start;
    }
    private void Update()
    {
        if (isOver) return;
        FSM();
    }
    public void FSM()
    {
        switch (CurrLVState)
        {
            case LVState.Start:
                break;
            case LVState.Figth:
                // 刷僵尸
                // 如果没有在刷新僵尸，则刷新僵尸
                if (isUpdateZombie==false)
                {
                    // 意味着是最后一波，需要刷新一个旗帜僵尸
                    if (StageInLV==2)
                    {
                        ZombieManager.Instance.UpdateZombie(1, ZombieType.FlagZombie);
                    }
                    // 僵尸刷新的时间
                    float updateTime = Random.Range(15 - stageInLV / 2, 20-stageInLV/2);
                    // 僵尸刷新的数量
                    int updateNum = Random.Range(1, 3+currLV);
                    UpdateZombie(updateTime, updateNum);
                }
                break;
            case LVState.Over:
                break;
        }
    }
    /// <summary>
    /// 关卡开始时 摄像机回归后要执行的方法
    /// </summary>
    private void LVStartCameraBackAction()
    {
        // 让阳光开始创建
        SkySunManager.Instance.StartCreatSun(6);
        // 开始显示UI特效
        UIManager.Instance.ShowLVStartEF();
        // 清理掉僵尸
        ZombieManager.Instance.ClearZombie();
        CurrLVState = LVState.Figth;

        // 关卡开始时需要做的事情
        if (LVStartAction != null) LVStartAction();

    }
    /// <summary>
    /// 更新僵尸
    /// </summary>
    private void UpdateZombie(float delay,int zombieNum)
    {
        StartCoroutine(DoUpdateZombie(delay, zombieNum));
    }

    IEnumerator DoUpdateZombie(float delay, int zombieNum)
    {
        isUpdateZombie = true;
        yield return new WaitForSeconds(delay);
        // 临时测试刷新僵尸
        ZombieManager.Instance.UpdateZombie(zombieNum,ZombieType.BucketheadZombie);
        ZombieManager.Instance.ZombieStartMove();
        isUpdateZombie = false;
        StageInLV += 1;
    }
    /// <summary>
    /// 添加关卡开始事件的监听者
    /// </summary>
    public void AddLVStartActionLinstener(UnityAction action)
    {
        LVStartAction += action;
    }

    /// <summary>
    /// 当全部僵尸死亡时触发的事件
    /// </summary>
    private void OnAllZombieDeadAction()
    {
        // 更新天数
        CurrLV += 1;
        // 执行一次之后，自己移除委托
        ZombieManager.Instance.RemoveAllZombieDeadAction(OnAllZombieDeadAction);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        StopAllCoroutines();
        // 效果d
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ZombieEat);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.GameOver);

        isOver = true;
        // 逻辑
        SkySunManager.Instance.StopCreatSun();
        ZombieManager.Instance.ClearZombie();

        // UI
        UIManager.Instance.GameOver();
    }
}

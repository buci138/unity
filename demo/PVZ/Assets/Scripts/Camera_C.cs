using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Camera_C : MonoBehaviour
{
    public static Camera_C Instance;
    private void Awake()
    {
        Instance = this;
        transform.position = new Vector3(-3.02f,0.2f,-10);
    }


    /// <summary>
    /// 开始移动
    /// </summary>
    public void StartMove(UnityAction action)
    {
        // 一开始往右，然后回归，回归到终点时调用传进来的委托方法
        MoveForLVStart(() => MoveForLVStartBack(action));
    }

    /// <summary>
    /// 关卡开始时的移动
    /// </summary>
    private void MoveForLVStart(UnityAction action)
    {
        StartCoroutine(DOMove(2.83f, action));
    }
    /// <summary>
    /// 关卡开始时的摄像机回归
    /// </summary>
    private void MoveForLVStartBack(UnityAction action)
    {
        StartCoroutine(DOMove(-3.02f, action));
    }

    IEnumerator DOMove(float targetPosX, UnityAction action)
    {
        // 获取一个目标
        Vector3 target = new Vector3(targetPosX, transform.position.y, -10f);
        // 获取一个标准长度的方向
        Vector2 dir = (target - transform.position).normalized;
        // 如果我距离目标点比较远，就一直移动
        while (Vector2.Distance(target,transform.position)>0.1)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(dir * 0.1f);
        }
        // 运行到这里意味着到达了指定的位置，停留1.5秒
        yield return new WaitForSeconds(1.5f);

        if (action != null) action();

    }
}
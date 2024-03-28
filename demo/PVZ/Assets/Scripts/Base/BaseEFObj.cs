using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEFObj : MonoBehaviour
{
    private Animator animator;
    private bool isOVer;
    public abstract string AnimationName { get; }
    public abstract GameObject PrefabForObjPool { get; }
    public void Init(Vector2 pos,string animationName=null)
    {
        animator = GetComponent<Animator>();
        transform.position = pos;
        isOVer = false;
        animator.speed = 1;
        if (animationName!=null)
        {
            animator.Play(animationName, 0, 0);
        }
        else
        {
            animator.Play(AnimationName, 0, 0);
        }
        


    }

    void Update()
    {
        if (!isOVer && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            // 播放完毕
            animator.speed = 0;
            isOVer = true;
            // 2秒之后销毁自身
            Invoke("Destroy", 2);
        }
    }

    private void Destroy()
    {
        CancelInvoke();
        // 把自己放进缓存池
        PoolManager.Instance.PushObj(PrefabForObjPool, gameObject);
    }
}

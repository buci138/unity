using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVStartEF : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 显示自身
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        animator.Play("LVStartEF", 0, 0);
    }

    
}

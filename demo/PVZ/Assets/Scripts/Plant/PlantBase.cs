using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 植物的基类
/// </summary>
public abstract class PlantBase : MonoBehaviour
{
    
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// 当前植物所在的网格
    /// </summary>
    protected Grid currGrid;
    private float hp;

    protected PlantType plantType;
    public float Hp
    {
        get => hp; protected set
        {
            hp = value;
            // 做生命值发生变化瞬间要做的事情
            HpUpdateEvent();
        }
    }

    public virtual bool ZombieCanEat { get; } = true;

    /// <summary>
    /// 偏移量，相对于 网格
    /// </summary>
    protected virtual Vector2 offset { get; } = Vector2.zero;
    public abstract float MaxHp { get;}

    // 攻击的CD，也就是攻击间隔
    protected virtual float attackCD { get; }
    // 攻击力
    protected virtual int attackValue { get; }

    /// <summary>
    /// 用于创建时 网格变化时的更新
    /// </summary>
    /// <param name="gridPos"></param>
    public void UpdateForCreate(Vector2 gridPos)
    {
        transform.position = gridPos + offset;
    }

    /// <summary>
    /// 任何情况的通用初始化
    /// </summary>
    protected void InitForAll(PlantType type)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantType = type;
    }
    /// <summary>
    /// 创建时的初始化
    /// </summary>
    public void InitForCreate(bool inGird, PlantType type,Vector2 pos)
    {
        InitForAll(type);
        transform.position = pos+ offset;      
        animator.speed = 0;
        if (inGird)
        {
            spriteRenderer.sortingOrder = -1;
            spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1f);
            spriteRenderer.sortingOrder = 1;
        }
    }
    /// <summary>
    /// 放置时的初始化
    /// </summary>
    public void InitForPlace(Grid grid, PlantType type)
    {
        InitForAll(type);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Hp = MaxHp;
        currGrid = grid;
        currGrid.CurrPlantBase = this;
        transform.position = grid.Position+offset;
        animator.speed = 1;
        spriteRenderer.sortingOrder = 0;
        OnInitForPlace();
    }

    /// <summary>
    /// 受伤方法，被僵尸攻击时调用
    /// </summary>
    /// <param name="hurtValue"></param>
    public void Hurt(float hurtValue)
    {
        Hp -= hurtValue;
        // 发光效果
        StartCoroutine(ColorEF(0.2f, new Color(0.5f, 0.5f, 0.5f), 0.05f, null));
        if (Hp<=0)
        {
            // 死亡
            Dead();
        }
    }

    /// <summary>
    /// 颜色变化效果
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ColorEF(float wantTime,Color targetColor,float delayTime,UnityAction fun)
    {
        float currTime = 0;
        float lerp;
        while (currTime < wantTime)
        {
            yield return new WaitForSeconds(delayTime);
            lerp = currTime / wantTime;
            currTime += delayTime;
            spriteRenderer.color = Color.Lerp(Color.white, targetColor, lerp);
        }
        spriteRenderer.color = Color.white;
        if (fun != null) fun();

    }

    public void Dead()
    {
        if (currGrid!=null)
        {
            currGrid.CurrPlantBase = null;
            currGrid = null;
        }
        
        StopAllCoroutines();
        CancelInvoke();
        PoolManager.Instance.PushObj(PlantManager.Instance.GetPlantByType(plantType), gameObject);
    }
    protected virtual void OnInitForPlace() { }
    protected virtual void OnInitForCreate() { }
    protected virtual void OnInitForAll() { }
    protected virtual void HpUpdateEvent() { }
}

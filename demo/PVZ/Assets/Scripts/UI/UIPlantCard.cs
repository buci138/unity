using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 卡片的四种状态
/// </summary>
public enum CardState
{
    // 有阳光有CD
    CanPlace,
    // 有阳光没有CD
    NotCD,
    // 没有阳光有CD
    NotSun,
    // 都没有
    NotAll
}

public class UIPlantCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    // 遮罩图片的img组件
    private Image maskImg;

    // 自身的img组件
    private Image image;

    // 需要阳光数量的Text
    private Text wantSunText;

    // 种植需要多少阳光
    public int WantSunNum;

    // 冷却时间：几秒可以放置一次植物
    public float CDTime;

    // 当前时间：用于冷却时间的计算
    private float currTimeForCd;

    // 是否可以放置植物的CD
    private bool canPlace;

    // 植物的预制体
    private GameObject prefab;

    // 是否需要放置植物
    private bool wantPlace;

    // 用来创建的植物
    private PlantBase plant;

    // 在网格中的植物，它是透明的
    private PlantBase plantInGrid;

    // 当前卡片所对应的植物类型
    public PlantType CardPlantType;

    private CardState cardState = CardState.NotAll;

    public CardState CardState { get => cardState;
        set 
        {
            // 如果要修改成为的值和当前值一样，就跳出，不需要运行任何逻辑
            if (cardState==value)
            {
                return;
            }
            
            switch (value)
            {
                case CardState.CanPlace:
                    // CD没有遮罩 自身是明亮的
                    maskImg.fillAmount = 0;
                    image.color = Color.white;
                    break;
                case CardState.NotCD:
                    // CD有遮罩 自身是明亮的
                    image.color = Color.white;
                    if (cardState == CardState.NotAll) return;
                    CDEnter();
                    break;
                case CardState.NotSun:
                    // CD没有遮罩 自身是昏暗的
                    maskImg.fillAmount = 0;
                    image.color = new Color(0.75f, 0.75f, 0.75f);
                    break;
                case CardState.NotAll:
                    image.color = new Color(0.75f, 0.75f, 0.75f);
                    if (cardState == CardState.NotCD) return;
                    CDEnter();
                    break;

            }

            cardState = value;
        }
    
    }
    public bool CanPlace { get => canPlace;
        set {
            canPlace = value;
            CheckState();
        }
    }

    public bool WantPlace { get => wantPlace;
        set {
            wantPlace = value;
            if (wantPlace)
            {
                prefab = PlantManager.Instance.GetPlantByType(CardPlantType);
                plant = PoolManager.Instance.GetObj(prefab).GetComponent<PlantBase>();
                plant.transform.SetParent(PlantManager.Instance.transform);
                plant.InitForCreate(false, CardPlantType,Vector2.zero);
                UIManager.Instance.CurrCard = this;
            }
            else
            {
                if (plant!=null)
                {
                    plant.Dead();
                    plant = null;
                }
                
            }
        }
    }

    

    void Start()
    {
        maskImg = transform.Find("Mask").GetComponent<Image>();
        wantSunText = transform.Find("Text").GetComponent<Text>();
        wantSunText.text = WantSunNum.ToString();
        image = GetComponent<Image>();
        CanPlace = true;
        PlayerManager.Instance.AddSunNumUpdateActionListener(CheckState);

        LVManager.Instance.AddLVStartActionLinstener(OnLVStartAction);
    }

    private void Update()
    {
        // 如果需要放置植物，并且要放置的植物不为空
        if (WantPlace && plant!=null)
        {
            // 让植物跟随我们的鼠标
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Grid grid = GridManager.Instance.GetGridByWorldPos(mousePoint);

            plant.transform.position = new Vector3(mousePoint.x, mousePoint.y,0);
            
            // 如果我距离网格比较近，并且它没有植物 需要在网格上出现一个透明的植物
            if (grid.HavePlant==false&& Vector2.Distance(mousePoint, grid.Position) <1.5)
            {
                if (plantInGrid ==null)
                {
                    plantInGrid = PoolManager.Instance.GetObj(prefab).GetComponent<PlantBase>();
                    plantInGrid.transform.SetParent(PlantManager.Instance.transform);
                    plantInGrid.InitForCreate(true, CardPlantType, grid.Position);
                }
                else
                {
                    plantInGrid.UpdateForCreate(grid.Position);
                }
                

                // 如果点击鼠标，需要放置植物
                if (Input.GetMouseButtonDown(0))
                {
                    plant.InitForPlace(grid, CardPlantType);
                    plant = null;
                    plantInGrid.Dead();
                    plantInGrid = null;
                    WantPlace = false;
                    CanPlace = false;
                    AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.Place);
                    // 种植成功需要减少玩家的阳光
                    PlayerManager.Instance.SunNum -= WantSunNum;
                }

            }
            else
            {
                if (plantInGrid!=null)
                {
                    plantInGrid.Dead();
                    plantInGrid = null;
                }
                
            }

        }
        // 如果右键取消放置状态
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlace();
         
        }
    }

    /// <summary>
    /// 取消放置
    /// </summary>
    private void CancelPlace()
    {
        if (plant != null) plant.Dead();
        if (plantInGrid != null) plantInGrid.Dead();
        plant = null;
        plantInGrid = null;
        WantPlace = false;
    }

    /// <summary>
    /// 在关卡开始时需要做的事情
    /// </summary>
    private void OnLVStartAction()
    {
        CancelPlace();
        // 重置CD
        CanPlace = true;
    }

    /// <summary>
    /// 状态检测
    /// </summary>
    private void CheckState()
    {
        //  有CD 有阳光
        if (canPlace && PlayerManager.Instance.SunNum >= WantSunNum)
        {
            CardState = CardState.CanPlace;
        }
        // 没有CD 有阳光
        else if (!canPlace && PlayerManager.Instance.SunNum >= WantSunNum)
        {
            CardState = CardState.NotCD;
        }
        // 有CD 没有阳光
        else if (canPlace && PlayerManager.Instance.SunNum < WantSunNum)
        {
            CardState = CardState.NotSun;
        }
        // 都没有
        else
        {
            CardState = CardState.NotAll;
        }
    }

    /// <summary>
    /// 进入CD
    /// </summary>
    private void CDEnter()
    {
        maskImg.fillAmount = 1;
        // 遮罩后，开始计算冷却
        StartCoroutine(CalCD());
    }
    /// <summary>
    /// 计算冷却时间
    /// </summary>
    IEnumerator CalCD()
    {
        float calCD = (1 / CDTime) * 0.1f;
        currTimeForCd = CDTime;
        while (currTimeForCd>=0)
        {
            yield return new WaitForSeconds(0.1f);
            maskImg.fillAmount -= calCD;
            currTimeForCd -= 0.1f;
        }

        // 到这里，意味着冷却时间到了，可以放置了
        CanPlace = true;

    }

    // 鼠标移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardState != CardState.CanPlace) return;
        transform.localScale = new Vector2(1.05f,1.05f);
    }
    // 鼠标移除

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardState != CardState.CanPlace) return;
        transform.localScale = new Vector2(1f, 1f);

    }
    // 鼠标点击时的效果,放置植物
    public void OnPointerClick(PointerEventData eventData)
    {
        if (CardState != CardState.CanPlace) return;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        if (!WantPlace)
        {
            WantPlace = true;
        }
    }
}

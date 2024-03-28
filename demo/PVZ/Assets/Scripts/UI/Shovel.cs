using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shovel : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Transform shoveImg;
    // 是否在使用铲子中
    private bool isShove;

    public bool IsShove 
    {
        get { return isShove; }
        set {
            isShove = value;
            // 需要铲植物
            if (isShove)
            {
                AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.Shovel);
                shoveImg.localRotation = Quaternion.Euler(0, 0, 45);

            }
            // 把铲子放回去
            else
            {
                AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.Shovel);
                shoveImg.localRotation = Quaternion.Euler(0, 0, 0);
                shoveImg.transform.position = transform.position;
            }           
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsShove==false)
        {
            IsShove = true;
        }
                
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shoveImg.transform.localScale = new Vector2(1.4f, 1.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shoveImg.transform.localScale = new Vector2(1f, 1f);
    }

    void Start()
    {
        shoveImg = transform.Find("Image");
        LVManager.Instance.AddLVStartActionLinstener(OnLVStartAction);
    }

    void Update()
    {
        // 如果需要铲植物
        if (IsShove)
        {
            shoveImg.position = Input.mousePosition;

            // 点击左键，判断是否要铲除植物
            if (Input.GetMouseButtonDown(0))
            {
                Grid grid= GridManager.Instance.GetGridByMouse();
                // 如果没有植物，直接跳过所有逻辑
                if (grid.CurrPlantBase == null) return;
                // 如果鼠标距离网格的距离小于1.5米，则杀死这个植物
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),grid.CurrPlantBase.transform.position)<1.5f)
                {
                    grid.CurrPlantBase.Dead();
                    IsShove = false;
                }                        
            }

            // 点击右键，取消铲子状态
            if (Input.GetMouseButtonDown(1))
            {
                IsShove = false;
            }
        }
                    
    }

    private void OnLVStartAction()
    {
        IsShove = false;
    }
}

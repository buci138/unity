using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPanel : MonoBehaviour
{
    public void Show(bool isShow)
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        gameObject.SetActive(isShow);
        // 如果显示出来，意味着游戏暂停
        if (isShow)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;

        }

    }

    public void BackMainScene()
    {
        PoolManager.Instance.Clear();
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        Time.timeScale = 1;
        Invoke("DoBackMainScene",0.5f);
    }

    private void DoBackMainScene()
    {
        SceneManager.LoadScene("Start");
    }
    public void Quit()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        Application.Quit();
    }
}

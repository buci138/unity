using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void GoEndLess()
    {
        // 清理对象池
        PoolManager.Instance.Clear();
        // 播放音效
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        Invoke("DoGoEndLess", 0.5f);
        
    }
    private void DoGoEndLess()
    {
        SceneManager.LoadScene("EndLess");
    }
    public void Quit()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    // 攻击力
    private int attackValue;
    // 是否击中
    private bool isHit;
    public void Init(int attackValue,Vector2 pos)
    {
        transform.position = pos;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody.AddForce(Vector2.right*300);
        this.attackValue = attackValue;
        rigidbody.gravityScale = 0;
        isHit = false;
        // 修改成正常状态的图片
        spriteRenderer.sprite = GameManager.Instance.GameConf.Bullet1Nor;
    }

    void Update()
    {
        if (isHit) return;
        if (transform.position.x>7.7F)
        {
            Destroy();
            return;
        }
        transform.Rotate(new Vector3(0,0,-15f));
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (isHit) return;
        if (coll.tag == "Zombie")
        {
            isHit = true;
            // 播放僵尸被豌豆攻击的音效
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ZombieHurtForPea);

            // 让僵尸受伤
            coll.GetComponentInParent<ZombieBase>().Hurt(attackValue);
            // 修改成击中图片
            spriteRenderer.sprite = GameManager.Instance.GameConf.Bullet1Hit;

            // 暂停自身的运动
            rigidbody.velocity = Vector2.zero;

            // 下落
            rigidbody.gravityScale = 1;

            // 销毁自身
            Invoke("Destroy", 0.5f);

        }
    }

    private void Destroy()
    {
        // 取消延迟调用
        CancelInvoke();
        // 把自己放进缓存池
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Bullet1,gameObject);
    }
}

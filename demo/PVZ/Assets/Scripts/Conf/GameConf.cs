using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameConf",menuName ="GameConf")]
public class GameConf : ScriptableObject
{

    [Header("音乐")]
    public GameObject EFAudio;
    public AudioClip ButtonClick;
    public AudioClip Pause;
    public AudioClip Shovel;
    public AudioClip Place;
    public AudioClip SunClick;
    public AudioClip Boom;

    public AudioClip ZombieEat;
    public AudioClip ZombieHurtForPea;
    public AudioClip ZombieGroan;

    public AudioClip GameOver;

    
    [Header("植物")]
    [Tooltip("阳光")]
    public GameObject Sun;
    [Tooltip("太阳花")]
    public GameObject SunFlower;
    [Tooltip("豌豆射手")]
    public GameObject Peashooter;
    [Tooltip("坚果")]
    public GameObject WallNut;
    [Tooltip("地刺")]
    public GameObject Spike;
    [Tooltip("樱桃")]
    public GameObject Cherry;


    [Header("僵尸")]
    [Tooltip("僵尸的头")]
    public GameObject Zombie_Head;
    [Tooltip("僵尸的身体")]
    public GameObject Zombie_DieBody;
    [Tooltip("普通僵尸")]
    public GameObject Zombie;
    [Tooltip("旗帜僵尸")]
    public GameObject FlagZombie;
    [Tooltip("路障僵尸")]
    public GameObject ConeheadZombie;
    [Tooltip("铁通僵尸")]
    public GameObject BucketheadZombie;
    

    [Header("子弹")]
    [Tooltip("豌豆")]
    public GameObject Bullet1;
    [Tooltip("豌豆正常")]
    public Sprite Bullet1Nor;
    [Tooltip("豌豆击中")]
    public Sprite Bullet1Hit;
    [Tooltip("爆炸效果")]
    public GameObject BoomObj;




}

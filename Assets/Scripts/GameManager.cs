using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //다른 오브젝트에서 접근 할수 있도록 static으로 지정
    public static GameManager instance;

    //인스펙터의 속성들을 구분해주는 타이틀 
    [Header("# Game Control")]

    //게임 정지
    public bool isLive;

    //게임 시작과 게임 오버 
    public bool gameStart;

    //게임 시간
    public float gameTime;

    //스테이지 시간
    public float maxGameTime = 600f;

    //사운드
    AudioSource audioSource;
    public AudioClip[] audioClips;

    [Header("# Player Info")]
    //적을 죽였을 때 업데이트 할 데이터 변수
    //적을 죽일 때 킬 수가 올라가고, 경험치가 쌓여 다음 레벨 경험치에 도달하면 레벨업
    public int level;
    public int kill;
    public float exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    public int initialHp;
    public int hp;

    public bool isBoss;

    [Header("# Game Object")]
    //오브젝트 풀링 스크립트 등록
    public PoolMangaer pool;

    public Spawner spawner;

    //player 오브젝트 등록
    public Player player;

    //LevelUp 오브젝트 등록
    public LevelUp uiLevelUp;

    public GameObject gameoverPop;
    public GameObject gameclearPop;

    void Awake()
    {
        isLive = true;
        instance = this;
        gameStart = false;
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    void Start()
    {
        // 임시 스크립트(기본무기 장착)    
        //uiLevelUp.Select(0);      
           
    }

    // Update is called once per frame
    void Update()
    {    

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)  
        {
            if(!isBoss)
            {
                spawner.GetComponent<Spawner>().isSpawn = true;
                isBoss = true;
                gameTime = 0;
            }
            else if (isBoss)
            {
                GameOver();
                gameTime = maxGameTime;
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        audioSource.loop = false;
        audioSource.clip = audioClips[1];
        audioSource.volume = 1;
        audioSource.Play();
        gameoverPop.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level : " + level;
        gameoverPop.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Kill : " + kill;
        gameoverPop.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Time : " + (int)gameTime;
        gameoverPop.SetActive(true);
        Stop();
    }

    public void GameClear()
    {
        Debug.Log("GAME CLEAR");
        audioSource.loop = false;
        audioSource.clip = audioClips[0];
        audioSource.volume = 1;
        audioSource.Play();
        gameclearPop.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Level : " + level;
        gameclearPop.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Kill : " + kill;
        gameclearPop.SetActive(true);
        Stop();
    }

    //적이 죽었을 때 경험치가 올라가고 레벨에 해당하는 경험치 도달시 레벨업
    public void GetExp() 
    {
       
        exp ++;

        if (exp >= nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void AddExp(float p)
    {
        float add = level * p;
        exp += add;

        if (exp >= nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void AddHp(int add)
    {
        hp += add;

        if (hp >= initialHp)
        {
            hp = initialHp;
        }
    }




    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

}

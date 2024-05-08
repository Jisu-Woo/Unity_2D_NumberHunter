using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //�ٸ� ������Ʈ���� ���� �Ҽ� �ֵ��� static���� ����
    public static GameManager instance;

    //�ν������� �Ӽ����� �������ִ� Ÿ��Ʋ 
    [Header("# Game Control")]

    //���� ����
    public bool isLive;

    //���� ���۰� ���� ���� 
    public bool gameStart;

    //���� �ð�
    public float gameTime;

    //�������� �ð�
    public float maxGameTime = 600f;

    //����
    AudioSource audioSource;
    public AudioClip[] audioClips;

    [Header("# Player Info")]
    //���� �׿��� �� ������Ʈ �� ������ ����
    //���� ���� �� ų ���� �ö󰡰�, ����ġ�� �׿� ���� ���� ����ġ�� �����ϸ� ������
    public int level;
    public int kill;
    public float exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    public int initialHp;
    public int hp;

    public bool isBoss;

    [Header("# Game Object")]
    //������Ʈ Ǯ�� ��ũ��Ʈ ���
    public PoolMangaer pool;

    public Spawner spawner;

    //player ������Ʈ ���
    public Player player;

    //LevelUp ������Ʈ ���
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
        // �ӽ� ��ũ��Ʈ(�⺻���� ����)    
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

    //���� �׾��� �� ����ġ�� �ö󰡰� ������ �ش��ϴ� ����ġ ���޽� ������
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

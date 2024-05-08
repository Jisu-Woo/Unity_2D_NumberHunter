using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //자식 오브젝트의 위치 값을 담을 Trasform 변수
    public Transform[] spawnPoint;

    //스폰 데이터를 담은 클래스
    public SpawnData[] spawnData;

    //소환 레벨, 증가할 수록 더 강하고 많은 적이 생성됨
    int level;
    
    //소환 레벨을 올릴 시간
    public float levelTime = 60f;

    float timer;

    public bool isSpawn;

    void Awake()
    {
        //자식들의 위치를 스폰포인트로 가져옴
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        //플레이어가 살아있는 상태에만 실행
        if (!GameManager.instance.isLive)
            return;

        //타이머
        timer += Time.deltaTime;

        //levelTime이 될때마다 float를 int로 변환하여 레벨의 숫자를 변경
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), 8); 

        //스폰 시간이 될 때마다 스폰 함수를 호출하고 타이머를 초기화
        //스폰 데이터를 가져와 스폰 데이터에 정의된 스폰 시간을 사용
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn(0);
        }

        if(isSpawn)
        {
            Spawn(1);
            isSpawn = false;
        }    
    }

    //enemy를 스폰하는 함수
    public void Spawn(int n)
    {
        GameObject enemy = null;

        if (n == 0)
        {
            enemy = GameManager.instance.pool.Get(Random.Range(0, level+1));

            //정해진 스폰 포인트 중 랜덤으로 한 곳을 골라 해당 위치로 생성
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

            //Enemy의 스폰 데이터를 가져와 레벨에 따라 다른 적을 생성함
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }
            //에너미 프리팹을 가져옴
        else if (n == 1)
        { 
            enemy = GameManager.instance.pool.Get(9);

            //정해진 스폰 포인트 중 랜덤으로 한 곳을 골라 해당 위치로 생성
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

            //Enemy의 스폰 데이터를 가져와 레벨에 따라 다른 적을 생성함
            enemy.GetComponent<Enemy>().Init(spawnData[9]);
        }
            
        
    }
}

//레벨 디자인을 위해 시간이 지날수록 강해지는 적을 만들기 위한 스폰 데이터 클래스
//에디터에서 값을 직접 수정할 수 있도록 Serializable을 통해 직렬화
[System.Serializable]
public class SpawnData
{
    //스프라이트 타입
    public int spriteType;
    //소환시간
    public float spawnTime;
    //체력
    public int hp;
    //속도
    public float speed;
}

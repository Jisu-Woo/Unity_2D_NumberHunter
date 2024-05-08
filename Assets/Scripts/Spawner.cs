using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //�ڽ� ������Ʈ�� ��ġ ���� ���� Trasform ����
    public Transform[] spawnPoint;

    //���� �����͸� ���� Ŭ����
    public SpawnData[] spawnData;

    //��ȯ ����, ������ ���� �� ���ϰ� ���� ���� ������
    int level;
    
    //��ȯ ������ �ø� �ð�
    public float levelTime = 60f;

    float timer;

    public bool isSpawn;

    void Awake()
    {
        //�ڽĵ��� ��ġ�� ��������Ʈ�� ������
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        //�÷��̾ ����ִ� ���¿��� ����
        if (!GameManager.instance.isLive)
            return;

        //Ÿ�̸�
        timer += Time.deltaTime;

        //levelTime�� �ɶ����� float�� int�� ��ȯ�Ͽ� ������ ���ڸ� ����
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), 8); 

        //���� �ð��� �� ������ ���� �Լ��� ȣ���ϰ� Ÿ�̸Ӹ� �ʱ�ȭ
        //���� �����͸� ������ ���� �����Ϳ� ���ǵ� ���� �ð��� ���
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

    //enemy�� �����ϴ� �Լ�
    public void Spawn(int n)
    {
        GameObject enemy = null;

        if (n == 0)
        {
            enemy = GameManager.instance.pool.Get(Random.Range(0, level+1));

            //������ ���� ����Ʈ �� �������� �� ���� ��� �ش� ��ġ�� ����
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

            //Enemy�� ���� �����͸� ������ ������ ���� �ٸ� ���� ������
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }
            //���ʹ� �������� ������
        else if (n == 1)
        { 
            enemy = GameManager.instance.pool.Get(9);

            //������ ���� ����Ʈ �� �������� �� ���� ��� �ش� ��ġ�� ����
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

            //Enemy�� ���� �����͸� ������ ������ ���� �ٸ� ���� ������
            enemy.GetComponent<Enemy>().Init(spawnData[9]);
        }
            
        
    }
}

//���� �������� ���� �ð��� �������� �������� ���� ����� ���� ���� ������ Ŭ����
//�����Ϳ��� ���� ���� ������ �� �ֵ��� Serializable�� ���� ����ȭ
[System.Serializable]
public class SpawnData
{
    //��������Ʈ Ÿ��
    public int spriteType;
    //��ȯ�ð�
    public float spawnTime;
    //ü��
    public int hp;
    //�ӵ�
    public float speed;
}

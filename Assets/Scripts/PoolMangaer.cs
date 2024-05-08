using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMangaer : MonoBehaviour
{
    //�����յ��� ������ ����
    public GameObject[] prefabs;

    //Ǯ ��� ����Ʈ
    List<GameObject>[] pools;

    void Awake()
    {
        //�迭�� ũ�⸸ŭ �����Ͽ� �迭 �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        //�迭 ���� ��� ������Ʈ ��ҵ��� �ʱ�ȭ�ϴ� �ݺ��� 
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    //input���� �־��� index ��ȣ�� �Ҵ�� ������Ʈ�� ã��, �ش� ������Ʈ�� Ȱ��ȭ �ϴ� �Լ�
    public  GameObject Get(int index)
    {
        //������Ʈ�� �����ϰ� Ȱ��ȭ�� ���� ���� ������Ʈ
        GameObject select = null;

        //foreach: �迭�� �����͸� ���������� Ž���ϴ� for�� 
        foreach (GameObject item in pools[index])
        {
            //Ž���� �������� �߰����� ��
            if (!item.activeSelf)
            {
                //�ش� ������Ʈ�� select�� �Ҵ��ϰ� Ȱ��ȭ ��, ���̻� Ž���� �ʿ䰡 �������� �ݺ��� ����
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //Ž���� �������� ã�� ���� ���
        if (!select) 
        { 
            //���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform);
            //pool�� ���
            pools[index].Add(select);
        }
        return select;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMangaer : MonoBehaviour
{
    //프리팹들을 보관할 변수
    public GameObject[] prefabs;

    //풀 담당 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        //배열의 크기만큼 지정하여 배열 초기화
        pools = new List<GameObject>[prefabs.Length];

        //배열 안의 모든 오브젝트 요소들을 초기화하는 반복문 
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    //input으로 주어진 index 번호에 할당된 오브젝트를 찾고, 해당 오브젝트를 활성화 하는 함수
    public  GameObject Get(int index)
    {
        //오브젝트를 선택하고 활성화를 위한 게임 오브젝트
        GameObject select = null;

        //foreach: 배열의 데이터를 순차적으로 탐색하는 for문 
        foreach (GameObject item in pools[index])
        {
            //탐색한 아이템을 발견했을 때
            if (!item.activeSelf)
            {
                //해당 오브젝트를 select에 할당하고 활성화 후, 더이상 탐색할 필요가 없음으로 반복문 종료
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //탐색한 아이템을 찾지 못한 경우
        if (!select) 
        { 
            //새롭게 생성하고 select 변수로 할당
            select = Instantiate(prefabs[index], transform);
            //pool에 등록
            pools[index].Add(select);
        }
        return select;
    }

}

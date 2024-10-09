using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // prefab 보관 변수
    public GameObject[] prefabs;

    // 풀 담당 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 비활성화된 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                // 발견시 select에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 못찾으면
        if (select == null)
        {
            // 새롭게 생성후 select에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}

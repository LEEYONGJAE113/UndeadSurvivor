using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnDatas;

    int level;
    float timer; // 몹소환 타이머

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f);

        if (timer > (spawnDatas[level].spawnTime))
        {
            Spawn();
            timer = 0;
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // children에 자기 자신도 포함돼서 1부터 시작
        enemy.GetComponent<Enemy>().Init(spawnDatas[level]); 
    }
}

[System.Serializable] // 직렬화
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
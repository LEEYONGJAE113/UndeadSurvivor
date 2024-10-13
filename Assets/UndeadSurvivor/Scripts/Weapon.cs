using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabsId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
        
        // test..
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0) { Place(); }
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver); // 무기 레벨업했을때도 기존 기어가 적용되게끔
    }

    public void Init(ItemData data)
    {
        // basic setting
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property setting
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabsId =  index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                Place();
                break;
            default:
                speed = 0.3f;
                break;
        }

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver); // 무기 처음 생성했을때도 기존 기어가 적용되게끔
    }

    void Place()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index); // 이미 가지고 있으면 풀매니저에서 생성안하고 그걸 쓰겠다
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabsId).transform; // 모자라면 풀에서 생성
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero; 
            bullet.localRotation = Quaternion.identity;
            // 회전무기 생성시 0,0,0에서 생성되도록
            
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget) { return; }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
    
        Transform bullet = GameManager.instance.pool.Get(prefabsId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // 지정된 축을 중심으로 목표를 향해 회전
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }

}

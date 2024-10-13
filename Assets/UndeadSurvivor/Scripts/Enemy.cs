using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }


    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0)/*현재 애니메이션 상태(애니메이션 레이어의 인덱스)*/.IsName("Hit") ) { return; }
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 물리속도가 이동에 영향이 없도록
    }

    void LateUpdate()
    {
        if (!isLive) { return; }
        spriter.flipX = (target.position.x < rigid.position.x);
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive/*연속 2번사망 방지*/) { return; }
        
        health -= collision.GetComponent<Bullet>().damage; //bullet 스크립트 가져와서 체력깎기
        StartCoroutine(KnockBack()); //코루틴 실행함수, == StartCoroutine("KnockBack");

        if(health > 0)
        {
            //생존, 히트액션...
            anim.SetTrigger("Hit");
        }
        else
        {
            // 사망
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack() //코루틴 인터페이스, 코루틴은 비동기함수
    {
        // yield return null; // 1프레임 쉬기
        // yield return new WaitForSeconds(2f); // 2초 쉬기
        yield return wait; // 다음 하나의 물리프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3/*넉백사이즈*/, ForceMode2D.Impulse/*순간적인 힘*/); 

    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner; 

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake() // 보통 초기화는 awake에서 함
    {
        rigid = GetComponent<Rigidbody2D>();
        speed = 3f;
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }


    // void Update()
    // {
    //     inputVec.x = Input.GetAxisRaw("Horizontal"); // project settings - input manager - axis
    //     inputVec.y = Input.GetAxisRaw("Vertical"); // Raw를 빼면 미끄러지듯이 나감
    // }

    void FixedUpdate() // 물리연산 프레임마다 호출
    {
        Vector2 nextVec = inputVec/*.normalized*/ * speed * Time.fixedDeltaTime; // fixedDeltaTime은 FixedUpdate기준으로 동작
        // // 1. 힘을 준다
        // rigid.AddForce(inputVec);
        // // 2. 속도 제어
        // rigid.velocity = inputVec;
        // 3. 위치 이동
        rigid.MovePosition(rigid.position + nextVec); // 현재 위치를 더해줘야함
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate() // 후처리, 프레임이 종료되기전 처리
    {
        anim.SetFloat("Speed", inputVec.magnitude); // magnitude : 벡터의 순수 크기

        if (inputVec.x != 0)
        {
            spriter.flipX = (inputVec.x < 0); // bool값을 반환해서 flipX를 체크할지 말지
        }
    }
}

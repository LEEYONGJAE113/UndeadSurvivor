using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 _inputVec;
    [SerializeField]
    private float _speed;

    Rigidbody2D _rigid;
    SpriteRenderer _spriter;
    Animator _anim;

    void Awake() // 보통 초기화는 awake에서 함
    {
        _rigid = GetComponent<Rigidbody2D>();
        _speed = 3f;
        _spriter = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }


    // void Update()
    // {
    //     _inputVec.x = Input.GetAxisRaw("Horizontal"); // project settings - input manager - axis
    //     _inputVec.y = Input.GetAxisRaw("Vertical"); // Raw를 빼면 미끄러지듯이 나감
    // }

    void FixedUpdate() // 물리연산 프레임마다 호출
    {
        Vector2 nextVec = _inputVec/*.normalized*/ * _speed * Time.fixedDeltaTime; // fixedDeltaTime은 FixedUpdate기준으로 동작
        // // 1. 힘을 준다
        // _rigid.AddForce(_inputVec);
        // // 2. 속도 제어
        // _rigid.velocity = _inputVec;
        // 3. 위치 이동
        _rigid.MovePosition(_rigid.position + nextVec); // 현재 위치를 더해줘야함
    }

    void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();
    }

    void LateUpdate() // 후처리, 프레임이 종료되기전 처리
    {
        _anim.SetFloat("Speed", _inputVec.magnitude); // magnitude : 벡터의 순수 크기

        if (_inputVec.x != 0)
        {
            _spriter.flipX = (_inputVec.x < 0); // bool값을 반환해서 flipX를 체크할지 말지
        }
    }
}

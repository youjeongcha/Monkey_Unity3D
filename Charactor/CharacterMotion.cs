using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // InputAction의 Behavior를 


[RequireComponent(typeof(Rigidbody))]
public class CharacterMotion : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] Transform characterMesh; // Mesh기준
    [SerializeField] float speed = 4.0f;

    //캐릭터 회전
    private Vector3 turnTextureAngle; // Mesh 방향 조절

    //Jump
    private bool isStart;
    private bool isJump;

    Rigidbody rigid;

    Vector3 direction = Vector3.zero;

    void Start()
    {
        isStart = true;
        isJump = false;
        turnTextureAngle = Vector3.zero;

        rigid = GetComponent<Rigidbody>();

        rigid.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Stop()
    {
        direction = Vector3.zero;
        //angle = Vector2.zero;
    }

    public void Initialize()
    {
        Stop();

        if (rigid)
        {
            rigid.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            rigid.velocity = Vector3.zero;
        }
    }

    public void Move(float _x, float _y)
    {
        direction.x = _x * speed;
        direction.z = _y * speed; //3D에서는 z가 y로 취급

        // 입력값이 있을때 : 이동 방향에 따라 회전할 각도 설정
        if (!(_x == 0 && _y == 0))
        {
            turnTextureAngle = Vector3.zero;

            // 이동 방향 체크
            if (_x > 0) // 우
                turnTextureAngle.x = 1f;
            if (_x < 0) // 좌
                turnTextureAngle.x = -1f;
            if (_y > 0) // 상
                turnTextureAngle.z = 1f;
            if (_y < 0) // 하
                turnTextureAngle.z = -1f;

            // 회전
            characterMesh.forward = turnTextureAngle;
        }
    }

    public IEnumerator Jump()
    {
        //Vector3.up > new Vector3(0, 1, 0)
        //Vector3.down > new Vector3(0, -1, 0)
        float t = .0f;
        isJump = true;

        while (t < 1) 
        {
            // 보간법. 알려진 데이터 지점의 고립점 내에서 새로운 데이터 지점을 구성하는 방식
            t = Mathf.Clamp01(t + Time.deltaTime * speed); // 0과 1사이의 값을 리턴
            // 개체를 이동시킴
            characterMesh.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, t);
            yield return null; // return 뒤에는 조건이 붙는다 + 조건
        }

        while (t > 0)
        {
            // 현재 위치 좌표 확인 및 로그 출력
            t = Mathf.Clamp01(t - Time.deltaTime * speed);
            // 개체를 이동시킴
            characterMesh.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, t);

            yield return null; // return 뒤에는 조건이 붙는다 + 조건
        }

        isJump = false;
    }



    public void Fly(float value)
    {
        
    }

    public void Swim(float value)
    {

    }

    public void Eat(float value)
    {

    }


    // 물리 업데이트를 수행하는 메서드
    void FixedUpdate()
    {
        // 원숭이 바닥에서 시작하게끔
        RaycastHit hit;
        float raycastDistance = 0.1f; // Raycast distance from character to ground

        if (isStart)
        {
            // Cast a ray downwards5
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
            {
                // Move the character to the ground
                transform.position = hit.point + new Vector3(0, 0.1f, 0);
            }
        }
        rigid.velocity = direction;
    }

    public bool GetIsJump() { return isJump; }
} // class PlayerMovement
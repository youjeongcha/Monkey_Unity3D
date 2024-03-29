using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // InputAction�� Behavior�� 


[RequireComponent(typeof(Rigidbody))]
public class CharacterMotion : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] Transform characterMesh; // Mesh����
    [SerializeField] float speed = 4.0f;

    //ĳ���� ȸ��
    private Vector3 turnTextureAngle; // Mesh ���� ����

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
        direction.z = _y * speed; //3D������ z�� y�� ���

        // �Է°��� ������ : �̵� ���⿡ ���� ȸ���� ���� ����
        if (!(_x == 0 && _y == 0))
        {
            turnTextureAngle = Vector3.zero;

            // �̵� ���� üũ
            if (_x > 0) // ��
                turnTextureAngle.x = 1f;
            if (_x < 0) // ��
                turnTextureAngle.x = -1f;
            if (_y > 0) // ��
                turnTextureAngle.z = 1f;
            if (_y < 0) // ��
                turnTextureAngle.z = -1f;

            // ȸ��
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
            // ������. �˷��� ������ ������ ���� ������ ���ο� ������ ������ �����ϴ� ���
            t = Mathf.Clamp01(t + Time.deltaTime * speed); // 0�� 1������ ���� ����
            // ��ü�� �̵���Ŵ
            characterMesh.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, t);
            yield return null; // return �ڿ��� ������ �ٴ´� + ����
        }

        while (t > 0)
        {
            // ���� ��ġ ��ǥ Ȯ�� �� �α� ���
            t = Mathf.Clamp01(t - Time.deltaTime * speed);
            // ��ü�� �̵���Ŵ
            characterMesh.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, t);

            yield return null; // return �ڿ��� ������ �ٴ´� + ����
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


    // ���� ������Ʈ�� �����ϴ� �޼���
    void FixedUpdate()
    {
        // ������ �ٴڿ��� �����ϰԲ�
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
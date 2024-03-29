using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Function")]
    [SerializeField] CharacterMotion movement; // Inspector â���� GameObject ����.
    [SerializeField] CharacterAnimation playerAnim;
    [SerializeField] Transform rotationObj; // Mesh ����

    /*//ĳ���� ȸ��
    private Vector3 turnTexture;*/


    public void Initialize()
    {
        /*turnTexture = Vector3.zero;*/
        movement.Initialize();
        //anim.Initialize();
    }


    // "Move" Actions�� �ش��ϴ� Ű �Է� �� �ڵ����� ȣ���.
    /* w,a,s,d,ȭ��ǥ�� Move �׼ǿ� �Է�Ű�� �����س��⶧����
    �ش� Ű���� �����ٸ� �� OnMove�� ȣ�� */
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        movement.Move(value.x, value.y * 2);
        playerAnim.Move();


        if (value == Vector2.zero)
            playerAnim.Idle();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // space Ű�� ������ �� �����ϵ��� �Ǿ��ִ�.
        float value = context.ReadValue<float>();

        // Action Type�� "Button"�� ��� Ű�� ���ȴ��� üũ
        if (context.performed && (!movement.GetIsJump())) // �Է� �׼��� �߻�
        {
            playerAnim.Jump();
            StartCoroutine(movement.Jump());
        }
    }

    /*
    public void OnFly(InputAction.CallbackContext context)
    {

    }
    */

    public void OnEat(InputAction.CallbackContext context)
    {

    }
}

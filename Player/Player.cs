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


    public void Initialize()
    {
        movement.Initialize();
        playerAnim.Initialize();
    }


    // "Move" Actions�� �ش��ϴ� Ű �Է� �� �ڵ����� ȣ���.
    /* w,a,s,d,ȭ��ǥ�� Move �׼ǿ� �Է�Ű�� �����س��⶧����
    �ش� Ű���� �����ٸ� �� OnMove�� ȣ�� */
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        playerAnim.Move();
        movement.Move(value.x, value.y * 2);

        if (value == Vector2.zero)
            playerAnim.Idle();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        // space Ű�� ������ �� �����ϵ��� �Ǿ��ִ�.
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



/*ȣ�� �ֱ�:
FixedUpdate(): ���� ������ �����ϴ� ������ �ð� ���ݸ��� ȣ��˴ϴ�. �⺻������ 0.02��(1�ʴ� 50ȸ)�� �����Ǿ� ������, �� ���� ������ �� �ֽ��ϴ�. �� �޼���� �������� ���(��: �̵�, �浹 �˻� ��)�� ���˴ϴ�.
Update(): �� �����Ӹ��� ȣ��Ǹ�, ���� �������� �߻��ϱ� ���� ���� ������ ������Ʈ�ϴ� �� ���˴ϴ�.

�ұ�Ģ�� ȣ��:
FixedUpdate(): ������ �ֱ�� ȣ��ǹǷ� �ұ�Ģ�� �ð� �������� ȣ����� �ʽ��ϴ�. ���� ���� �ùķ��̼� �� ���� ��� ���� ������Ʈ�� �̵� �� �浹 ó���� �̻����Դϴ�.
Update(): �� �����Ӹ��� ȣ��Ǹ�, �ұ�Ģ�� �ð� �������� ȣ��� �� �ֽ��ϴ�. ���� �ð��� ���� ������ �ʴ� �����̳� ����� �Է� ó���� ���� �͵鿡 �����մϴ�.
*/
    public void OnEat(InputAction.CallbackContext context)
    {

    }
}

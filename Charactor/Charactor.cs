using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// �����Ϳ� ���õ� ������� Character ��ũ��Ʈ
// �Է� �� ��Ʈ�ѿ� ���� ������� Controller ��ũ��Ʈ�� ���� �и�




public class Charactor : MonoBehaviour
{
    [Header("Function")]
    [SerializeField] CharacterMotion movement; // Inspector â���� GameObject ����.
    [SerializeField] CharacterAnimation playerAnim;
    [SerializeField] Transform rotationObj;

    [Header("Movement")]
   //[SerializeField] protected float direction; // ����
    [SerializeField] protected float moveSpeed; // �ӵ�
    [SerializeField] protected float idleSec; // idle �ð�

    //ĳ���� ȸ��
    private Vector3 turnTexture;

    // ���� ��ġ�� ���� ������
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private float radius = 20f;

    //idle ����
    private float idleSecCnt = 0f;

    private void Start()
    {
        //���� ��ġ
        startPosition = transform.localPosition;
    }

    public void Initialize()
    {
        turnTexture = Vector3.zero;
        movement.Initialize();

        //anim.Initialize();
    }


    // �� ���� ������ ��ġ ��ȯ
    private Vector3 GetRandomPointInCircle()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(5f, 10f);
        Vector3 randomPoint = startPosition + new Vector3(Mathf.Cos(angle) * distance, 0f, Mathf.Sin(angle) * distance);
        return randomPoint;
    }

    // �ȱ� �ڷ�ƾ
    private IEnumerator WalkToTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            playerAnim.Move();
            yield return null;
        }

        /*// �̵��� �Ϸ�� �� idle ���·� ����
        playerAnim.Idle();*/
    }


    /*
     * ó�� ���� ��ġ ���� ������ ������ 20 ����������,
     * Move �Լ��� ����ɶ����� 5~10������ �Ÿ��� �������� �̵��ϴ� �ڵ�
     */
    public void Move()
    {
        if (idleSecCnt < idleSec) // �̵��� �Ϸ�� �� idle ���·� ����
            playerAnim.Idle();
        else
        {
            idleSecCnt = 0f;

            currentPosition = GetRandomPointInCircle();
            transform.localPosition = currentPosition;

            // ��ǥ ��ġ ����
            Vector3 targetPosition = GetRandomPointInCircle();

            // �̵� ���� ���
            Vector3 direction = (targetPosition - transform.localPosition).normalized;
            turnTexture = Vector3.zero;

            // ĳ������ ������ �̵��ϴ� �������� ����
            rotationObj.forward = direction; // �޽� ȸ���Ͼ� �ɾ�� ���� ����
            playerAnim.Move(); // �ִϸ��̼� 
            movement.Move(moveSpeed, moveSpeed * 2); // ���� �̵�

            // �ȴ� �ӵ��� �̵�
            //StartCoroutine(WalkToTarget(targetPosition));
        }
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

    public void Update()
    {
        Move();
    }
}

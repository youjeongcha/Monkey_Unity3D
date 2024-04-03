using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [Header("Function")]
    [SerializeField] CharacterMotion movement; // Inspector â���� GameObject ����.
    [SerializeField] CharacterAnimation playerAnim;
    [SerializeField] Transform rotationObj;
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 1.4f; // �ӵ�
    [SerializeField] protected float idleSec; // idle �ð�

    private enum State
    {
        Idle,
        Move,
        Jump,

    }

    // FSM
    private FSM fsm;
    private State currentstate;


    // ���� ��ġ�� ���� ������
    private Vector3 startPosition = Vector3.zero; // NPC ��ġ ������ ���� �߽�������. �� �ȿ��� ������ �Ÿ��� �̵��Ѵ�.
    private float mathSqure = 0.4f; // �Ÿ�Ȯ�� : �������ϱ� ���� �� ���ϱ� ���� ��(0.2f) * 0.2f �Ÿ� ���� 
    private Vector3 vDistacne; // ��ǥ������ �������� : �Ÿ�����


    private bool isMove = false;

    //idle ����
    private float idleSecCnt = 0f;

    private void Start()
    {
        Initialize();
        rb = GetComponent<Rigidbody>();
        fsm = new FSM(new IdleState(this));
        currentstate = State.Idle;
    }

    public void Initialize()
    {
        movement.Initialize();
        //���� ��ġ
        startPosition = transform.position;
        idleSecCnt = 0f;
        //anim.Initialize();
    }


    // �� ���� ������ ��ġ ��ȯ
    private Vector3 GetRandomPointInCircle()
    {
        // angle: 0���� 2��(�������� �� ��)������ ������ ������ �����մϴ�. �̸� ���� �� ���� ������ ��ġ�� ����
        float angle = Random.Range(0f, Mathf.PI * 2f);
        // ���� �߽ɿ��� ������ �������� �Ÿ��� �����մϴ�. �� �Ÿ��� 5���� 10 ������ ������ ������ �����˴ϴ�.
        float distance = Random.Range(5f, 10f);
        // Mathf.Cos(angle)�� Mathf.Sin(angle)�� ����Ͽ� ������ ���� �ﰢ �Լ� ��
        // �� ������ ���� �߽ɿ��������� x�� z �� ���������� �Ÿ�
        Vector3 randomPoint = startPosition + new Vector3(Mathf.Cos(angle) * distance, 0f, Mathf.Sin(angle) * distance);
        return randomPoint;
    }


    // �ȱ� �ڷ�ƾ : NPC ��ġ ������ ���� �߽�������. �� �ȿ��� ������ �Ÿ��� �̵�
    private IEnumerator WalkToTarget(Vector3 targetPosition)
    {
        isMove = true;

        //����? : �ۼ�Ʈ ���

        //Vector3 v = transform.position - targetPosition;
        Vector3 pos = transform.position;
        

        // ���� ��ġ�� ��ǥ ��ġ�� 0.2f���� �ָ� ������ ������ �̵�
        do
        {
            vDistacne = targetPosition - pos;


            // ���� ��ġ���� ��ǥ ��ġ������ ���� ���� ���
            Vector3 direction = vDistacne.normalized;

            // ��ǥ �������� 1 * �ӵ��� �̵�
            pos += direction * moveSpeed * Time.deltaTime;

            //transform.position = pos;
            rb.position = pos;
            // �̵��� �Ÿ� ���
            //float distanceRemaining = Vector3.Distance(transform.position, targetPosition);
            /*
                        // ���� �Ÿ��� �̵��� �Ÿ����� ������ ���� �Ÿ���ŭ�� �̵�
                        if (distanceRemaining < moveSpeed * Time.deltaTime)
                        {
                            transform.position = targetPosition;
                        }*/
            yield return null;
        } while (vDistacne.sqrMagnitude > mathSqure);

        isMove = false;
    }


    Vector3 targetPosition = Vector3.zero;

    /*
     * ó�� ���� ��ġ ���� ������ ������ 20 ����������,
     * Move �Լ��� ����ɶ����� 5~10������ �Ÿ��� �������� �̵��ϴ� �ڵ�
     */
    public void Move()
    {
        idleSecCnt += Time.deltaTime;


        if (idleSecCnt < idleSec) // �̵��� �Ϸ�� �� idle ���·� ����
            playerAnim.Idle();
        else
        {
            idleSecCnt = 0f;

            // ��ǥ ��ġ ����
            /*Vector3*/ targetPosition = GetRandomPointInCircle();

            // �̵� ���� ���
            Vector3 direction = (targetPosition - transform.position).normalized;
            //turnTexture = Vector3.zero;

            // ĳ������ ������ �̵��ϴ� �������� ����
            rotationObj.forward = direction; // �޽� ȸ���Ͼ� �ɾ�� ���� ����
            playerAnim.Move(); // �ִϸ��̼� 
            //movement.Move(moveSpeed, moveSpeed * 2); // ���� �̵�

            // �ȴ� �ӵ��� �̵�
            StartCoroutine(WalkToTarget(targetPosition));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPosition, 5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPosition, 10f);
        Gizmos.DrawSphere(targetPosition, 0.5f);
    }

    public void Jump(InputAction.CallbackContext context)
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

    public void Eat(InputAction.CallbackContext context)
    {

    }


    private void FSMUpdate()
    {
        switch (currentstate)
        {
            case State.Idle:
            {
                break;
            }
            case State.Move:
            {
                break;
            }
            case State.Jump:
            {
                break;
            }
        }

        fsm.UpdateState();
    }

    private void ChangeState(State nextState)
    {
        currentstate = nextState;

        switch (currentstate)
        {
            case State.Idle:
                fsm.ChangeState(new IdleState(this));
                break;
            case State.Move:
                fsm.ChangeState(new MoveState(this));
                break;
            case State.Jump:
                //fsm.ChangeState(new (this));
                break;
        }
    }


    /*ȣ�� �ֱ�:
    FixedUpdate(): ���� ������ �����ϴ� ������ �ð� ���ݸ��� ȣ��˴ϴ�. �⺻������ 0.02��(1�ʴ� 50ȸ)�� �����Ǿ� ������, �� ���� ������ �� �ֽ��ϴ�. �� �޼���� �������� ���(��: �̵�, �浹 �˻� ��)�� ���˴ϴ�.
    Update(): �� �����Ӹ��� ȣ��Ǹ�, ���� �������� �߻��ϱ� ���� ���� ������ ������Ʈ�ϴ� �� ���˴ϴ�.

    �ұ�Ģ�� ȣ��:
    FixedUpdate(): ������ �ֱ�� ȣ��ǹǷ� �ұ�Ģ�� �ð� �������� ȣ����� �ʽ��ϴ�. ���� ���� �ùķ��̼� �� ���� ��� ���� ������Ʈ�� �̵� �� �浹 ó���� �̻����Դϴ�.
    Update(): �� �����Ӹ��� ȣ��Ǹ�, �ұ�Ģ�� �ð� �������� ȣ��� �� �ֽ��ϴ�. ���� �ð��� ���� ������ �ʴ� �����̳� ����� �Է� ó���� ���� �͵鿡 �����մϴ�.
    */
    void FixedUpdate()
    {
        FSMUpdate();
        //if (!isMove)
        //    Move();
    }
}

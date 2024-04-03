using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [Header("Function")]
    [SerializeField] CharacterMotion movement; // Inspector 창에서 GameObject 연결.
    [SerializeField] CharacterAnimation playerAnim;
    [SerializeField] Transform rotationObj;
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 1.4f; // 속도
    [SerializeField] protected float idleSec; // idle 시간

    private enum State
    {
        Idle,
        Move,
        Jump,

    }

    // FSM
    private FSM fsm;
    private State currentstate;


    // 시작 위치와 원의 반지름
    private Vector3 startPosition = Vector3.zero; // NPC 배치 구역을 원의 중심점으로. 원 안에서 랜덤한 거리로 이동한다.
    private float mathSqure = 0.4f; // 거리확인 : 제곱근하기 전의 값 구하기 전의 값(0.2f) * 0.2f 거리 오차 
    private Vector3 vDistacne; // 목표지점과 현재지점 : 거리차이


    private bool isMove = false;

    //idle 조정
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
        //시작 위치
        startPosition = transform.position;
        idleSecCnt = 0f;
        //anim.Initialize();
    }


    // 원 안의 랜덤한 위치 반환
    private Vector3 GetRandomPointInCircle()
    {
        // angle: 0부터 2π(원주율의 두 배)까지의 랜덤한 각도를 생성합니다. 이를 통해 원 안의 임의의 위치를 선택
        float angle = Random.Range(0f, Mathf.PI * 2f);
        // 원의 중심에서 생성된 점까지의 거리를 지정합니다. 이 거리는 5부터 10 사이의 랜덤한 값으로 설정됩니다.
        float distance = Random.Range(5f, 10f);
        // Mathf.Cos(angle)와 Mathf.Sin(angle)를 사용하여 각도에 대한 삼각 함수 값
        // 이 값들은 원의 중심에서부터의 x와 z 축 방향으로의 거리
        Vector3 randomPoint = startPosition + new Vector3(Mathf.Cos(angle) * distance, 0f, Mathf.Sin(angle) * distance);
        return randomPoint;
    }


    // 걷기 코루틴 : NPC 배치 구역을 원의 중심점으로. 원 안에서 랜덤한 거리로 이동
    private IEnumerator WalkToTarget(Vector3 targetPosition)
    {
        isMove = true;

        //러프? : 퍼센트 계산

        //Vector3 v = transform.position - targetPosition;
        Vector3 pos = transform.position;
        

        // 현재 위치와 목표 위치가 0.2f보다 멀리 떨어져 있으면 이동
        do
        {
            vDistacne = targetPosition - pos;


            // 현재 위치에서 목표 위치까지의 방향 벡터 계산
            Vector3 direction = vDistacne.normalized;

            // 목표 지점까지 1 * 속도씩 이동
            pos += direction * moveSpeed * Time.deltaTime;

            //transform.position = pos;
            rb.position = pos;
            // 이동할 거리 계산
            //float distanceRemaining = Vector3.Distance(transform.position, targetPosition);
            /*
                        // 남은 거리가 이동할 거리보다 작으면 남은 거리만큼만 이동
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
     * 처음 시작 위치 기준 원으로 반지름 20 범위내에서,
     * Move 함수가 실행될때마다 5~10까지의 거리를 랜덤으로 이동하는 코드
     */
    public void Move()
    {
        idleSecCnt += Time.deltaTime;


        if (idleSecCnt < idleSec) // 이동이 완료된 후 idle 상태로 변경
            playerAnim.Idle();
        else
        {
            idleSecCnt = 0f;

            // 목표 위치 설정
            /*Vector3*/ targetPosition = GetRandomPointInCircle();

            // 이동 방향 계산
            Vector3 direction = (targetPosition - transform.position).normalized;
            //turnTexture = Vector3.zero;

            // 캐릭터의 방향을 이동하는 방향으로 설정
            rotationObj.forward = direction; // 메시 회전하야 걸어가는 방향 보게
            playerAnim.Move(); // 애니메이션 
            //movement.Move(moveSpeed, moveSpeed * 2); // 실제 이동

            // 걷는 속도로 이동
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
        // space 키를 눌렀을 때 점프하도록 되어있다.
        float value = context.ReadValue<float>();

        // Action Type이 "Button"일 경우 키가 눌렸는지 체크
        if (context.performed && (!movement.GetIsJump())) // 입력 액션이 발생
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


    /*호출 주기:
    FixedUpdate(): 물리 엔진이 동작하는 고정된 시간 간격마다 호출됩니다. 기본적으로 0.02초(1초당 50회)로 설정되어 있지만, 이 값은 변경할 수 있습니다. 이 메서드는 물리적인 계산(예: 이동, 충돌 검사 등)에 사용됩니다.
    Update(): 매 프레임마다 호출되며, 실제 렌더링이 발생하기 전에 게임 로직을 업데이트하는 데 사용됩니다.

    불규칙한 호출:
    FixedUpdate(): 고정된 주기로 호출되므로 불규칙한 시간 간격으로 호출되지 않습니다. 따라서 물리 시뮬레이션 및 물리 기반 게임 오브젝트의 이동 및 충돌 처리에 이상적입니다.
    Update(): 매 프레임마다 호출되며, 불규칙한 시간 간격으로 호출될 수 있습니다. 따라서 시간에 따라 변하지 않는 로직이나 사용자 입력 처리와 같은 것들에 적합합니다.
    */
    void FixedUpdate()
    {
        FSMUpdate();
        //if (!isMove)
        //    Move();
    }
}

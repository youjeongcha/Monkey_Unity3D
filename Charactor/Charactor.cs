using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// 데이터와 관련된 내용들은 Character 스크립트
// 입력 및 컨트롤에 관한 내용들은 Controller 스크립트에 각각 분리




public class Charactor : MonoBehaviour
{
    [Header("Function")]
    [SerializeField] CharacterMotion movement; // Inspector 창에서 GameObject 연결.
    [SerializeField] CharacterAnimation playerAnim;
    [SerializeField] Transform rotationObj;

    [Header("Movement")]
   //[SerializeField] protected float direction; // 방향
    [SerializeField] protected float moveSpeed; // 속도
    [SerializeField] protected float idleSec; // idle 시간

    //캐릭터 회전
    private Vector3 turnTexture;

    // 시작 위치와 원의 반지름
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private float radius = 20f;

    //idle 조정
    private float idleSecCnt = 0f;

    private void Start()
    {
        //시작 위치
        startPosition = transform.localPosition;
    }

    public void Initialize()
    {
        turnTexture = Vector3.zero;
        movement.Initialize();

        //anim.Initialize();
    }


    // 원 안의 랜덤한 위치 반환
    private Vector3 GetRandomPointInCircle()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(5f, 10f);
        Vector3 randomPoint = startPosition + new Vector3(Mathf.Cos(angle) * distance, 0f, Mathf.Sin(angle) * distance);
        return randomPoint;
    }

    // 걷기 코루틴
    private IEnumerator WalkToTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            playerAnim.Move();
            yield return null;
        }

        /*// 이동이 완료된 후 idle 상태로 변경
        playerAnim.Idle();*/
    }


    /*
     * 처음 시작 위치 기준 원으로 반지름 20 범위내에서,
     * Move 함수가 실행될때마다 5~10까지의 거리를 랜덤으로 이동하는 코드
     */
    public void Move()
    {
        if (idleSecCnt < idleSec) // 이동이 완료된 후 idle 상태로 변경
            playerAnim.Idle();
        else
        {
            idleSecCnt = 0f;

            currentPosition = GetRandomPointInCircle();
            transform.localPosition = currentPosition;

            // 목표 위치 설정
            Vector3 targetPosition = GetRandomPointInCircle();

            // 이동 방향 계산
            Vector3 direction = (targetPosition - transform.localPosition).normalized;
            turnTexture = Vector3.zero;

            // 캐릭터의 방향을 이동하는 방향으로 설정
            rotationObj.forward = direction; // 메시 회전하야 걸어가는 방향 보게
            playerAnim.Move(); // 애니메이션 
            movement.Move(moveSpeed, moveSpeed * 2); // 실제 이동

            // 걷는 속도로 이동
            //StartCoroutine(WalkToTarget(targetPosition));
        }
    }

    public void OnJump(InputAction.CallbackContext context)
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

    public void OnEat(InputAction.CallbackContext context)
    {

    }

    public void Update()
    {
        Move();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Function")]
    [SerializeField] CharacterMotion movement; // Inspector 창에서 GameObject 연결.
    [SerializeField] CharacterAnimation playerAnim;
    [SerializeField] Transform rotationObj; // Mesh 기준

    /*//캐릭터 회전
    private Vector3 turnTexture;*/


    public void Initialize()
    {
        /*turnTexture = Vector3.zero;*/
        movement.Initialize();
        //anim.Initialize();
    }


    // "Move" Actions에 해당하는 키 입력 시 자동으로 호출됨.
    /* w,a,s,d,화살표를 Move 액션에 입력키로 설정해놨기때문에
    해당 키들이 눌린다면 위 OnMove가 호출 */
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
}

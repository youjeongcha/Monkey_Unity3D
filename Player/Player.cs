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


    public void Initialize()
    {
        movement.Initialize();
        playerAnim.Initialize();
    }


    // "Move" Actions에 해당하는 키 입력 시 자동으로 호출됨.
    /* w,a,s,d,화살표를 Move 액션에 입력키로 설정해놨기때문에
    해당 키들이 눌린다면 위 OnMove가 호출 */
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
        // space 키를 눌렀을 때 점프하도록 되어있다.
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



/*호출 주기:
FixedUpdate(): 물리 엔진이 동작하는 고정된 시간 간격마다 호출됩니다. 기본적으로 0.02초(1초당 50회)로 설정되어 있지만, 이 값은 변경할 수 있습니다. 이 메서드는 물리적인 계산(예: 이동, 충돌 검사 등)에 사용됩니다.
Update(): 매 프레임마다 호출되며, 실제 렌더링이 발생하기 전에 게임 로직을 업데이트하는 데 사용됩니다.

불규칙한 호출:
FixedUpdate(): 고정된 주기로 호출되므로 불규칙한 시간 간격으로 호출되지 않습니다. 따라서 물리 시뮬레이션 및 물리 기반 게임 오브젝트의 이동 및 충돌 처리에 이상적입니다.
Update(): 매 프레임마다 호출되며, 불규칙한 시간 간격으로 호출될 수 있습니다. 따라서 시간에 따라 변하지 않는 로직이나 사용자 입력 처리와 같은 것들에 적합합니다.
*/
    public void OnEat(InputAction.CallbackContext context)
    {

    }
}

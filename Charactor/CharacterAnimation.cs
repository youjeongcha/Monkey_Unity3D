using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]Animator anim;


    // Update is called once per frame
    public void Initialize()
    {
    }

    public void Idle()
    {
        if (anim)
            anim.SetBool("isMove", false);
    }

    public void Move()
    {
        if (anim)
            anim.SetBool("isMove", true);
    }

    public void Jump()
    {
        if (anim)
            anim.SetTrigger("isJump"); // Jump Trigger true ����
    }

    public void Fly()
    {
        if (anim)
            anim.SetTrigger("isFly"); // ������ ���̷� �ö󰡰��ҰŶ� trigger ���
    }

    public void Swim()
    {
        if (anim)
            anim.SetBool("isSwim", true);
    }
}
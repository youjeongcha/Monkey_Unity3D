using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Charactor
{
    [Header("Aid")]
    [SerializeField] protected int favor; // ������ ���� ������

    public void Initalize()
    {
        this.favor = 0;
    }
}
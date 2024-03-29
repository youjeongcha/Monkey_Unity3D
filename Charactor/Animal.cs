using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Charactor
{
    [Header("Aid")]
    [SerializeField] protected int favor; // 도움을 줄지 안줄지

    public void Initalize()
    {
        this.favor = 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseBehavior : MonoBehaviour
{
    public EnemyBase EnemyBase { get; private set; }

    private void Awake() 
    {
        EnemyBase = gameObject.GetComponent<EnemyBase>();
    }
}

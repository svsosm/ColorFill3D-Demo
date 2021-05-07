using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    public Vector3 startPos;
    public float movementLength;
    public float speed;
    public bool isHorizontalMovement;

}

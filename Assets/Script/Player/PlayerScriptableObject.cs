using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player", order = 1)]
public class PlayerScriptableObject : ScriptableObject
{
    public Vector3 startPos;
    public Vector3 targetPosForLevelTransition;
    public float speed;
}

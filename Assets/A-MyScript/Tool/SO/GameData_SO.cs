using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameData",menuName ="Other/GameData")]
public class GameData_SO : ScriptableObject
{
    public int OpenBulletBoxIncluded, ClosedBulletBoxIncluded;
    public float RotateSpeed;
}

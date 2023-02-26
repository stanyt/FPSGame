using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Other/CharacterData")]

public class CharacterData_SO : ScriptableObject
{
    public int RifleAmmunitionNumber, PistolAmmunitionNumber;
    public float MoveSpeed,JumpForce;
}

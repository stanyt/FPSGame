using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("ǹ֧��ֵ����")]
    public Weapon_SO TemplateSO,DataSO;
    public Transform BulletShellOutPoint, BulletOutPoint,Magazine;
    public bool IsGot;
    private void Awake()
    {
        if (TemplateSO)
        {
            DataSO = Instantiate(TemplateSO);
        }
    }

    bool IsEmpty()
    {
        return DataSO.CurrentBulletNumInGun <= 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunTypes
{
    Rifle,Pistol,Sniper,ShotGun//��ǹ����ǹ���ѻ�ǹ������ǹ
}[CreateAssetMenu(fileName ="Weapon",menuName ="Other/Weapon")]
public class Weapon_SO : Item_SO
{
    public int BulletHoldMax;
    public int CurrentBulletNumInGun;
    public float AmmunitionDamage,ShootRange,OpeningAimTime, /*RateOfFireROF,*/ Recoil, AimingViewRate;
    public GunTypes Type;
    
}

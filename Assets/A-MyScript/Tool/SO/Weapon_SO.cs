using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunTypes
{
    Rifle,Pistol,Sniper,ShotGun//²½Ç¹£¬ÊÖÇ¹£¬¾Ñ»÷Ç¹£¬ö±µ¯Ç¹
}[CreateAssetMenu(fileName ="Weapon",menuName ="Other/Weapon")]
public class Weapon_SO : Item_SO
{
    public int BulletHoldMax;
    public int CurrentBulletNumInGun;
    public float AmmunitionDamage,ShootRange,OpeningAimTime, /*RateOfFireROF,*/ Recoil, AimingViewRate;
    public GunTypes Type;
    
}

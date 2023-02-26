
using UnityEngine;

public class AnimController : Singlton<AnimController>
{
    public GameObject RifleBulletShellPrefab, PistolBulletShellPrefab, RifleBulletPrefab, PistolBulletPrefab,
        SniperBulletShellPrefab, SniperBulletPrefab;
    public Transform LeftHand;
    private Transform LeftHandGunSpot;
    public AnimatorOverrideController RifleAnimator, PistolAnimator;
    private  Animator CharacterAnimator;
    private  bool flag;
    private  Vector3 MagazineLocalOriPos,MagazineLocalScale;
    private  Quaternion MagazineLocalQu,Oriquaternion;
    private void Start()
    {
        LeftHandGunSpot = LeftHand.GetChild(0);
        CharacterAnimator = GetComponent<Animator>();
        SwitchCharacterAnimator();
        flag = false;
        Oriquaternion = transform.localRotation;
    }
    private void Update()
    {
        transform.localRotation = Oriquaternion;
    }
    public void BulletAndItsShellOut()
    {
        GameObject BulletTemp;
        GameObject Muzzle = Instantiate(Inventory.Instance.Muzzles[Random.Range(0, 4)], Inventory.Instance.CurrentWeapon.BulletOutPoint.position, Quaternion.identity, Inventory.Instance.CurrentWeapon.transform);

        switch (Inventory.Instance.CurrentWeapon.DataSO.Type)
        {
            case GunTypes.Pistol:
                Destroy(Instantiate(PistolBulletShellPrefab, Inventory.Instance.CurrentWeapon.BulletShellOutPoint.position, Quaternion.identity), 2f);
                BulletTemp = Instantiate(PistolBulletPrefab, Inventory.Instance.CurrentWeapon.BulletOutPoint.position, Quaternion.identity, transform.parent);
                Muzzle.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                break;
            case GunTypes.Sniper:
                Destroy(Instantiate(SniperBulletShellPrefab, Inventory.Instance.CurrentWeapon.BulletShellOutPoint.position, Quaternion.identity), 2f);
                BulletTemp = Instantiate(SniperBulletPrefab, Inventory.Instance.CurrentWeapon.BulletOutPoint.position, Quaternion.identity, transform.parent);
                Muzzle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                break;
            default:
                Destroy(Instantiate(RifleBulletShellPrefab, Inventory.Instance.CurrentWeapon.BulletShellOutPoint.position, Quaternion.identity), 2f);
                BulletTemp = Instantiate(RifleBulletPrefab, Inventory.Instance.CurrentWeapon.BulletOutPoint.position, Quaternion.identity, transform.parent);
                Muzzle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); 
                break;
        }
        Muzzle.transform.forward = transform.forward;
        BulletTemp.transform.rotation = Inventory.Instance.CurrentWeapon.BulletOutPoint.rotation;
        Destroy(BulletTemp, 2f);
        if (GameManager.Instance.raycastHit.collider)
        {
            BulletTemp.GetComponent<Rigidbody>().AddForce((GameManager.Instance.raycastHit.point - BulletTemp.transform.position).normalized * Inventory.Instance.CurrentWeapon.DataSO.ShootRange, ForceMode.Impulse);
        }
        else BulletTemp.GetComponent<Rigidbody>().AddForce(BulletTemp.transform.parent.forward * Inventory.Instance.CurrentWeapon.DataSO.ShootRange, ForceMode.Impulse);

        --Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun;
        Destroy(Muzzle,0.1f);
        transform.parent.Rotate(Vector3.Cross(transform.parent.forward, Vector3.up),
        Inventory.Instance.CurrentWeapon.DataSO.Recoil * GameManager.Instance.GameData.RotateSpeed,Space.World);
        
    }
    void SwitchCharacterAnimator()
    {
        switch (Inventory.Instance.CurrentWeapon.DataSO.Type)
        {
            case GunTypes.Pistol:
                CharacterAnimator.runtimeAnimatorController = PistolAnimator;
                break;
            default:
                CharacterAnimator.runtimeAnimatorController = RifleAnimator;
                break;
        }
    }
    public void SwitchWeapon()
    {
        Inventory.Instance.ClearAllHoldWeapons();
        Inventory.Instance.CurrentWeapon.gameObject.SetActive(true);
        SwitchCharacterAnimator();
        Inventory.Instance.CurrentWeapon.IsGot=true;
        GetComponent<Animator>().SetBool("Holstered", false);
    }
    public void ReloadingChangeParent()
    {
        if (!flag)
        {
            flag = true;
            if (Inventory.Instance.CurrentWeapon.TemplateSO.Type == GunTypes.Pistol)
            {
                GameObject NewMagazine = Instantiate(Inventory.Instance.CurrentWeapon.Magazine.gameObject, LeftHandGunSpot.position, Quaternion.identity, LeftHand);
                NewMagazine.transform.localPosition = LeftHandGunSpot.localPosition;
                NewMagazine.transform.localRotation = LeftHandGunSpot.localRotation;
                NewMagazine.transform.localScale = LeftHandGunSpot.localScale;
                Destroy(NewMagazine, 0.5f);
            }
            else
            {
                Inventory.Instance.CurrentWeapon.Magazine.SetParent(LeftHand);
            }
        }
        else
        {
            flag = false;
            Inventory.Instance.CurrentWeapon.Magazine.SetParent(Inventory.Instance.CurrentWeapon.transform);
            Inventory.Instance.CurrentWeapon.Magazine.localPosition = MagazineLocalOriPos;
            Inventory.Instance.CurrentWeapon.Magazine.localRotation = MagazineLocalQu;
            Inventory.Instance.CurrentWeapon.Magazine.localScale = MagazineLocalScale;
        }
    }

    public void GetOriTran()
    {
        MagazineLocalOriPos = Inventory.Instance.CurrentWeapon.Magazine.localPosition;
        MagazineLocalQu = Inventory.Instance.CurrentWeapon.Magazine.localRotation;
        MagazineLocalScale = Inventory.Instance.CurrentWeapon.Magazine.localScale;
        
    }

    public void PlayReloadingAudio(int index)
    {
        AudioManager.Instance.PlayReloadingSound(index);
    }

    public void PlayAimIn(AudioClip aimIn)
    {
       AudioManager.Instance.GunSound.PlayOneShot(aimIn);
    }

    public void AmmoFill()
    {
        if (Inventory.Instance.CurrentWeapon.DataSO.Type == GunTypes.Pistol&& (GameManager.Instance.theCharacter.Data_SO.PistolAmmunitionNumber += Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun - Inventory.Instance.CurrentWeapon.DataSO.BulletHoldMax) < 0)
        {
            Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun += GameManager.Instance.theCharacter.Data_SO.PistolAmmunitionNumber;
            GameManager.Instance.theCharacter.Data_SO.PistolAmmunitionNumber = 0;
        }
        else if(Inventory.Instance.CurrentWeapon.DataSO.Type != GunTypes.Pistol&& (GameManager.Instance.theCharacter.Data_SO.RifleAmmunitionNumber += Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun - Inventory.Instance.CurrentWeapon.DataSO.BulletHoldMax) < 0)
        {
            Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun += GameManager.Instance.theCharacter.Data_SO.RifleAmmunitionNumber;
            GameManager.Instance.theCharacter.Data_SO.RifleAmmunitionNumber = 0;
        }
        Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun = Inventory.Instance.CurrentWeapon.DataSO.BulletHoldMax;
    }

    public void AtTheEndOfReload()
    {
        GameManager.Instance.Reloading = false;
    }

    public void NewMagazine()
    {
        GameObject NewMagazine = Instantiate(Inventory.Instance.CurrentWeapon.Magazine.gameObject, LeftHandGunSpot.position, Quaternion.identity, LeftHand);
        NewMagazine.transform.localPosition = LeftHand.GetChild(1).localPosition;
        NewMagazine.transform.localRotation = LeftHand.GetChild(1).localRotation;

        Destroy(NewMagazine, 0.5f);
        Inventory.Instance.CurrentWeapon.Magazine.SetParent(transform);
    }
}

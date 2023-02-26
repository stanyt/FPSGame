using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator mAnimator;
    private Rigidbody mRigidbody;

    [Header("数值")]
    public CharacterData_SO TemplateDataSO, Data_SO;
    public GameObject Arms;

    private float AimTime,OpeningTime;
    private float NormalView;
    private void Start()
    {
        mAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        AimTime = 0;
        NormalView = Camera.main.fieldOfView;
        GameManager.Instance.GamePaused = false;
        if (TemplateDataSO)
        {
            Data_SO = Instantiate(TemplateDataSO);
        }
    }
    private void Update()
    {
        if (GameManager.Instance.GamePaused) return;
        SwitchAnim();
        Jump();
        Move();
        MouseControl();
    }
    private void SwitchAnim()
    {
        if (!GameManager.Instance.Reloading&&(Input.mouseScrollDelta.y < -0.7f || Input.GetKeyDown(KeyCode.Q)))
        {
            Inventory.Instance.NextWeapon();
            GameManager.Instance.Reloading = false;
            mAnimator.SetBool("Holstered", true);
        }
        else if (!GameManager.Instance.Reloading && Input.mouseScrollDelta.y > 0.7f)
        {

            Inventory.Instance.PrevWeapon();
            mAnimator.SetBool("Holstered", true);
        }
        if (Inventory.Instance.CurrentWeapon.IsGot)
        {
            
            if (Input.GetKeyDown(KeyCode.R)&&!GameManager.Instance.Reloading&&!IsAmmoEmpty()&&
                Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun!=
                Inventory.Instance.CurrentWeapon.DataSO.BulletHoldMax)
            {
                GameManager.Instance.Reloading = true;
                Reload();
            }
            else if (Input.GetMouseButton(1)&& !GameManager.Instance.Reloading)
            {
                mAnimator.SetFloat("Aiming", AimTime += Time.deltaTime);
                mAnimator.SetBool("Aim", true);
                Aim();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun <= 0&&!GameManager.Instance.Reloading)
                {
                    mAnimator.Play("Fire Empty", 2, 0.0f);
                    if(!IsAmmoEmpty()){
                        mAnimator.SetTrigger("ReloadEmpty");
                        GameManager.Instance.Reloading = true;
                    }
                }
                else if(!GameManager.Instance.Reloading){
                    AudioManager.Instance.PlayFireSound();
                    Debug.Log("Fire1:" + Input.GetAxisRaw("Fire1"));
                    mAnimator.Play("Fire", 2, 0.0f);
                }
                UIManager.Instance.FireStarAnimStart();
            }
            else
            {
                EndAim();
            }
            
        }
        mAnimator.SetFloat("Movement", new Vector2(mRigidbody.velocity.x, mRigidbody.velocity.z).sqrMagnitude);
        if (Input.GetKey(KeyCode.LeftShift) && mRigidbody.velocity.sqrMagnitude > 0.1f)
        {
            mAnimator.SetBool("Running", true);
        }
        else
        {
            mAnimator.SetBool("Running", false);
        }
    }
    private void Aim()
    {
        if (OpeningTime <= (1 - Inventory.Instance.CurrentWeapon.DataSO.AimingViewRate))
        {
            OpeningTime += Time.deltaTime * (1 - Inventory.Instance.CurrentWeapon.DataSO.AimingViewRate)
                / Inventory.Instance.CurrentWeapon.DataSO.OpeningAimTime;
            Camera.main.fieldOfView = NormalView * (1 - OpeningTime);
        }
        else if (Inventory.Instance.CurrentWeapon.DataSO.Name.CompareTo("Sniper Rifle Gun")==0)
        {
            UIManager.Instance.ShowSniperAim();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun <= 0
                && !GameManager.Instance.Reloading)
            {
                mAnimator.Play("A_FP_PCH_AR_Aim_Fire_Empty", 2, 0.0f);
                if(!IsAmmoEmpty()){
                    GameManager.Instance.Reloading = true;
                    mAnimator.SetTrigger("ReloadEmpty");
                }
                EndAim();
            }
            else if(!GameManager.Instance.Reloading)
            {
                AudioManager.Instance.PlayFireSound();
                mAnimator.Play("A_FP_PCH_AR_Aim_Fire", 2, 0.0f);
                
            }
            UIManager.Instance.FireStarAnimStart();
        }
    }
    private void EndAim()
    {
        if (AimTime != 0) UIManager.Instance.CloseSniperAim();
        if (Camera.main.fieldOfView < NormalView)
        {
            OpeningTime -= Time.deltaTime * (1 - Inventory.Instance.CurrentWeapon.DataSO.AimingViewRate) / Inventory.Instance.CurrentWeapon.DataSO.OpeningAimTime;
            Camera.main.fieldOfView = NormalView * (1 - OpeningTime);
        }
        else
        {
            OpeningTime = 0;
        }
        AimTime = 0;
        mAnimator.SetFloat("Aiming", AimTime);
        mAnimator.SetBool("Aim", false);

    }
    private void Move()
    {
        float lr= Input.GetAxis("Horizontal"), fb= Input.GetAxis("Vertical");

        if (fb != 0||lr!=0)
        {
            float Speed= Data_SO.MoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Speed *= 2;
                AudioManager.Instance.PlayFootstep(1);
            }
            else
            {
                AudioManager.Instance.PlayFootstep(0);
            }
            mRigidbody.velocity = Speed * (transform.forward * fb + lr * Vector3.Cross(Vector3.up, transform.forward)) + new Vector3(0, mRigidbody.velocity.y, 0);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mRigidbody.velocity += Data_SO.JumpForce * Vector3.up;
            AnimController.Instance.PlayAimIn(AudioManager.Instance.JumpAndLand[0]);
        }
    }
    private void MouseControl()
    {
        float xMouseOffset = Input.GetAxis("Mouse X"), yMouseOffset = Input.GetAxis("Mouse Y");
        if (xMouseOffset != 0 || yMouseOffset != 0)
        {
            //左右旋转整体
            transform.Rotate(Vector3.up, xMouseOffset * GameManager.Instance.GameData.RotateSpeed,Space.World);
            //上下旋转手臂
            transform.GetChild(0).Rotate(Vector3.Cross(transform.GetChild(0).forward, Vector3.up), yMouseOffset * GameManager.Instance.GameData.RotateSpeed, Space.World);
        }
    }
    private void Reload()
    {
        if(Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun>0)
            mAnimator.SetTrigger("Reload");
    }
    private bool IsAmmoEmpty()
    {
        switch (Inventory.Instance.CurrentWeapon.DataSO.Type)
        {
            case GunTypes.Pistol:
                return Data_SO.PistolAmmunitionNumber == 0;
            default: return Data_SO.RifleAmmunitionNumber == 0;
        }
    }
}

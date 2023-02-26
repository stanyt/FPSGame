using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class UIManager : Singlton<UIManager>
{
    private Image AimUI;
    public Image aimUI => AimUI;
    public TextMeshProUGUI HoldAmmoNumText, BulletInGunNumText;
    private Transform BaseUI;
    private Animator UIAnimator;
    void Start()
    {
        AimUI = transform.GetChild(0).GetComponent<Image>();
        UIAnimator = GetComponent<Animator>();
        AimUI.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        BaseUI = transform.GetChild(0).GetChild(1);
        BaseUI.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(ContinueMode);
        BaseUI.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(EndGame);
        BaseUI.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                GameManager.Instance.GamePaused = true;
                Time.timeScale = 0;
                BaseUI.gameObject.SetActive(true);
            }
        }
        if(Inventory.Instance.CurrentWeapon.gameObject.activeInHierarchy)
        {
            BulletInGunNumText.text = Inventory.Instance.CurrentWeapon.DataSO.CurrentBulletNumInGun.ToString();
            HoldAmmoNumText.text = Inventory.Instance.CurrentWeapon.DataSO.Type == GunTypes.Pistol ?
               GameManager.Instance.theCharacter.Data_SO.PistolAmmunitionNumber.ToString() : GameManager.Instance.theCharacter.Data_SO.RifleAmmunitionNumber.ToString();
        }
    }
    public void ShowSniperAim()
    {
        AimUI.enabled = true;
        Inventory.Instance.CurrentWeapon.gameObject.SetActive(false);
        GameManager.Instance.theCharacter.Arms.SetActive(false);
    }
    public void CloseSniperAim()
    {
        Inventory.Instance.CurrentWeapon.gameObject.SetActive(true);
        GameManager.Instance.theCharacter.Arms.SetActive(true);
        AimUI.enabled = false;
    }
    public void FireStarAnimStart()
    {
        UIAnimator.Play("Star", 0, 0.0f);
    }
    void ContinueMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        GameManager.Instance.GamePaused = false;
    }
    void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}

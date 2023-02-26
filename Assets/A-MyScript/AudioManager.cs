
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singlton<AudioManager>
{
    [Header("…˘“Ù¿¥‘¥")]
    public AudioSource FootStep,GunSound;
    [Header("“Ù–ß")]
    public List<AudioClip> Footsteps = new List<AudioClip>();
    public List<AudioClip> JumpAndLand = new List<AudioClip>();
    public List<AudioClip> FireSound = new List<AudioClip>();
    public List<AudioClip> ReloadingSound = new List<AudioClip>();
    private void Start()
    {
        FootStep.PlayOneShot(JumpAndLand[1]);
    }
    private void Update()
    {
        if (GameManager.Instance.GamePaused)
        {
            GunSound.Pause();FootStep.Pause();
        }
        else
        {
            GunSound.UnPause(); FootStep.UnPause();
        }
    }
    public void PlayFootstep(int index)
    {
        if(FootStep.isPlaying) return;
        FootStep.PlayOneShot(Footsteps[index]);
    }
    public void PlayFireSound()
    {
        GunSound.PlayOneShot(FireSound[(int)Inventory.Instance.CurrentWeapon.DataSO.Type]);
    }
    public void PlayReloadingSound(int index)
    {
       GunSound.PlayOneShot(ReloadingSound[index]);
    }
}

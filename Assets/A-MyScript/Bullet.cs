using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public GameObject BlowEffectPrefab,DamageEffectPrefab,DamageNumberPrefab;
    private bool collided;
    public Bullet_SO DataSO;
    private void Start()
    {
        collided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collided)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.collider.CompareTag("Target"))
            {
                //œ‘ æ…À∫¶
                GameObject TempNum = Instantiate(DamageNumberPrefab, collision.GetContact(0).point, Quaternion.identity);
                TempNum.transform.GetChild(0).GetComponent<Text>().text = Inventory.Instance.CurrentWeapon.DataSO.AmmunitionDamage.ToString();
                TempNum.transform.forward= -collision.GetContact(0).normal;
                Destroy(TempNum, 1f);
            }
            Destroy(Instantiate(BlowEffectPrefab, collision.GetContact(0).point, Quaternion.identity), 1f);
            GameObject Temp = Instantiate(DamageEffectPrefab, collision.GetContact(0).point, Quaternion.identity);
            if(DataSO) Temp.transform.GetChild(0).GetComponent<Image>().sprite = DataSO.Bullet_Hole;
            Temp.transform.forward=collision.GetContact(0).normal;
            Temp.transform.position += Temp.transform.forward*0.01f;
            Destroy(Temp, 3.5f);
            collided = true;
        }
    }

}

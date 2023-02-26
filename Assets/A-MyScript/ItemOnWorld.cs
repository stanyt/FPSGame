
using UnityEngine;
using TMPro;

public class ItemOnWorld : MonoBehaviour
{
    public Item_SO ItemTemplateData;
    private Item_SO ItemData;
    private GameObject temp;
    public bool IsSelected;
    private void Start()
    {
        if (ItemTemplateData)
        {
            ItemData = Instantiate(ItemTemplateData);
        }
    }
    void Update()
    {
        if (IsSelected&&GameManager.Instance.currentItemOnWorldSelected!=this)
        {
            IsSelected = false;
        }
        if (IsSelected&&ItemData.Pickable&&IsClosedCharacter())
        {
            if (!temp)
            {
                temp = Instantiate(ItemData.FloatInfo, transform.position, Quaternion.identity, transform);
                temp.GetComponent<Animator>().Play("Float", 0);
                temp.transform.forward = GameManager.Instance.theCharacter.transform.forward;
                temp.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "[E]" + ItemData.Name;
            }
            else
            {
                temp.transform.forward = GameManager.Instance.theCharacter.transform.forward;
                
            }
            if (Input.GetKeyDown(KeyCode.E)){
                AnimController.Instance.PlayAimIn(ItemData.PickAudio);
                switch (ItemData.Name)
                {
                    case "Rifle Ammos":
                        GameManager.Instance.theCharacter.Data_SO.RifleAmmunitionNumber += GameManager.Instance.GameData.ClosedBulletBoxIncluded;
                        break;
                    case "Pistol Ammos":
                        GameManager.Instance.theCharacter.Data_SO.PistolAmmunitionNumber += GameManager.Instance.GameData.ClosedBulletBoxIncluded;
                        break;
                }
            }
        }
        else
        {
            if (temp)
            {
                Destroy(temp);
            }
            IsSelected = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ItemTemplateData.PickableRange);
    }

    bool IsClosedCharacter()
    {
        return Mathf.Abs((GameManager.Instance.theCharacter.transform.position - transform.position).sqrMagnitude) <= ItemData.PickableRange;
    }
}

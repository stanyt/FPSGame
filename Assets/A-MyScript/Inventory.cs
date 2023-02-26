
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singlton<Inventory>
{
    private int IndexOfCurrentHold = -1;
    private List<Weapon> weapons = new List<Weapon>();
    public List<GameObject> Muzzles=new List<GameObject>();
    public Weapon CurrentWeapon => weapons[IndexOfCurrentHold];
    protected new void Awake()
    {
        base.Awake();
        for (int i = 0; i < transform.childCount; ++i)
        {
            weapons.Add(transform.GetChild(i).GetComponent<Weapon>());
            if (weapons[i].IsGot)
            {
                IndexOfCurrentHold = i;
            }
        }
    }
    public void NextWeapon()
    {
        IndexOfCurrentHold = IndexOfCurrentHold + 1 == weapons.Count ? 0 : IndexOfCurrentHold + 1;
    }
    public void PrevWeapon()
    {
        IndexOfCurrentHold = IndexOfCurrentHold - 1 < 0 ? weapons.Count-1 : IndexOfCurrentHold - 1;
    }
    public void ClearAllHoldWeapons()
    {
        foreach (Weapon wea in weapons)
        {
            wea.gameObject.SetActive(false);
            wea.IsGot = false;
        }
    }
}

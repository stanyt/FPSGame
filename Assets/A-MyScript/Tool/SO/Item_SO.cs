using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Other/Item")]
public class Item_SO : ScriptableObject
{
    public string Name;
    public bool Pickable;
    public float PickableRange;
    public GameObject FloatInfo;
    public AudioClip PickAudio;
}

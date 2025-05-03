using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Industry")]
public class Industry : ScriptableObject
{
    public List<ItemList> OriginLists;
    public List<ItemList> ProductLists;
    public bool isAutomation;
}
[System.Serializable]
public class ItemList
{
    public Item Item;
    public int amount;
}
[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{

}
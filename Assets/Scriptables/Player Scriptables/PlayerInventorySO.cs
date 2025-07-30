using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerInventorySO", menuName = "Scriptable Objects/PlayerInventorySO")]
public class PlayerInventorySO : ScriptableObject
{
    public List<Item> items = new List<Item>();

    public int currentItemIndex = 0;
}

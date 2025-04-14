using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TableSAO", menuName = "SAO/TableSAO")]
public class TableSAO : ScriptableObject
{
    public ItemSpriteSAO[] ItemSpriteTable;

    public ItemSpriteSAO GetItemSprite(int key)
    {
        for (int i = 0; i < ItemSpriteTable.Length; i++)
        {
            if(ItemSpriteTable[i].key == key) return ItemSpriteTable[i];
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TouchItemEvent
{
    public TriggerItem.TriggerItemType Type { get; set; }
    public Vector3 Position { get; set; }
}

public struct ChangeWeaponEvent
{
    public Weapon.WeaponType Type { get; set; }
    public Vector3 Position { get; set; }
}
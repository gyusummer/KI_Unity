using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mapper
{
    public static Weapon.WeaponType GetWeaponType(TriggerItem.TriggerItemType triggerItemType)
    {
        switch (triggerItemType)
        {
            case TriggerItem.TriggerItemType.AssaultRifle:
                return Weapon.WeaponType.AssaultRifle;
            case TriggerItem.TriggerItemType.GrenadeLauncher:
                return Weapon.WeaponType.GrenadeLauncher;
            case TriggerItem.TriggerItemType.Shotgun:
                return Weapon.WeaponType.Shotgun;
            default:
                return Weapon.WeaponType.Unknown;
        }
    }
    
    public static Type GetTypeByTriggerItem(TriggerItem.TriggerItemType triggerItemType)
    {
        switch (triggerItemType)
        {
            case TriggerItem.TriggerItemType.AssaultRifle:
                return typeof(AssultRifle);
            case TriggerItem.TriggerItemType.GrenadeLauncher:
                return typeof(GrenadeLauncher);
            case TriggerItem.TriggerItemType.Shotgun:
                return typeof(ShotGun);
            default:
                return typeof(Weapon);
        }
    }
    
    
}
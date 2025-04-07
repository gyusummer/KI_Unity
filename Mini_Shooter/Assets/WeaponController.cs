using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Gun")]
    public Weapon assaultRifle;
    public Weapon grenadeLauncher;
    public Weapon shotGun;

    private void OnEnable()
    {
        assaultRifle.gameObject.SetActive(false);
        grenadeLauncher.gameObject.SetActive(false);
        shotGun.gameObject.SetActive(false);
    }

    public Weapon GetWeaponOrNull(Weapon.WeaponType type)
    {
        switch (type)
        {
            case Weapon.WeaponType.AssaultRifle:
                return assaultRifle;
            case Weapon.WeaponType.GrenadeLauncher:
                return grenadeLauncher;
            case Weapon.WeaponType.Shotgun:
                return shotGun;
            default:
                return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunData
{
    public string weaponName = "missing name";
    public float fireRate = 0.05f;
    public int damage = 10;
    public int totalAmmo = 30;
    public float reloadTime = 2.0f;
    public float range = 100.0f;
}

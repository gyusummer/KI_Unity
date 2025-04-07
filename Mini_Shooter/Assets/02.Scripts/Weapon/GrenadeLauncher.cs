using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
    [Header("Grenade Launcher Settings")] 
    public Transform firePoint;
    public Grenade grenadePrefab;
    public float force = 100f;
    
    private void Start()
    {
        muzzleFlash.SetActive(false);
    }

    public override bool Fire()
    {
        bool isAbleFire = IsAbleFire();
        if (isAbleFire == false) return false;
        
        
        muzzleFlash.transform.localRotation *= 
            Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
        
        muzzleFlash.SetActive(true);

        var bullet = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        bullet.Damage = data.damage;
        bullet.LaunchGrenade(force);
        
        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrenadeLauncher : Weapon
{
    [Header("Grenade Launcher Settings")]
    public Transform firePoint;
    public Grenade grenadePrefab;
    public float fireForce = 20f;
    
    // [Header("Visual")]
    // public GameObject MuzzleFlash;
    // public float muzzleFlashDuration = 0.1f;
    // private float currentMuzzleFlashDuration { get; set; }

    private void Update()
    {
        currentFireRate += Time.deltaTime;
        // currentMuzzleFlashDuration += Time.deltaTime;
        //
        // if (currentMuzzleFlashDuration >= muzzleFlashDuration)
        // {
        //     currentMuzzleFlashDuration = 0;
        //     MuzzleFlash.SetActive(false);
        // }
    }

    public override bool Fire()
    {
        if (currentFireRate < data.fireRate)
        {
            return false;
        }
        currentFireRate = 0.0f;
        currentAmmo--;

        // MuzzleFlash.transform.localRotation *= Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
        // MuzzleFlash.SetActive(true);
        
        Grenade grenade = ObjectPoolManager.Instance.GetObjectPoolOrNull("Grenade") as Grenade;
        grenade.transform.position = firePoint.position;
        grenade.transform.rotation = firePoint.rotation;
        grenade.LaunchGrenade(fireForce, data.damage);

        return true;
    }
}

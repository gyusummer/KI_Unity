using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Weapon : MonoBehaviour
{
    [System.Serializable]
    public class Pivot
    {
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale;
    }
    
    public enum WeaponType
    {
        Unknown,
        AssaultRifle,
        GrenadeLauncher,
        Shotgun,
    }
    
    [Header("GunData")]
    public GunData data;
    
    [Header("Visual")]
    public GameObject muzzleFlash;
    public float muzzleFlashDuration = 0.1f;

    [Header("PivotSetting")] 
    public Pivot pivot;
    
    public int CurrentAmmo { get; protected set; }
    public float CurrentFireRate { get; protected set; }
    protected float CurrentMuzzleFlashDuration { get; set; }

    public abstract bool Fire();

    private void Update()
    {
        CurrentFireRate += Time.deltaTime;
        CurrentMuzzleFlashDuration += Time.deltaTime;

        if (CurrentMuzzleFlashDuration > muzzleFlashDuration)
        {
            CurrentMuzzleFlashDuration = 0.0f;
            muzzleFlash.SetActive(false);
        }
    }

    protected bool IsAbleFire()
    {
        //가상함수를 사용해서 base로 접근하는 것보다 
        //멤버함수로 따로 선언하여 사용하는것이 더 좋다
        
        if (CurrentFireRate < data.fireRate) return false;
        CurrentFireRate = 0.0f;

        if (CurrentAmmo <= 0) return false;
        CurrentAmmo--;
        return true;
    }

    public void Reload()
    {
        Debug.Log($"Reload ::: {CurrentAmmo} -> {data.totalAmmo}");
        CurrentAmmo = data.totalAmmo;
    }

    public void UpdateTransform(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = pivot.localPosition;
        transform.localRotation = Quaternion.Euler(pivot.localRotation);
        transform.localScale = pivot.localScale;
    }
}

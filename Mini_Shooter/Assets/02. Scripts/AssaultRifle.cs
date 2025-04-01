using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AssaultRifle : MonoBehaviour
{
    [Header("GunData")]
    public GunData data;
    
    [Header("Visual")]
    public GameObject MuzzleFlash;
    public float muzzleFlashDuration = 0.1f;
    
    public int currentAmmo { get; private set; }
    public float currentFireRate { get; private set; }
    private float currentMuzzleFlashDuration { get; set; }
    
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        currentFireRate += Time.deltaTime;
        currentMuzzleFlashDuration += Time.deltaTime;

        if (currentMuzzleFlashDuration >= muzzleFlashDuration)
        {
            currentMuzzleFlashDuration = 0;
            MuzzleFlash.SetActive(false);
        }
    }

    public bool Fire()
    {
        if (currentFireRate < data.fireRate)
        {
            return false;
        }
        currentFireRate = 0.0f;
        currentAmmo--;

        MuzzleFlash.transform.localRotation *= Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
        MuzzleFlash.SetActive(true);
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, data.range, LayerMask.GetMask("Enemy")))
        {
            var monster = CombatSystem.Instance.GetMonsterOrNull(hit.collider);
            if (monster != null)
            {
                CombatEvent combatEvent = new CombatEvent();
                combatEvent.Sender = Player.LocalPlayer;
                combatEvent.Receiver = monster;
                combatEvent.Damage = data.damage;
                combatEvent.HitPosition = hit.point;
                CombatSystem.Instance.AddCombatEvent(combatEvent);
            }
        }
        
        return true;
    }
}

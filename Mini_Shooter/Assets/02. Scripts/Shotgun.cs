using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Shotgun : Weapon
{
    public float Spread = 0.3f;
    public int pellets = 10;

    // random degree(axis forward)
    // random radius 0~spread
    [Header("Visual")]
    public GameObject MuzzleFlash;
    public float muzzleFlashDuration = 0.1f;
    // public Transform Ejector;
    
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
    public override bool Fire()
    {
        if (currentFireRate < data.fireRate)
        {
            return false;
        }
        currentFireRate = 0.0f;
        currentAmmo--;

        MuzzleFlash.transform.localRotation *= Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
        MuzzleFlash.SetActive(true);
        // var shell = ObjectPoolManager.Instance.GetObjectPoolOrNull("BulletShell");
        // shell.GameObject.transform.position = Ejector.position;
        // shell.GameObject.transform.right = -Ejector.right;
        // shell.GameObject.SetActive(true);

        for (int i = 0; i < pellets; i++)
        {
            Vector3 rDistance = new Vector3(0, Random.Range(0, Spread), 0);
            Quaternion rRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
            Vector3 rSpread = rRotation * rDistance;
            
            rSpread = mainCamera.ViewportToScreenPoint(rSpread);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition + rSpread);
            Debug.DrawRay(ray.origin, ray.direction * data.range, Color.red, 1f);
            
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
                    combatEvent.Collider = hit.collider;
                
                    CombatSystem.Instance.AddCombatEvent(combatEvent);
                }
            }
        }
        
        return true;
    }

    public override void Reload()
    {
        
    }
}

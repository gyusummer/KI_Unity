using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    public enum TriggerItemType
    {
        Unknown = 0,
        AssaultRifle = 100,
        GrenadeLauncher,
        Shotgun,
        SniperRifle,
        LightningGun,
        PlasmaGun,
        FlameThrower,
        HealPack = 200,
    }
    
    public float rotateSpeed = 60.0f;
    public TriggerItemType type = TriggerItemType.Unknown;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("GunData")]
    public GunData data;

    public int currentAmmo { get; protected set; }
    public float currentFireRate { get; protected set; }

    public abstract bool Fire();
}

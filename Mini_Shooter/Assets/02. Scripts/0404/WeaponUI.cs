using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text bulletsText;

    public PlayerController player;

    private IEnumerator Start()
    {
        yield return null;
        player.OnWeaponUpdated += UpdateWeapon;
        UpdateWeapon();
    }

    private void OnDestroy()
    {
        player.OnWeaponUpdated -= UpdateWeapon;
    }

    public void UpdateWeapon()
    {
        nameText.text = player.CurrentWeapon.data.weaponName;
        UpdateBullets();
    }

    public void UpdateBullets()
    {
        bulletsText.text = $"{player.CurrentWeapon.currentAmmo.ToString()} / {player.CurrentWeapon.data.totalAmmo.ToString()}";
    }
}

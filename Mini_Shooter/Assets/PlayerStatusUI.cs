using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : StatusUI
{
    public TMP_Text weaponNameText;
    public TMP_Text ammoText;

    private LocalPlayer localPlayer;

    private void Start()
    {
        localPlayer = Player.LocalPlayer;
    }

    private void Update()
    {
        UpdateWeaponName(localPlayer.CurrentWeapon);
        UpdateAmmoText(localPlayer.CurrentWeapon);
        UpdateFillImage(localPlayer.Stat.HP, localPlayer.Stat.MaxHP);
    }

    private void UpdateWeaponName(Weapon current)
    {
        weaponNameText.text = current.data.weaponName;
    }

    private void UpdateAmmoText(Weapon weapon)
    {
        int currentAmmo = weapon.CurrentAmmo;
        int maxAmmo = weapon.data.totalAmmo;
        
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }
}

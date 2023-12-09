using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject weaponObj;
    private weapon selfWpn;
    public int dmg, accuracy, durab, ammoQuantPerShot;
    public float splashRadius, shootSpeed, reloadSpeed;
    public int ammoCap;

    public string ammoType = null;
    public bool holdShoot = false, ignition = false;

    public string WpnName = null;

    public void selectWeapon(weapon wpn) {
        selfWpn = wpn;
        weaponObj.GetComponent<SpriteRenderer>().sprite = wpn.weaponSprite;
        dmg = wpn.Damage;
        durab = wpn.durability;
        accuracy = wpn.accuracy;
        splashRadius = wpn.splashArea;
        shootSpeed = wpn.shootSpeed;
        reloadSpeed = wpn.reloadSpeed;
        ammoQuantPerShot = wpn.ammoQuantityPerShot;
        switch (wpn.ammo)
        {
            case(weapon.ammoType.Light):
                ammoType = "Light";
            break;
            case(weapon.ammoType.Medium):
                ammoType = "Medium";
            break;
            case(weapon.ammoType.Shells):
                ammoType = "Shells";
            break;
            case(weapon.ammoType.Rocket):
                ammoType = "Rocket";
            break;
            case(weapon.ammoType.neuralEnergy):
                ammoType = "neuralEnergy";
            break;
            case (weapon.ammoType.fuel):
                ammoType = "fuel";
            break;
        }
        ammoCap = wpn.ammoCapacity;
        holdShoot = wpn.holdShooting;
        ignition = wpn.IgnitionMode;

        WpnName = wpn.Wname;
    }

    public void deSelectWeapon() {
        selfWpn = null;
        weaponObj.GetComponent<SpriteRenderer>().sprite = null;
        dmg = 0;
        durab = 0;
        accuracy = 100;
        ammoQuantPerShot = 1;
        splashRadius = -1f;
        shootSpeed = 0.8f;
        reloadSpeed = 0.0f;
        ammoType = null;

        WpnName = "none";
    }

    public void setToLoadedState()
    {
        weaponObj.GetComponent<SpriteRenderer>().sprite = selfWpn.weaponSprite;
    }

    public void setToUnloadedState()
    {
        if (selfWpn.unLoadedStateSprite != null)
        {
            weaponObj.GetComponent<SpriteRenderer>().sprite = selfWpn.unLoadedStateSprite;
        }
    }
}

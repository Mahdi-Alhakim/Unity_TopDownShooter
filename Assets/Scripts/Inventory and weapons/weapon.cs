using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "WeaponType", order = 0)]
public class weapon : ScriptableObject
{
    public string Wname;
    public Sprite weaponSprite, unLoadedStateSprite;
    public float splashArea, shootSpeed, reloadSpeed;
    public int Damage, accuracy, price, rarity, durability, ammoQuantityPerShot = 1;
    public bool holdShooting = false;
    public int ammoCapacity;
    public enum ammoType {
        Light, Medium, Shells, Rocket, neuralEnergy, fuel
    }
    [SerializeField] public ammoType ammo;
    public bool ShellRelease = false, IgnitionMode = false;
}

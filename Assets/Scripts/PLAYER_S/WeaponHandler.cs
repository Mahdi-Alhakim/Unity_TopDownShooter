using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponHandler : MonoBehaviour
{
    class Wpninfo { public int durb, loaded; public Wpninfo(int _durb, int _loaded) { durb = _durb; loaded = _loaded; } };

    public weapon pistol, ar, shot, smg, RPG, flameThrower;
    [HideInInspector] public PlayerWeapon wpn;
    public Transform FirePoint_pist, FirePoint_smg, FirePoint_shot, FirePoint_ar, FirePoint_RPG, FirePoint_FT;
    public GameObject bulletprefab, shootLightE, flameEffect;
    private GameObject flames;
    [HideInInspector] public cam_handler CAM;
    public AudioClip Clip;

    private bool reShoot = true;
    private bool loaded = false;
    private bool isReloading = false;
    private bool flashed = false;
    private bool Ignited = false;
    bool anyChange = false;
    [SerializeField] private float loadedAmmoQuant;
    Coroutine reloadRoutine;

    [HideInInspector] public float bulletForce = 22f;

    private Transform shootPoint;

    Dictionary<string, Wpninfo> Stored_wpns;
    Dictionary<string, int> ammo = new Dictionary<string, int>();

    void Start()
    {
        Stored_wpns = new Dictionary<string, Wpninfo>() {
            {"Pistol", new Wpninfo(pistol.durability, 0)},
            {"SMG", new Wpninfo(smg.durability, 0)},
            {"Shotgun", new Wpninfo(shot.durability, 0)},
            {"AR", new Wpninfo(ar.durability, 0)},
            {"RPG", new Wpninfo(RPG.durability, 0)},
            {"FlameThrower", new Wpninfo(flameThrower.durability, 0)}
        };

        ammo = new Dictionary<string, int>() {
            {"Light", pistol.ammoCapacity + smg.ammoCapacity*3},
            {"Medium", ar.ammoCapacity*3},
            {"Shells", shot.ammoCapacity*3},
            {"Rocket", RPG.ammoCapacity*3},
            {"fuel", flameThrower.ammoCapacity*3},
            {"neuralEnergy", 0}
        };

        wpn = this.GetComponent<PlayerWeapon>();
        wpn.selectWeapon(pistol);
        HandleAmmo();
    }

    void Update()
    {
        bool shootBool = false;
        if (wpn.holdShoot) {
            shootBool = Input.GetButton("Fire1");
        }
        if (!wpn.holdShoot) {
            shootBool = Input.GetButtonDown("Fire1");
        }
        if (shootBool && reShoot == true){
            GetComponent<Movement>().setSpeedMultiplier(0.5f);
            if (wpn.WpnName == "FlameThrower") GetComponent<Movement>().setSpeedMultiplier(0.2f);
            Shoot();
            reShoot = false;
            Invoke("enableShoot", wpn.shootSpeed);
        }
        if (Input.GetButtonUp("Fire1")) {
            if (wpn.holdShoot)
                CAM.resetZoom();
            if (wpn.ignition && Ignited)
                EndIgnition();
            GetComponent<Movement>().setSpeedMultiplier(1f);
        }
        if (Ignited) return;
        anyChange = false;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (reloadRoutine != null) { StopCoroutine(reloadRoutine); isReloading = false; }
            wpn.selectWeapon(pistol); //Stored_wpns[wpn.WpnName].loaded = Mathf.Clamp(ammo[wpn.ammoType], 0, wpn.ammoCap);
            CAM.resetZoom();
            anyChange = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (reloadRoutine != null) { StopCoroutine(reloadRoutine); isReloading = false; }
            wpn.selectWeapon(smg); //Stored_wpns[wpn.WpnName].loaded = Mathf.Clamp(ammo[wpn.ammoType], 0, wpn.ammoCap);
            CAM.resetZoom();
            anyChange = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (reloadRoutine != null) { StopCoroutine(reloadRoutine); isReloading = false; }
            wpn.selectWeapon(shot); //Stored_wpns[wpn.WpnName].loaded = Mathf.Clamp(ammo[wpn.ammoType], 0, wpn.ammoCap);
            CAM.resetZoom();
            anyChange = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (reloadRoutine != null) { StopCoroutine(reloadRoutine); isReloading = false; }
            wpn.selectWeapon(ar); //Stored_wpns[wpn.WpnName].loaded = Mathf.Clamp(ammo[wpn.ammoType], 0, wpn.ammoCap);
            CAM.resetZoom();
            anyChange = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (reloadRoutine != null) { StopCoroutine(reloadRoutine); isReloading = false; }
            wpn.selectWeapon(RPG); //Stored_wpns[wpn.WpnName].loaded = Mathf.Clamp(ammo[wpn.ammoType], 0, wpn.ammoCap);
            CAM.resetZoom();
            anyChange = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (reloadRoutine != null) { StopCoroutine(reloadRoutine); isReloading = false; }
            wpn.selectWeapon(flameThrower); //Stored_wpns[wpn.WpnName].loaded = Mathf.Clamp(ammo[wpn.ammoType], 0, wpn.ammoCap);
            CAM.resetZoom();
            anyChange = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadRoutine = StartCoroutine(Reload());
            CAM.resetZoom();
        }

        if (anyChange && Stored_wpns[wpn.WpnName].loaded == 0) wpn.setToUnloadedState();
    }

    private void Shoot() {
        if (Ignited) return;
        switch (GetComponent<PlayerWeapon>().WpnName)
        {
            case ("Shotgun"):
                shootPoint = FirePoint_shot;
                break;
            case ("Pistol"):
                shootPoint = FirePoint_pist;
                break;
            case ("AR"):
                shootPoint = FirePoint_ar;
                break;
            case ("SMG"):
                shootPoint = FirePoint_smg;
                break;
            case ("RPG"):
                shootPoint = FirePoint_RPG;
                break;
            case ("FlameThrower"):
                shootPoint = FirePoint_FT;
                break;
        }
        if (wpn.ignition && reShoot)
        {
            StartIgnition();
            return;
        }
        HandleAmmo();
        if (Stored_wpns[wpn.WpnName].durb != 0 && loaded && isReloading == false) {
            for (int i = 0; i < wpn.ammoQuantPerShot; i++)
            {
                GameObject bullet = Instantiate(bulletprefab, shootPoint.position, shootPoint.rotation);
                bullet.GetComponent<BULLET>().weapon = GetComponent<PlayerWeapon>();
                bullet.GetComponent<BULLET>().setRot(GetComponent<PlayerWeapon>().accuracy);
                bullet.GetComponent<BULLET>().CAM = CAM;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            }
            if (wpn.WpnName != "none") Stored_wpns[wpn.WpnName].durb--;

            //AudioSource source = gameObject.AddComponent<AudioSource>();
            //source.clip = Clip;
            //source.Play();

            wpn.setToUnloadedState();
            if (!flashed) StartCoroutine(Flash());
            GetComponent<Movement>().knockback(new List<float>() {(wpn.dmg * wpn.ammoQuantPerShot)/2.5f}, (float)wpn.dmg * wpn.ammoQuantPerShot / 45f, 0.05f * wpn.shootSpeed);
            CAM.AddShake(wpn.dmg*wpn.ammoQuantPerShot / 2000f, .3f);
            if (wpn.holdShoot)
                CAM.applyShootZoom(wpn.dmg * wpn.ammoQuantPerShot / 22000f);
            else CAM.applyShootZoom(wpn.dmg * wpn.ammoQuantPerShot / 22000f, true);
        } else
        {
            CAM.resetZoom();
        }
        if (Stored_wpns[wpn.WpnName].durb == 0)
        {
            GetComponent<PlayerWeapon>().deSelectWeapon();
        }
    }

    private void HandleAmmo()
    {
        if (Stored_wpns[wpn.WpnName].loaded > 0)
        {
            loaded = true;
            Stored_wpns[wpn.WpnName].loaded--;
            ammo[wpn.ammoType]--;
        } else
        {
            loaded = false;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(wpn.reloadSpeed);
        isReloading = false;
        wpn.setToLoadedState();
        Stored_wpns[wpn.WpnName].loaded = Mathf.Clamp(ammo[wpn.ammoType], 0, wpn.ammoCap);
    }

    IEnumerator Flash()
    {
        flashed = true;
        GameObject lightFlash = (GameObject)Instantiate(shootLightE, shootPoint.position, Quaternion.Euler(0, 0, shootPoint.eulerAngles.z + (int)UnityEngine.Random.Range(-5, 5)));
        lightFlash.transform.position -= 0.12f * transform.up - 0.07f * transform.right;
        float value = wpn.dmg * wpn.ammoQuantPerShot / 20;
        lightFlash.GetComponent<Light2D>().intensity = Mathf.Clamp(value, 0.4f, 1.2f); 
        yield return new WaitForSeconds(0.055f);
        Destroy(lightFlash);
        yield return new WaitForSeconds(0.01f);
        flashed = false;
    }

    private void StartIgnition()
    {
        flames = (GameObject)Instantiate(flameEffect, shootPoint.position, shootPoint.rotation);
        flames.GetComponent<FLAMES>().setShootPoint(shootPoint);
        Ignited = true;
    }

    private void EndIgnition()
    {
        flames.GetComponent<FLAMES>().off();
        Ignited = false;
        reShoot = false;
        Invoke("enableShoot", wpn.shootSpeed);
    }

    private void enableShoot () {reShoot = true; }
}

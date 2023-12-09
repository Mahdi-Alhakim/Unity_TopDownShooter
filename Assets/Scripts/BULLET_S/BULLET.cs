using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BULLET : MonoBehaviour
{


    public Sprite Light_A, Medium_A, Shell_A, Rocket_A;

    public PlayerWeapon weapon;
    public GameObject explosion, RPGexplosionLight;
    private string thisType;
    public int Dmg = 10;
    private GameObject newLight;
    [HideInInspector] public cam_handler CAM;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3);
        if (weapon != null) {
            Dmg = weapon.dmg;

        }
        SpriteRenderer thisSprite = GetComponent<SpriteRenderer>();

        // Setting the bullet's ammoType
        thisType = weapon.ammoType;
        switch (weapon.ammoType)
        {
            case "Light":
                thisSprite.sprite = Light_A;
                break;
            case "Medium":
                thisSprite.sprite = Medium_A;
                break;
            case "Shells":
                thisSprite.sprite = Shell_A;
                break;
            case "Rocket":
                thisSprite.sprite = Rocket_A;
                break;
            case "neuralEnergy":

                break;
        }
    }

    public void setRot(int accuracy)
    {
        float rotation = UnityEngine.Random.Range(GetComponent<Rigidbody2D>().rotation - (100f - accuracy)/2, GetComponent<Rigidbody2D>().rotation + (100f - accuracy)/2);
        GetComponent<Transform>().rotation = Quaternion.Euler(0.0f, 0.0f, rotation);
    }

    void OnTriggerEnter2D (Collider2D other)
    {

        ZOMBIE zomb = other.GetComponent<ZOMBIE>();
        if (zomb != null)
        {
            zomb.TakeDamage(Dmg);
            zomb.knockback(transform.up * weapon.dmg * weapon.ammoQuantPerShot);
            zomb.bloodSplatZomb(transform.rotation, (float)weapon.dmg * weapon.ammoQuantPerShot / 200f);
        }
        PLAYERHEALTH P = other.GetComponent<PLAYERHEALTH>();
        BULLET B = other.GetComponent<BULLET>();
        if (P == null && B == null) {
            Destroy(this.gameObject);
            if (weapon.splashRadius != -1)
            {
                GameObject newobj = Instantiate(explosion, transform.position, transform.rotation);
                newobj.GetComponent<RPG_ExplosioniMainscript>().isRPGexp = true;
                newobj.GetComponent<RPG_ExplosioniMainscript>().RPGdmg = weapon.dmg;
                newobj.GetComponent<RPG_ExplosioniMainscript>().area = weapon.splashRadius;
                Instantiate(RPGexplosionLight, transform.position, transform.rotation);
                newobj.transform.localScale = new Vector2(2.67f*weapon.splashRadius, 2.67f*weapon.splashRadius);
                CAM.AddShake(weapon.dmg / (1000f*Vector2.Distance(weapon.weaponObj.transform.position, transform.position)), .6f);
                
            } else{ Instantiate(explosion, transform.position, transform.rotation); }
        }
    }
}

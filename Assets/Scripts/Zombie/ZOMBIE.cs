using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOMBIE : MonoBehaviour
{
    public MAINSCRIPT main;
    public Rigidbody2D rb;
    public Transform player;

    public float zombspeed = 1f;
    public int maxHealth = 100;
    public int currentHealth;
    public int zombnumber;
    public int damage = 1;
    public float hitSpeed = 2.0f;
    private bool attackAvailable = true;

    private bool inKnockback = false;
    private Vector2 knockbackForce;
    private float KBfriction = 15f;
    
    public GameObject healthBar, bloodSplat, bloodEffect;

    private GameObject HBC;
    private HealthBar HB;


    Vector2 lookDir;

    public void knockback(Vector2 KBforce)
    {
        inKnockback = true;
        knockbackForce = KBforce;
    }

    private void OnCollisionStay2D(Collision2D other) {
        PLAYERHEALTH plyr = other.collider.GetComponent<PLAYERHEALTH>();
        if (plyr != null && attackAvailable) {
            plyr.TakeDamage(damage);
            Vector2 zombToPlayerVec = (plyr.transform.position - transform.position).normalized * damage * 2.5f;
            plyr.GetComponent<Movement>().knockback(new List<float>() {zombToPlayerVec.x, zombToPlayerVec.y});
            attackAvailable = false;
            Invoke("regainStrength", hitSpeed);
        }
    }

    public float TakeDamage(int dmg) {
        if (currentHealth - dmg >= 0) {
            currentHealth -= dmg;
        } else {
            currentHealth = 0;
        }
        HB.SET_H(currentHealth);
        return (float)dmg;
    }

    public void bloodSplatZomb (Quaternion rot, float partFactor, bool isExplosion=false)
    {
        GameObject bloodPS = (GameObject)Instantiate(bloodEffect, transform.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f)), rot);
        bloodPS.GetComponent<BloodRandomizer>().setQuant_Life((int)(1750 * partFactor), 0.004f * partFactor);

        if ((int)Random.Range(0, 3) == 1 || isExplosion == true)
        {
            int? i = !isExplosion ? Random.Range(1, 4) : 8;
            for (int x = 0; x < i; x++)
            {
                if (!isExplosion)
                {
                    Instantiate(bloodSplat, transform.position - 0.7f * transform.up + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f)) + new Vector3(Mathf.Cos(rot.eulerAngles.z), Mathf.Sin(rot.eulerAngles.z)).normalized * Random.Range(0.2f, 0.5f), Quaternion.Euler(0, 0, Random.Range(0, 360)));
                }
                else
                {
                    Instantiate(bloodSplat, transform.position - 0.7f * transform.up + new Vector3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f)) + new Vector3(Mathf.Cos(rot.eulerAngles.z), Mathf.Sin(rot.eulerAngles.z)).normalized * Random.Range(0.8f, 1f), Quaternion.Euler(0, 0, Random.Range(0, 360)));
                }
            }
        }
    }
    
    private void Die() {
        Destroy(HBC);
        main.KillZomb(gameObject);
        
    }
    private void regainStrength() { attackAvailable = true; }

    void Start()
    {
        currentHealth = maxHealth;
        HBC = (GameObject)Instantiate(healthBar, (Vector2)transform.position + Vector2.up, Quaternion.identity);
        HealthBar[] HBs = HBC.GetComponentsInChildren<HealthBar>();
        foreach (HealthBar H in HBs) {
            if (H.gameObject.name == "HealthBar") {
                HB = H;
            }
        }
        HB.SetMaxHealth(maxHealth);
    }

    
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        if (player != null) {
            lookDir = player.position - transform.position;
            lookDir.Normalize();
        } else {
            lookDir = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (!inKnockback)
        {
            rb.MovePosition(rb.position + lookDir * zombspeed * Time.fixedDeltaTime);
        } else
        {
            rb.velocity = knockbackForce * 10 * Time.fixedDeltaTime;
            if (knockbackForce.x - KBfriction >= 0) knockbackForce.x -= KBfriction; else knockbackForce.x = 0;
            if (knockbackForce.y - KBfriction >= 0) knockbackForce.y -= KBfriction; else knockbackForce.y = 0;

            if (knockbackForce.x == 0 && knockbackForce.y == 0) inKnockback = false;
        }
        HBC.transform.position = rb.position + Vector2.up;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }


}

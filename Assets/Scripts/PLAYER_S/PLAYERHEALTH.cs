using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYERHEALTH : MonoBehaviour
{
    public GameObject exp;
    public HealthBar HB;
    public int MaxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        HB.SetMaxHealth(MaxHealth);
    }

    public void TakeDamage(int dmg) {
        if (currentHealth - dmg >= 0) {
            currentHealth -= dmg;
        } else {
            currentHealth = 0;
        }
        HB.SET_H(currentHealth);
        Instantiate(exp, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 0) {
            Destroy(gameObject);
        }
    }
}

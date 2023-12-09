using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_ExplosioniMainscript : MonoBehaviour {
    public bool isRPGexp;
    public int RPGdmg;
    public float area;

    // Start is called before the first frame update
    void Start()
    {
        if ( isRPGexp ) {
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, area);
            for (int i = 0; i < objects.Length; i++)
            {
                ZOMBIE zomb = objects[i].GetComponent<ZOMBIE>();
                if (zomb == null) { continue; }
                float dist = Vector2.Distance((Vector2)transform.position, (Vector2)zomb.transform.position);
                float damaged = zomb.TakeDamage(RPGdmg);
                zomb.knockback(((Vector2)zomb.transform.position - (Vector2)transform.position) * RPGdmg / dist);
                zomb.bloodSplatZomb(transform.rotation, (float)RPGdmg/(100f*dist), true);
            }
        }

    }
}
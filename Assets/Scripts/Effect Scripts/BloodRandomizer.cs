using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BloodRandomizer : MonoBehaviour
{
    public Light2D lightEffect;
    private int partQuant = 0;
    private float partLife = 0.00f;

    public void setQuant_Life(int _q, float _l) { partQuant = (int)Random.Range(_q-100, _q+100); partLife = Random.Range(_l-0.01f, _l+0.01f); }

    void Start()
    {
        if (partLife == 0f) partLife = 0.02f; if (partQuant == 0f) partQuant = 850;
        lightEffect.intensity *= partQuant / 550;
        Mathf.Clamp(partQuant, 850, 1500);
        GetComponent<ParticleSystem>().startLifetime = partLife;
        GetComponent<ParticleSystem>().emission.SetBurst(0, new ParticleSystem.Burst() {count = partQuant});

        GetComponent<ParticleSystem>().Play();
    }
}

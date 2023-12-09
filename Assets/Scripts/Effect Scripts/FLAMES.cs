using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FLAMES : MonoBehaviour
{

    public Transform shootPoint;
    public Light2D light1, closeUpLight;
    public float ignitionTime = 1f;
    private bool Ignited = false, on = true;
    private float flt = 0;
    private Vector3 sP;

    public void setShootPoint(Transform sP)
    {
        shootPoint = sP;
    }

    public void off()
    {
        GetComponent<ParticleSystem>().Stop();
        StartCoroutine(Destroythis());
        sP = transform.position;
        flt = 1; on = false; Ignited = false;
    }

    private IEnumerator Destroythis()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private IEnumerator ignitionlight(bool reverse = false)
    {
        if ((flt >= ignitionTime && !reverse) || (flt <= 0 && reverse)) {
            Ignited = true;
            yield return null;
        }
        light1.intensity = Mathf.Lerp(0f, 0.5f, flt);
        if (reverse)
        {
            flt -= Time.deltaTime;
            closeUpLight.intensity = Mathf.Lerp(0f, 0.5f, flt / 1.5f);
            closeUpLight.transform.position = sP + (0.5f - flt) * transform.up;
        } else flt += Time.deltaTime;
    }

    void Update()
    {
        if (!Ignited) StartCoroutine(ignitionlight(!on));
        if (on)
        {
            transform.position = shootPoint.position;
            transform.rotation = shootPoint.rotation;
        }
    }
}

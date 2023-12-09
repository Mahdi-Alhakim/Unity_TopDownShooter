using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class rpg_exp_light_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("selfDelete", 0.2f);
    }

    // Update is called once per frame
    void selfDelete()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionHandler : MonoBehaviour
{
    public PlayerWeapon weapon;
    public Movement moveScript;
    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<PlayerWeapon>();
        moveScript = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectItem()
    {
        
    }
}

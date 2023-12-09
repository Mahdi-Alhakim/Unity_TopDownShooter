using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAINSCRIPT : MonoBehaviour
{

    public Camera mainCam;
    public canvScript cnvs;
    public GameObject player, zomb;
    private GameObject plyrthing, zombie;
    public HealthBar HB;
    private int zombnumber = 0;
    [SerializeField] private int WAVE = 0;
    [SerializeField] private int ZombiesKilled = 0;

    public List<GameObject> ZOMBs;

    // Start is called before the first frame update
    void Start()
    {
        ZOMBs = new List<GameObject>();
        plyrthing = (GameObject)Instantiate(player, Vector2.zero, Quaternion.identity);
        plyrthing.GetComponent<Movement>().cam = mainCam;
        plyrthing.GetComponent<PLAYERHEALTH>().HB = HB;
        plyrthing.GetComponent<WeaponHandler>().CAM = mainCam.GetComponent<cam_handler>();
        mainCam.gameObject.GetComponent<cam_handler>().player = plyrthing.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (ZOMBs.Count == 0){
            WAVE++;
            newWave();
            cnvs.setWave(WAVE);
        }
    }

    public void newWave() {
        for (int i = 0; i < WAVE; i++) {

            Vector2 zombPOS;
            int side = Random.Range(0, 4);
            int DISToff = Random.Range(0, 2);
            int POSoff;
            if (side == 0) {
                POSoff = Random.Range(-9, 9);
                zombPOS = new Vector2(POSoff, 7 + DISToff);
            } else if (side == 1) {
                POSoff = Random.Range(-9, 9);
                zombPOS = new Vector2(POSoff, -(7 + DISToff));
            } else if (side == 2) {
                POSoff = Random.Range(-7, 7);
                zombPOS = new Vector2(7 + DISToff, POSoff);
            } else {
                POSoff = Random.Range(-7, 7);
                zombPOS = new Vector2(-(7 + DISToff), POSoff);
            }

            zombie = (GameObject)Instantiate(zomb, zombPOS, Quaternion.identity);
            zombie.GetComponent<ZOMBIE>().zombnumber = zombnumber;
            zombie.GetComponent<ZOMBIE>().player = plyrthing.transform;
            zombie.GetComponent<ZOMBIE>().main = this;
            zombnumber++;
            ZOMBs.Add(zombie);
        }
    }
    public void KillZomb(GameObject Zomb) {
        if (ZOMBs.Remove(Zomb)) {
            Destroy(Zomb);
            ZombiesKilled++;
            cnvs.setKillScore(ZombiesKilled);
        }
    }

    public void GAMEOVER() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvScript : MonoBehaviour
{

    public Text killScore, wave;

    public void setKillScore(int score) {
        killScore.text = score.ToString();
    }

    public void setWave(int Wave) {
        wave.text = Wave.ToString();
    }
}

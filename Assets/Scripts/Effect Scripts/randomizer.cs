using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizer : MonoBehaviour
{
    public Gradient grad;
    public Sprite[] imgs;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = grad.Evaluate(Random.Range(0f, 1f));
        float factor = Random.Range(1f, 1.8f);
        transform.localScale *= factor;
        int rndIndex = (int)Random.Range(0, imgs.Length);
        GetComponent<SpriteRenderer>().sprite = imgs[rndIndex];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
    public float heath = 100;
    public bool broken = false;
    public Transform[] indicators;

    void Update()
    {
        
    }

    public void GetDamage()
    {
        if (broken) return;

        heath -= 1;

        if (heath <= 0)
        {
            indicators[0].GetComponent<Renderer>().material.color = Color.red;
            broken = true;
            GameController.instance.numOfBrokenGen += 1;

            GetComponent<MeshCollider>().enabled = false;
        }
        else if (heath <= 20)
            indicators[1].GetComponent<Renderer>().material.color = Color.red;
        else if (heath <= 40)
            indicators[2].GetComponent<Renderer>().material.color = Color.red;
        else if (heath <= 60)
            indicators[3].GetComponent<Renderer>().material.color = Color.red;
        else if (heath <= 80)
            indicators[4].GetComponent<Renderer>().material.color = Color.red;
    }
}

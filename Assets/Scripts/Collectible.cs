using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    public string collectibleName;
    public float speed;

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (collectibleName)
            {
                case "ammo":
                    other.GetComponent<FPSController>().GetAmmo();
                    break;
                case "health":
                    other.GetComponent<FPSController>().GetHealth();
                    break;
            }

            AudioManager.instance.PlaySFX("Pick");

            Destroy(gameObject);
        }
    }
}

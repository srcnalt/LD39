using System.Collections;
using UnityEngine;

public class AntAI : MonoBehaviour
{
    public float speed = 2;
    public float health = 100;
    public float threatDistance = 20;
    public float attackDistance = 5;
    public Transform target;

    public GameObject explosion;
    public GameObject collAmmo;
    public GameObject collHealth;

    public enum AntState { WALKING, ATTACKING, DEAD, WAIT, ATTACK_PLAYER};
    public AntState antState;
    public Transform player;

    void Start ()
    {
        antState = AntState.WALKING;
        
        player = GameObject.Find("Player").transform;

        FindTarget();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Generator")
        {
            antState = AntState.ATTACKING;
            GetComponent<Animator>().Play("Attack");
        }
        else if(other.tag == "Player")
        {
            antState = AntState.ATTACK_PLAYER;
            GetComponent<Animator>().Play("Attack");
        }
    }
	
	void Update ()
    {
        if (Vector3.Distance(player.position, transform.position) < threatDistance)
        {
            //better use another collider?
            target = player;

            if (antState == AntState.ATTACKING)
            {
                antState = AntState.WALKING;
                GetComponent<Animator>().Play("Walk");
            }
        }

        if (antState == AntState.ATTACK_PLAYER && Vector3.Distance(player.position, transform.position) > attackDistance)
        {
            antState = AntState.WALKING;
            GetComponent<Animator>().Play("Walk");
        }

        if (target.tag != "Player" && antState == AntState.ATTACKING)
        {
            if (target.GetComponent<Generator>().broken)
            {
                FindTarget();

                antState = AntState.WALKING;
            }
            else
            {
                antState = AntState.WAIT;

                StartCoroutine(Hit());
            }
        }

        if (antState == AntState.ATTACK_PLAYER)
        {
            antState = AntState.WAIT;

            StartCoroutine(HitPlayer());
        }

        if (antState == AntState.WALKING)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

            transform.LookAt(target.transform);
        }
        
        if(health <= 0 && antState != AntState.DEAD)
        {
            antState = AntState.DEAD;

            StartCoroutine(Die());
        }
    }

    void FindTarget()
    {
        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");
        target = GameObject.Find("FarPoint").transform;
        GetComponent<Animator>().Play("Walk");

        foreach (GameObject generator in generators)
        {
            if (!generator.GetComponent<Generator>().broken && Vector3.Distance(this.transform.position, generator.transform.position) < Vector3.Distance(this.transform.position, target.position))
            {
                target = generator.transform;
            }
        }
    }

    IEnumerator Die()
    {
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);

        AudioManager.instance.PlaySFX("Explosion");

        yield return new WaitForSeconds(0.2f);

        switch(Random.Range(0, 10))
        {
            case 0:
            case 1:
            case 3:
                Instantiate(collAmmo, transform.position, transform.rotation);
                break;
            case 4:
                Instantiate(collHealth, transform.position, transform.rotation);
                break;
        }

        Destroy(gameObject);
    }

    IEnumerator Hit()
    {
        target.GetComponent<AudioSource>().Play();
        target.GetComponent<Generator>().GetDamage();

        yield return new WaitForSeconds(0.5f);
        antState = AntState.ATTACKING;
    }

    IEnumerator HitPlayer()
    {
        target.GetComponent<FPSController>().GetDamage();

        yield return new WaitForSeconds(0.5f);
        antState = AntState.ATTACK_PLAYER;
    }
}

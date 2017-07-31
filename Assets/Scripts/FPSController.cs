using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    public Transform playerCamera;
    public Vector2 maxAngles;
    public Animator animator;
    public float damage;
    public float range;
    public int magazineSize;
    public int magazine;
    public int ammo;
    public float gravity;

    public Text healthText;
    public int health;

    public float speed = 2;
    public float sensitivity = 1;

    private float vertical;
    private float horizontal;

    private float rotationX;
    private float rotationY;

    private bool readyToFire = true;
    private bool reloading = false;

    public Text countDown;
    public int timeLeft = 300;

    public Text ammoText;

    public enum PlayerState {WAIT, READY_TO_FIRE};
    public PlayerState playerState;

    private AudioManager am;

    private CharacterController player;
    
	void Start () {
        player = GetComponent<CharacterController>();
        animator.Play("Move");
        playerState = PlayerState.READY_TO_FIRE;
        am = AudioManager.instance;

        StartCoroutine(CountDown());
    }
	
	void Update ()
    {
        if (!GameController.instance.startGame) return;

        vertical    = Input.GetAxis("Vertical") * speed;
        horizontal  = Input.GetAxis("Horizontal") * speed;

        rotationX   += Input.GetAxis("Mouse X") * sensitivity;
        rotationY   -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationY   = Mathf.Clamp(rotationY, maxAngles.x, maxAngles.y);
        
        Quaternion q_TargetRot = Quaternion.Euler(new Vector3(rotationY, rotationX, 0.0f));
        transform.rotation = q_TargetRot;
        
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        movement = transform.rotation * movement;
        movement.y -= gravity;

        player.Move(movement * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(vertical) + Mathf.Abs(horizontal));

        ammoText.text = string.Format("{0} / {1}", ammo, magazine);

        if (Input.GetButton("Fire1"))
        {
            if (playerState == PlayerState.READY_TO_FIRE)
            {
                StartCoroutine(Shoot());
            }
        }
        else if (Input.GetButton("Fire2"))
        {
            animator.Play("Aim");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Shoot()
    {
        playerState = PlayerState.WAIT;

        if (magazine == 0)
        {
            playerState = PlayerState.READY_TO_FIRE;
            StartCoroutine(Reload());
        }
        else
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range))
            {
                if(hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<AntAI>().health -= 20;
                }
            }

            GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>().Simulate(1);
            animator.Play("Fire");
            magazine--;

            am.PlaySFX("Shoot" + Random.Range(1, 4));

            yield return new WaitForSeconds(0.2f);

            GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>().Simulate(0);

            playerState = PlayerState.READY_TO_FIRE;
        }
    }

    private IEnumerator Reload()
    {
        if (magazine != magazineSize && ammo != 0 && playerState != PlayerState.WAIT)
        {
            playerState = PlayerState.WAIT;

            ammo += magazine;

            if (ammo > 0)
            {
                if (ammo > magazineSize)
                {
                    magazine = magazineSize;
                    ammo -= magazineSize;
                }
                else
                {
                    magazine = ammo;
                    ammo = 0;
                }

                am.PlaySFX("Reload");
                animator.Play("Reload");
            }

            yield return new WaitForSeconds(1.5f);
        }

        playerState = PlayerState.READY_TO_FIRE;
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(5f);

        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);

            timeLeft -= 1;

            countDown.text = string.Format("{0:00}:{1:00}", (timeLeft / 60), (timeLeft % 60));
        }
    }

    public void GetDamage()
    {
        health -= Random.Range(5, 11);
        healthText.text = health.ToString();
    }

    public void GetAmmo()
    {
        if (ammo <= 370) ammo += 30;
    }

    public void GetHealth()
    {
        if (health < 100) health += 20;

        if (health > 100) health = 100;

        healthText.text = health.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject ant;
    public GameObject enemies;
    public int numOfBrokenGen;
    public Transform player;

    public Transform dialogPanel;
    public Transform playerPanel;

    public Transform ferhat;
    public Transform inan;

    public Text dialog;
    public Image blackScreen;

    public Text result;

    public Transform[] spawnPoints;

    private bool ready = false;
    public bool startGame = false; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        AudioManager.instance.PlayMusic("ActionMusic");
        Cursor.visible = false;

        StartCoroutine(Dialog());
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");

        if(ready)
            StartCoroutine(SpawnAnt());

        if (numOfBrokenGen == 2 || player.GetComponent<FPSController>().health <= 0)
        {
            StartCoroutine(Co_End(false));
            KillAllAnts();
        }
        else if (player.GetComponent<FPSController>().timeLeft == 0)
        {
            StartCoroutine(Co_End(true));
            KillAllAnts();
        }
        else if (player.GetComponent<FPSController>().timeLeft == 15)
        {
            ready = false;
        }
    }

    void KillAllAnts()
    {
        GameObject[] ants = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach(GameObject ant in ants)
        {
            Destroy(ant);
        }
    }

    IEnumerator SpawnAnt()
    {
        ready = false;

        yield return new WaitForSeconds(Random.Range(4, 7));

        Instantiate(ant, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, enemies.transform);

        ready = true;
    }

    IEnumerator Dialog()
    {
        yield return new WaitForSeconds(2f);

        dialogPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        dialogPanel.gameObject.SetActive(false);
        playerPanel.gameObject.SetActive(true);
        startGame = true;
        ready = true;
    }
    
    IEnumerator Co_End(bool victory)
    {

        if (victory)
            result.text = "You Win!";
        else
            result.text = "Mission Failed";

        result.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        while (blackScreen.color.a < 255f)
        {
            blackScreen.color += new Color(0, 0, 0, Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        playerPanel.gameObject.SetActive(false);
        dialogPanel.gameObject.SetActive(true);

        ferhat.gameObject.SetActive(false);
        inan.gameObject.SetActive(true);

        if (victory)
        {
            dialog.text = "<b>Incoming Message: </b>Good job soldier! You can head back to the barracks now.";
        }
        else
        {
            dialog.text = "<b>Incoming Message: </b>Such a waste...";
        }

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Menu");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    public Transform mars;
    public float rotationSpeed;
    public Transform panel;

    private bool startButtonClicked = false;
    private Transform mainCamera;

    private AudioManager am;
    
	void Start () {
        mainCamera = Camera.main.transform;
        am = AudioManager.instance;
        Cursor.visible = true;

        am.PlayMusic("MenuMusic");
    }
	
	void Update () {
        mars.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * rotationSpeed);

        MoveToMars();
	}

    public void StartGame()
    {
        startButtonClicked = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MoveToMars()
    {
        if (startButtonClicked)
        {
            mainCamera.GetComponent<Animator>().Play("GoToPoint");

            StartCoroutine(C_FadeMenu());
        }
    }

    private IEnumerator C_FadeMenu()
    {
        while (panel.GetComponent<CanvasGroup>().alpha > 0)
        {
            panel.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        panel.gameObject.SetActive(false);
    }
}

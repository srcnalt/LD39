using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCameraScript : MonoBehaviour
{
    public Image whiteScreen;

    public void FadeScreen()
    {
        StartCoroutine(C_FadeScreen());
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public IEnumerator C_FadeScreen()
    {
        while (whiteScreen.color.a < 255f)
        {
            whiteScreen.color += new Color(0, 0, 0, Time.deltaTime * 2);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}

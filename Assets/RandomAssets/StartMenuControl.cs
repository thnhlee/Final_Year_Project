using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[AddComponentMenu("AnhDuy/StartMenuControl")]
public class StartMenuControl : MonoBehaviour
{
    // References to the canvases
    public Canvas mainMenuCanvas;
    public Canvas settingsCanvas;
    public Canvas modeCanvas;

    public void OnClickStart()
    {
        SceneManager.LoadScene("Lobby");
        Time.timeScale = 1f;
    }
}

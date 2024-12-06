using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlaySceneUIControll : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Canvas mainMenuCanvas;
    public Canvas GameLostCanvas;
    private PlayerHealth player;
    private bool isPause = false;


    void Start()
    {
        
    }

    

    public void OnClickPause()
    {
        if (!isPause)
        {
            isPause = true;
            
            Time.timeScale = 0;
        }
        else
        {
            isPause = false;
            Time.timeScale = 1;
        }

    }

    public void updateLostScene()
    {    
        mainMenuCanvas.gameObject.SetActive(true);
        GameLostCanvas.gameObject.SetActive(true);      
    }
}
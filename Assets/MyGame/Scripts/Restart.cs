using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private GameObject GameOver;
    [SerializeField] private string spawnPointName;

    public void OnClickReplay()
    {
        SceneManagement.Instance.SetTransitionName(spawnPointName);
        SceneManager.LoadScene("Lobby");
        PlayerHealth.Instance.Start();
        UIFade.Instance.FadeToClear();
        Time.timeScale = 1f;
    }

    public void OnClickSettings()
    {
        SceneManagement.Instance.SetTransitionName(spawnPointName);
        PlayerHealth.Instance.Start();
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}

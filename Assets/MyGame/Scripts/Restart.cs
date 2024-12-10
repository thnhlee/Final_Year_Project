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

    public void OnClickResume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void OnClickSettings()
    {
        SceneManagement.Instance.SetTransitionName(spawnPointName);
        PlayerHealth.Instance.Start();
        UIFade.Instance.FadeToClear();
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
        EnemyHealth[] enemyHealths = FindObjectsOfType<EnemyHealth>(); foreach (EnemyHealth enemyHealth in enemyHealths) { enemyHealth.Start(); }
    }
}

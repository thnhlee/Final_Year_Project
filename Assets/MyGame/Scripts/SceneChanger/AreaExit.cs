using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
[AddComponentMenu("ThinhLe/AreaExit")]

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    AudioManager audioManager;

    private void Awake()
    {
        GameObject[] audioGameObjects = GameObject.FindGameObjectsWithTag("Audio");
        //Find the first GameObject with the AudioManager component
        audioManager = Array.Find(audioGameObjects, go => go.GetComponent<AudioManager>() != null)?.GetComponent<AudioManager>();

    }

    private float waitToLoadTime = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/AreaEntrance")]

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    AudioManager audioManager;

    private void Awake()
    {
        GameObject[] audioGameObjects = GameObject.FindGameObjectsWithTag("Audio");
        //Find the first GameObject with the AudioManager component
        audioManager = Array.Find(audioGameObjects, go => go.GetComponent<AudioManager>() != null)?.GetComponent<AudioManager>();

    }

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();
            audioManager.PlaySFX(audioManager.portalOut);

        }
    }
}

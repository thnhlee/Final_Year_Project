using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/AreaEntrance")]

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    public Canvas UI;

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();

            if(transitionName == "Map_1_Entry")
            {
                UI.gameObject.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStory : MonoBehaviour
{
    void OnEnable()
    {
        // Chỉ định sceneName hoặc sceneBuildIndex sẽ tải Scene với chế độ Single
        SceneManager.LoadScene("Story", LoadSceneMode.Single);
    }
}
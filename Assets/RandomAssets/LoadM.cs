using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadM : MonoBehaviour
{
    public float waitTime = 5f;

    void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(5); 
    }
}
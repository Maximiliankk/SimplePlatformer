using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.Log("buildIndex: " + SceneManager.GetActiveScene().buildIndex);
            Debug.Log("SceneCount is: " + SceneManager.sceneCountInBuildSettings);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("Can't load next level. Current scene build index: " + SceneManager.GetActiveScene().buildIndex);
            Debug.Log("SceneCount is: " + SceneManager.sceneCountInBuildSettings);
            SceneManager.LoadScene(0);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class ChangeScene : MonoBehaviour
{
    public string sceneName;

    public void changeGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        transform.parent.gameObject.SetActive(false);
    }

}

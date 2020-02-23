using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour
{
    public string sceneName;

    public void ChangeGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        transform.parent.gameObject.SetActive(false);
    }

}

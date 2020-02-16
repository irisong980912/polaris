using UnityEngine;

/// <summary>
/// When player lighting up a star, then check if level is cleared
/// </summary>
public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    public int totalStarNum;

    public Transform cam;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("collide with clear");
        // if the player has collected sufficient number of stars
        if (other.CompareTag("|Player|") &&
            totalStarNum == other.GetComponent<ThirdPersonPlayer>().litStarNum)
        {
            // camera pans
            cam.GetComponent<ThirdPersonCamera>().isCleared = true;

            Invoke("showClearImage", 6);
        }
    }

    void showClearImage()
    {
        clearLevelImage.SetActive(true);
    }



}

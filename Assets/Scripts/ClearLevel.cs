using UnityEngine;

public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    public int totalStarNum;

    public Transform cam;

    private void OnTriggerStay(Collider other)
    {

        // if the player has collected sufficient number of stars
        if (other.CompareTag("|Player|") &&
            totalStarNum == other.GetComponent<ThirdPersonPlayer>().litStarNum)
        {
            // camera pans
            cam.GetComponent<ThirdPersonCamera>().isCleared = true;

            Invoke("showClearImage", 7);
        }
    }

    void showClearImage()
    {
        clearLevelImage.SetActive(true);
    }



}

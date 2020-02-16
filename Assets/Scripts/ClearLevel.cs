using UnityEngine;

/// <summary>
/// When player lighting up a star, then check if level is cleared
/// </summary>
public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    public int totalStarNum;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("collide with clear");
        // if the player has collected sufficient number of stars
        if (other.CompareTag("|Player|") &&
            totalStarNum == other.GetComponent<ThirdPersonPlayer>().litStarNum)
        {
            //TopDownCameraPans();
            clearLevelImage.SetActive(true);
        }
    }


    private void TopDownCameraPans()
    {
        
    }
    

}

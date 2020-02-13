using UnityEngine;

public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("|Player|"))
        {
            clearLevelImage.SetActive(true);
        }
    }

}

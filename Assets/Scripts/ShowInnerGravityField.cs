using UnityEngine;

public class ShowInnerGravityField : MonoBehaviour
{
    private MeshRenderer _renderInside;


    private void Start()
    {
        _renderInside = gameObject.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (!player.CompareTag("MainCamera")) return;
        _renderInside.enabled = true;
        Debug.Log("enabled");
    }

    private void OnTriggerExit(Collider player)
    {
        if (!player.CompareTag("MainCamera")) return;
        _renderInside.enabled = false;
        Debug.Log("disabled");
    }
}

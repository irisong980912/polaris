using System;
using UnityEngine;

public class StardustPickup : MonoBehaviour
{
    public int stardustValue = 1;
    // TODO: implement me.
    //public AudioClip clip;
    public GameObject stardust;
    public static event Action<bool> OnPickingStardusts;
    private bool _remindPicking;

    public void Start()
    {
        _remindPicking = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("|Player|")) return;
        var player = other.GetComponent<ThirdPersonPlayer>();
        if (player is null) return;

        // radius is set to 50 for reminding
        if (_remindPicking)
        {
            OnPickingStardusts?.Invoke(true);
        }
        
        var disToPlayer = Vector3.Distance(transform.position, other.transform.position);
        if (!(disToPlayer < 25)) return;
        if (disToPlayer > 5)
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 2);
        }
        else
        {
            Debug.Log("pickup add");
            player.stardust += stardustValue;
            player.inventory.Add(stardust);
            gameObject.SetActive(false);

            if (!_remindPicking) return;
            Invoke(nameof(DisableTutorial), 0);

        }
        
        // remind player after 1 minute
        Invoke(nameof(RemindPicking), 60.0f);
    }

    private void DisableTutorial()
    {
        OnPickingStardusts?.Invoke(false);
        _remindPicking = false;
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("|Player|")) return;
        if (!_remindPicking) return;
        OnPickingStardusts?.Invoke(false);
        _remindPicking = false;
    }

    private void RemindPicking()
    {
        _remindPicking = true;
    }

}

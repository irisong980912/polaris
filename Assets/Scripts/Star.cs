using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages star content
/// 
/// </summary>
public class Star : MonoBehaviour
{

    public List<GameObject> planetCores = new List<GameObject>();
    public bool isCreated;

    void Start()
    {
        Debug.Log("hi star");
        isCreated = false;
        AddDescendantsWithTag(transform, "|PlanetCore|");
        PrintAllPlanetCores();
    }

    /// <summary>
    /// Recursively add all the planets into the planet list
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    private void AddDescendantsWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            Debug.Log(child.name);

            if (child.gameObject.CompareTag(tag))
            {
                Debug.Log("add");
                planetCores.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, tag);
        }
    }

    /// <summary>
    /// For debug usage
    /// 
    /// </summary>
    private void PrintAllPlanetCores()
    {
        foreach (GameObject core in planetCores)
        {
            Debug.Log(core.name);
        }
    }



}
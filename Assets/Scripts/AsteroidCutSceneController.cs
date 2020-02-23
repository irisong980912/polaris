using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AsteroidCutSceneController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Camera cutSceneCam1;

    public void Play(){
        //Enable cutscene camera that looks at the asteroid
        cutSceneCam1.enabled = true;
        
        //Play cutscene
        playableDirector.Play();

        //Controls what happens after the cutscene plays
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (playableDirector == aDirector)
        {
            //Disable cutscene camera after the timeline stops playing
            cutSceneCam1.enabled = false;
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
        }    
            
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPrompts : MonoBehaviour
{
    public TextMeshProUGUI tutorialtext;

    void OnTriggerStay(Collider other)
    {
        var tutorialzone = other.name;
        if (tutorialzone == "TutorialZone1")
        {
            tutorialtext.text = "You have a stardust in your inventory, try walking up to a dead star and pressing Cross/A";
        } else if (tutorialzone == "TutorialZone2"){
            tutorialtext.text = "Awesome, do you see that glowing ball? That is stardust, pick it up to activate more stars";
        } else if (tutorialzone == "TutorialZone3"){
            tutorialtext.text = "See that black ball? That is a planet. Ride it's orbit and press Square/X to slingshot";
        }else if (tutorialzone == "TutorialZone4"){
            tutorialtext.text = "Find the scattered stardust and light up the remaining stars";
        }else {
            tutorialtext.text = "";
        }
        if (other.tag.Contains("|Star|"))
        {
            if (other.GetComponent<CreateStar>().enabled == true && other.GetComponent<DestroyStar>().enabled == false){
                tutorialtext.text = "Press Cross/A to activate star";
            }
            else if (other.GetComponent<CreateStar>().enabled == false && other.GetComponent<DestroyStar>().enabled == true){
                tutorialtext.text = "Press Cross/A to deactivate star";
            }
        }
        if (other.tag.Contains("|Planet|"))
        {
            tutorialtext.text = "Press Square/X to slingshot";
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Destroy everything that leaves the trigger
        tutorialtext.text = "";
    }
}

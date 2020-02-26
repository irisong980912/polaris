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
            tutorialtext.text = "You have a stardust in your inventory, try walking up to a dead star and pressing X";
        } else if (tutorialzone == "TutorialZone2"){
            tutorialtext.text = "Awesome, do you see that glowing ball? That is a stardust, pick it up to activate more stars";
        } else if (tutorialzone == "TutorialZone3"){
            tutorialtext.text = "See that black ball? That is a planet. Ride it's orbit and press B to slingshot";
        }else if (tutorialzone == "TutorialZone4"){
            tutorialtext.text = "Find the scattered stardust and light up the remaining stars";
        }else {
            tutorialtext.text = "";
        }
        if (other.tag.Contains("|Star|"))
        {
            tutorialtext.text = "Press X to activate star";
        }
        if (other.tag.Contains("|Planet|"))
        {
            tutorialtext.text = "Press B to slingshot";
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Destroy everything that leaves the trigger
        tutorialtext.text = "";
    }
}

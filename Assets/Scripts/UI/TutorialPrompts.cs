using UnityEngine;
using TMPro;

public class TutorialPrompts : MonoBehaviour
{
    public TextMeshProUGUI tutorialtext;
    private bool _isFirstEntryToGravityField = true;
    private bool _isAsteroid = true;

    private void OnTriggerStay(Collider other)
    {
        var tutorialzone = other.name;
        if (tutorialzone == "TutorialZone1")
        {
            tutorialtext.text = "You have a stardust in your inventory, try walking up to a dead star and pressing X";
        } else if (tutorialzone == "TutorialZone2"){
            tutorialtext.text = "Awesome, do you see that glowing ball? That is stardust, pick it up to activate more stars";
        } else if (tutorialzone == "TutorialZone3"){
            tutorialtext.text = "See that colourful ball? That is a planet. Ride it's orbit and press ◻ to slingshot";
        } else if (tutorialzone == "TutorialZone4"){
            tutorialtext.text = "Find the scattered stardust and light up the remaining stars";
        } else {
            tutorialtext.text = "";
        }
        
        if (other.tag.Contains("|Star|"))
        {
            if (other.GetComponent<CreateStar>().enabled && other.GetComponent<DestroyStar>().enabled == false){
                tutorialtext.text = "Press X to activate star";
            }
            else if (other.GetComponent<CreateStar>().enabled == false && other.GetComponent<DestroyStar>().enabled){
                tutorialtext.text = "Press X to deactivate star";
            }
        }
        if (other.tag.Contains("|PlanetCore|"))
        {
            tutorialtext.text = "Press ◻ to slingshot. You can also slingshot to escape the gravity field";
        }
        
        if (other.tag.Contains("|Asteroids|") && _isAsteroid)
        {
  
            tutorialtext.text = "This is an asteroid belt. You cannot walk past it.";
            Invoke(nameof(SetAsteroidFalse), 4);
  
            
        }
        

        if (other.tag.Contains("|GravityCore|"))
        {
            if (_isFirstEntryToGravityField)
            {
                tutorialtext.text = "You have entered the gravitational field of a star";
                Invoke(nameof(SetFalse), 3);
                
            }
            
            if (other.transform.parent.name == "Star 1" &&
                other.transform.parent.GetComponent<Star>().isCreated 
                && other.GetComponent<Gravity>().disToPlayer > other.GetComponent<Gravity>().gravityRadius * .50f)       
            {
                if (other.transform.parent.name == "Star 1")
                {
                    tutorialtext.text = "Long press O when moving to escape the gravitational field." +
                                                        " You can only use Mega Boost when you have 3 or more stardust";
                }
                else
                {
                    tutorialtext.text = "Long press O to perform MegaBoosts";
                }
                
            }
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        // Destroy everything that leaves the trigger
        tutorialtext.text = "";
    }
    
    private void SetFalse()
    {
        _isFirstEntryToGravityField = false;
    }
    
    private void SetAsteroidFalse()
    {
        _isAsteroid = false;
        Invoke(nameof(SetAsteroidTrue), 4);
    }
    
    private void SetAsteroidTrue()
    {
        _isAsteroid = true;
    }
    
}
    

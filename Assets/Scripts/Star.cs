using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : InteractableItemBase
{
    private bool mIsActive = false;
    public override void OnInteract() 
    {
        InteractText = "Press F to ";
        mIsActive = !mIsActive;
        InteractText += (mIsActive ? "activate star" : "deactivate star");
    } 

}

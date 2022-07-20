using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    //FixedUpdate means it will occur at the same time within the game,
    //happens several times as you move through the game
    void FixedUpdate()
    {
        //stops scroll if you die
        if (PlayerController.isDead) return;
        //creates a scrolling behavior that we will apply to our platforms. Sets speed of scroll.
        //
        this.transform.position += PlayerController.player.transform.forward * -0.1f;

        //accounts for null value to prevent errors
        if (PlayerController.currentPlatform == null) return;

        //the -0.06 works with the -0.1 above. for every 0.1 forward,
        //the stairs go up .06
        //since it is our ground that is moving and not our player,
        //stairs going up means the platform has to move down
        if (PlayerController.currentPlatform.tag == "stairsUp")
            this.transform.Translate(0, -0.06f, 0);
        if (PlayerController.currentPlatform.tag == "stairsDown")
            this.transform.Translate(0, 0.06f, 0);
        //increments speed by 25% of the original speed every 30 seconds
        Time.timeScale = (float)(Time.realtimeSinceStartup / 30)/ 4 + 1f;
    }
}

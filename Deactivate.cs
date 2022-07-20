using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour
{
    //flags if deactivate has been scheduled or not so that it can't accidentally be triggered multiple times
    bool dScheduled = false;
    //using the capsule colliders placed on the objects in Unity
    //when the player passes through the collider, a countdown starts
    //after 4 seconds, it is deactivated
    void OnCollisionExit(Collision player)
    {
        //if you die, the platform isn't deactivated
        if (PlayerController.isDead) return;

        if (player.gameObject.tag == "Player" && !dScheduled)
        {
            Invoke("SetInactive", 4.0f);
            dScheduled = true;
        }
    }

    void SetInactive()
    {
        this.gameObject.SetActive(false);
        dScheduled = false;
    }
}

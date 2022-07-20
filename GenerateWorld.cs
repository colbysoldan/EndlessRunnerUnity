using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateWorld : MonoBehaviour
{
    //this script will generate the platforms randomly forever and ever

    static public GameObject dummyTraveller;
    static public GameObject lastPlatform;

    //loads Menu scene
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void Awake()
    {
        //dummyTraveller will help up change direction when we turn left or right
        dummyTraveller = new GameObject("dummy");
    }

    public static void RunDummy()
    {
        //retrieves random object from pool
        GameObject p = Pool.singleton.GetRandom();
        //null check
        if (p == null) return;

        if (lastPlatform != null)
        {
            //tsections are longer than other sections and require a movement of 20
            if (lastPlatform.tag == "platformTSection")
                dummyTraveller.transform.position = lastPlatform.transform.position +
                PlayerController.player.transform.forward * 20;

            else
                //moves dummy position forward by 10 (one platform length)
                dummyTraveller.transform.position = lastPlatform.transform.position +
                    PlayerController.player.transform.forward * 10;

            //accounts for y axis change with stairs up
            if (lastPlatform.tag == "stairsUp")
                dummyTraveller.transform.Translate(0, 5, 0);
        }

        //uses that dummytraveller position to place a platform
        //sets as active so it is shown in the game
        lastPlatform = p;
        p.SetActive(true);
        p.transform.position = dummyTraveller.transform.position;
        p.transform.rotation = dummyTraveller.transform.rotation;

        if (p.tag == "stairsDown")
        {
            dummyTraveller.transform.Translate(0, -5, 0);
            p.transform.Rotate(0, 180, 0);
            p.transform.position = dummyTraveller.transform.position;
        }
    }
}

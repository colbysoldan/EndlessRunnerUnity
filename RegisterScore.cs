using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameData.singleton.scoreText = this.GetComponent<Text>();
        GameData.singleton.UpdateScore(0);
    }


}

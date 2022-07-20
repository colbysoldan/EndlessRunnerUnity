using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatUpText : MonoBehaviour
{
    Text text;
    //like in rgba, alpha is opacity
    float alpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        text.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        //moves text upward, then
        //fades color out by decrementing alpha
        this.transform.Translate(0, 20, 0);
        alpha -= 0.05f;
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

        //when alpha reaches zero, the text is destroyed
        if (alpha < 0) Destroy(this.gameObject);
    }
}

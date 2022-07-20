using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //used below to affect the mesh renderers (mrs) of all children of an object
    MeshRenderer[] mrs;
    public GameObject scorePrefab;
    public GameObject particlePrefab;
    GameObject canvas;

    private void Start()
    {
        mrs = this.GetComponentsInChildren<MeshRenderer>();
        canvas = GameObject.Find("Canvas");
    }

    //updates coin score
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameData.singleton.UpdateScore(10);
            PlayerController.sfx[8].Play();
            //sends score text up from collected coinß
            GameObject scoreText = Instantiate(scorePrefab);
            scoreText.transform.parent = canvas.transform;

            GameObject pE = Instantiate(particlePrefab, this.transform.position, Quaternion.identity);
            Destroy(pE, 1);

            Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            scoreText.transform.position = screenPoint;
            //turns off meshrenderer for coins when they're collected
            foreach (MeshRenderer m in mrs)
                m.enabled = false;
        }
    }

    private void OnEnable()
    {
        //turns renderer back on so coins can be collected again when platform is regenerated
        if(mrs != null)
        foreach (MeshRenderer m in mrs)
            m.enabled = true;
    }
}

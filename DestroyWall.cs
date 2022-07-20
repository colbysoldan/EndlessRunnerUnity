using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    //array of bricks that make up wall
    public GameObject[] bricks;
    List<Rigidbody> bricksRBs = new List<Rigidbody>();
    //positions and roatations of all of the bricks in order to reset
    List<Vector3> positions = new List<Vector3>();
    List<Quaternion> rotations = new List<Quaternion>();

    public GameObject explosion;

    Collider col;

    private void OnEnable()
    {
        col.enabled = true;
        for(int i = 0; i < bricks.Length; i++)
        {
            //regenerates bricks to original position
            bricks[i].transform.localPosition = positions[i];
            bricks[i].transform.localRotation = rotations[i];
            bricksRBs[i].isKinematic = true;
        }
    }
    //awake makes this happen first
    private void Awake()
    {
        col = this.GetComponent<Collider>();
        foreach(GameObject b in bricks)
        {
            bricksRBs.Add(b.GetComponent<Rigidbody>());
            //local positions are relative to the platform they're a child of
            positions.Add(b.transform.localPosition);
            rotations.Add(b.transform.localRotation);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Spell")
        {
            GameObject obj = Instantiate(explosion, other.contacts[0].point, Quaternion.identity);
            
            Destroy(obj, 2.5f);
            col.enabled = false;
            foreach (Rigidbody r in bricksRBs)
                r.isKinematic = false;

            PlayerController.sfx[4].Play();
        }
    }
}


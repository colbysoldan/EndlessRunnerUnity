using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//serializable makes it visibile in the inspector in unity
[System.Serializable]
public class PoolItem
{
    //this creates a base for our pool, which is set to hold a certain amount of items,
    //if we didn't have a pool, items would keep generating and taking up more and more memory
    
    //The pool means we aren't instantiating or destroying items, only activating and deactivating them.
    //When they're deactivated, they return to the pool

    //links to platforms in unity
    public GameObject prefab;

    //create more than one of each platform if we want.
    public int amount;

    // The expandable bool lets the item count be overridden if we happen to need more of an item.
    public bool expandable;
}

public class Pool : MonoBehaviour
{
    //let's us easily access the pool, only ever one of these in the scene
    public static Pool singleton;

    //list of prefabs and how many you can have/whether expandable
    public List<PoolItem> items;

    //used to pull items out of while in use and put back into when not in use
    public List<GameObject> pooledItems;


    private void Awake()
    {
        //this refers to the instantiation of this particular class
        singleton = this;

        //we need to have pooled items created and ready to use
        pooledItems = new List<GameObject>();
        foreach (PoolItem item in items)
        {
            for (int i = 0; i < item.amount; i++)
            {
                //creates a game item
                GameObject obj = Instantiate(item.prefab);
                //immediately sets it to inactive
                obj.SetActive(false);
                //adds to pool until we're ready to use it
                pooledItems.Add(obj);
            }
        }


    }

    public GameObject GetRandom()
    {
        //Fisher-Yates shuffle
        Utils.Shuffle(pooledItems);

        //loop through list
        for (int i = 0; i < pooledItems.Count; i++)
        {
            //if the item isn't active in the hierarchy, it is available for use
            if (!pooledItems[i].activeInHierarchy)
            {
                return pooledItems[i];
            }
        }
        //if there are none of that item left, first check to see if the item is expandable
        foreach (PoolItem item in items)
        {
            //if it is, instantiate and add to pool
            if (item.expandable)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                pooledItems.Add(obj);
                return obj;
            }
        }
        //if there are no items that can be expanded, return nothing
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}

//The Fisher-Yates Shuffle
//creating a separate class means we can use it in various places
public static class Utils
    {
        //plucks random values out of list and puts them elsewhere in the list
        public static System.Random r = new System.Random();
    //so that it can take any type of list, use "this" keyword
    public static void Shuffle<T>(this IList<T> list)
        {

        //have to have a list of more than one item
        //goes backward through list
        int n = list.Count;
            while(n>1)
            {
                n--;
            //plus one means we can't get the zero item
                int k = r.Next(n + 1);
            //the following code swaps the values around in the list
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
            }
        }

    }

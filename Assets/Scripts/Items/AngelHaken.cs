using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelHaken : MonoBehaviour {

    private GameObject Angel; 
    public FishingRod myFishingRod;
    public GameObject SoundManager;

	// Use this for initialization
	void Start ()
    {
        SoundManager = GameObject.Find("SoundManager");
        Angel = transform.root.gameObject;
        myFishingRod = Angel.GetComponent<FishingRod>();
    }
	
	// Update is called once per frame
	void Update ()
    {


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.GetComponent("Fishable") as Fishable) != null)
        {
            myFishingRod.waitonbitetimer = Random.Range(0, 10);
            myFishingRod.fishIndex = Random.Range(0, myFishingRod.Fishsorts.Count);
            if (myFishingRod.Ftiming == true)
            {
                SoundManager.SendMessage("PlaySound", "waterfill");
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if((col.gameObject.GetComponent("Fishable") as Fishable) != null)
        {
            myFishingRod.isfishable = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        myFishingRod.isfishable = false;
    }
}

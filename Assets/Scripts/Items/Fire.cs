﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public Ctrl myCtrl;
    private GameObject Charakter;
    public GameObject Feuer;
    public float timer = 32;
    public Animator animator;
    public GameObject FuelSlot;
    public GameObject RoastSlot;
    public GameObject RoastRdySlot;
    private GameObject Roasted;
    public GameObject SoundManager;
    private Transform FuelChild;
    private Transform RoastChild;
    private int fuelcounter;
    private int roastcounter;
    private bool firetrigger = true;
    private float roasttimer = 20f;
    private float fueltimer = 2f;
    private float fueltimertmp;
    private bool gettimer = true;
    public string item;

    // Use this for initialization
    void Start ()
    {
        SoundManager = GameObject.Find("SoundManager");
        Feuer.transform.GetComponent<SpriteRenderer>().sprite = null;
        animator = GetComponent<Animator>();
        Charakter = GameObject.Find("Charakter");
        myCtrl = Charakter.GetComponent<Ctrl>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        fuelcounter= FuelSlot.transform.childCount;
        roastcounter = RoastSlot.transform.childCount;

        Roasting();
        Fuelwaste();
    }


    // Feuer anzündeun und löschen
    void OnTriggerStay2D(Collider2D col)
    {
        if (fuelcounter > 1) //  && col.transform.childCount == 0
        {
            FuelChild = FuelSlot.gameObject.transform.GetChild(1);
        }

        if (col.CompareTag("Tinderdrill") && myCtrl.Wftiming == true && myCtrl.Wftimer < 0.5f
            && fuelcounter > 1 && FuelChild.CompareTag("Fuel"))
        {
            animator.SetBool("fire", true);

            if (animator.GetBool("fire"))
            {
                SoundManager.SendMessage("PlaySound", "firesound");
            }
            else
            {
                SoundManager.SendMessage("PlaySound", "stop");
            }
        }
        if (col.CompareTag("waterfillable") && myCtrl.Wftiming == true && myCtrl.Wftimer < 0.5f
            && animator.GetBool("fire"))
        {
            animator.SetBool("fire", false);
            SoundManager.SendMessage("PlaySound", "fireout");
        }

    }

    void Roasting()
    {
        if (roastcounter > 1)
        {
            RoastChild = RoastSlot.gameObject.transform.GetChild(1);
        }

        if (animator.GetBool("fire") && roastcounter >1
            && (RoastChild.CompareTag("roastable") || RoastChild.CompareTag("mealtable")))
        {
            roasttimer -= Time.deltaTime;
        }
        if(roasttimer<=0 && roastcounter > 1)
        {
            RoastChild = RoastSlot.gameObject.transform.GetChild(1);

            if (RoastChild.CompareTag("mealtable") || (RoastChild.CompareTag("roastable")))
            {
                string Newitem = ChooseProduct(RoastChild.name);
                Roasted = Instantiate(Resources.Load<GameObject>("AllPrefabs/" + Newitem), new Vector3(0, 0, 0), Quaternion.identity);
                Roasted.name = Newitem;

                Destroy((RoastChild as Transform).gameObject);
                Roasted.transform.SetParent(RoastRdySlot.transform);
                roasttimer = 20f;
            }
        }
    }

    void Fuelwaste()
    {
        if (fuelcounter > 1)
        {
            FuelChild = FuelSlot.gameObject.transform.GetChild(1);
        }
        else
        {
            gettimer = true;
        }
        if(fuelcounter == 1 && gettimer == true)
        {
            fueltimer = fueltimertmp;
        }
        if(gettimer == true && fuelcounter > 1
            && FuelChild.CompareTag("Fuel"))
        {
            Items myItems = FuelChild.GetComponent<Items>();
            fueltimer = myItems.fuelwerte[FuelChild.name];
            fueltimertmp = fueltimer;
        }

        if (fuelcounter > 1 && animator.GetBool("fire")
            && FuelChild.CompareTag("Fuel"))
        {
            gettimer = false;
            FuelChild = FuelSlot.gameObject.transform.GetChild(1);
            fueltimer -= Time.deltaTime;
            if(fueltimer<=0)
            {
                gettimer = true;
                Items myItems = FuelChild.GetComponent<Items>();
                fueltimer = myItems.fuelwerte[FuelChild.name];
                Destroy((FuelChild as Transform).gameObject);
            }
        }
        else { animator.SetBool("fire", false); }
    }

    public string ChooseProduct(string name)
    {
        switch (name)
        {
            case "Tinore": item = "Tiningot"; break;
            case "Copperore": item = "Copperingot"; break;
            case "Ironore": item = "Ironingot"; break;
            case "Goldore": item = "Goldingot"; break;
            case "Apple": item = "Apple_roasted"; break;
            case "Peach": item = "Peach_roasted"; break;
            case "Meat": item = "Meat_roasted"; break;
            case "Mutton": item = "Mutton_roasted"; break;
            case "Sandpile": item = "Glas"; break;
            case "Egg": item = "Egg_fried"; break;

            default: Debug.Log("error"); break;
        }
        return item;
    }
}

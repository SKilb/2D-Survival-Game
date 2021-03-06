﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCtrl : MonoBehaviour
{
    public MapGenerator myMapgenerator;
    public GameObject Mapgenerator;
    public NightDayCircel myNightDayCircel;
    public GameObject NightDay;
    public Ctrl myCtrl;
    private float speed = 0.5f;
    private bool setleft;
    public Animator animator;
    public Vector3 Targetposition;
    private GameObject Item = null;
    public GameObject Charakter;
    public GameObject Attacker;
    public GameObject Camera;
    public bool moverandom = false;
    public bool movetoplayer = false;
    public bool searchitem = false;
    private bool hiding = false;
    private bool sleeping = false;
    private bool death = false;
    public bool attacking = false;
    public bool attackrange = false;
    private float ThisX;
    private float ThisY;
    private int roll;
    private float waittimer;
    private float currentHealth = 1;
    private float maxHunger = 100;
    public float currentHunger;
    private float placeholderX;
    private float placeholderY;
    private Collider2D[] arrayofItems;
    public LayerMask layermask;
    public Dictionary<string, int> animaldamage;
    public Dictionary<string, int> health;
    public float distanceH;
    public float distance;
    public float disappeartimer = 10;
    public Dictionary<string, int> Hidedrop;
    public Dictionary<string, int> Meatdrop;
    public Dictionary<string, int> Featherdrop;
    public Dictionary<string, int> Linendrop;
    public Dictionary<string, int> Eggdrop;
    public Dictionary<string, int> Wooldrop;
    public int fellcounter;
    public int fleischcounter;
    public int federcounter;
    public int linencounter;
    public int eggcounter;
    public int woolcounter;


    // Use this for initialization
    void Start()
    {
        Mapgenerator = GameObject.Find("MapGenerator");
        myMapgenerator = Mapgenerator.GetComponent<MapGenerator>();
        Camera = GameObject.Find("Main Camera");
        NightDay = Camera.transform.GetChild(0).gameObject;
        myNightDayCircel = NightDay.GetComponent<NightDayCircel>();
        Charakter = GameObject.Find("Charakter");
        myCtrl = Charakter.GetComponent<Ctrl>();
        Direction();
        animator = GetComponent<Animator>();

        //*****************************************************************//
        //  HIER Tierleben EINFÜGEN                                      //
        //*****************************************************************//
        health = new Dictionary<string, int>();
        health.Add("Crawler", 50);
        health.Add("Grashopper", 50);
        health.Add("Squirrel", 50);
        health.Add("Opossum", 70);
        health.Add("Rabbit", 80);
        health.Add("Chicken", 90);
        health.Add("Fox", 100);
        health.Add("Deer", 150);
        health.Add("Alpaca", 150);
        health.Add("Wolve", 200);     
        health.Add("Bear", 300);
        // Start Leben setzen
        currentHealth = health[this.name];
        // Start Hunger setzen
        currentHunger = Random.Range(25, 100);

        //*****************************************************************//
        //  HIER Tierschaden EINFÜGEN                                      //
        //*****************************************************************//

        animaldamage = new Dictionary<string, int>();
        animaldamage.Add("Crawler", 2);
        animaldamage.Add("Grashopper", 2);
        animaldamage.Add("Opossum", 5);
        animaldamage.Add("Chicken", 5);
        animaldamage.Add("Rabbit", 5);
        animaldamage.Add("Squirrel", 5);
        animaldamage.Add("Fox", 10);
        animaldamage.Add("Deer", 15);
        animaldamage.Add("Alpaca", 15);
        animaldamage.Add("Wolve", 20);
        animaldamage.Add("Bear", 30);


        //*****************************************************************//
        //  HIER Felldrop EINFÜGEN                                      //
        //*****************************************************************//

        Hidedrop = new Dictionary<string, int>();
        Hidedrop.Add("Fox", 1);
        Hidedrop.Add("Deer", 2);
        Hidedrop.Add("Bear", 5);
        Hidedrop.Add("Chicken", 0);
        Hidedrop.Add("Rabbit", 1);
        Hidedrop.Add("Squirrel", 1);
        Hidedrop.Add("Wolve", 3);


        //*****************************************************************//
        //  HIER Fleischdrop EINFÜGEN                                      //
        //*****************************************************************//

        Meatdrop = new Dictionary<string, int>();
        Meatdrop.Add("Fox", 1);
        Meatdrop.Add("Deer", 2);
        Meatdrop.Add("Chicken", 1);
        Meatdrop.Add("Bear", 5);
        Meatdrop.Add("Rabbit", 1);
        Meatdrop.Add("Grashopper", 1);
        Meatdrop.Add("Opossum", 1);
        Meatdrop.Add("Squirrel", 1);
        Meatdrop.Add("Wolve", 3);
        Meatdrop.Add("Alpaca", 2);




        //*****************************************************************//
        //  HIER Federdrop EINFÜGEN                                        //
        //*****************************************************************//

        Featherdrop = new Dictionary<string, int>();
        Featherdrop.Add("Chicken", 1);

        //*****************************************************************//
        //  HIER Linendrop EINFÜGEN                                        //
        //*****************************************************************//
        Linendrop = new Dictionary<string, int>();
        Linendrop.Add("Crawler", 1);

        //*****************************************************************//
        //  HIER Eggdrop EINFÜGEN                                        //
        //*****************************************************************//
        Eggdrop = new Dictionary<string, int>();
        Eggdrop.Add("Chicken", 1);

        //*****************************************************************//
        //  HIER Wooldrop EINFÜGEN                                        //
        //*****************************************************************//
        Wooldrop = new Dictionary<string, int>();
        Wooldrop.Add("Alpaca", 3);
    }

    // Update is called once per frame
    void Update()
    {
        Hungertimer(0);
        MoveRandom();
        MovetoPlayer();
        SearchItem();
        Hide();
        Death();


        //waittimer

        waittimer -= Time.deltaTime;

        // Position erreicht

        if (this.transform.position == Targetposition && (moverandom || searchitem))
        {
            Direction();
        }

        //Random
        if (!searchitem && !hiding && !death && !movetoplayer && !sleeping && waittimer <= 0)
        {
            moverandom = true;
        }
        else { moverandom = false; }

        // MovetoPlayer (Angreifen)
        if(movetoplayer)
        {
            var spritetake = GetComponent<SpriteRenderer>();
            spritetake.sortingOrder = 12;
        }
        else
        {
            var spritetake = GetComponent<SpriteRenderer>();
            spritetake.sortingOrder = 1;
        }
     
        // Search Item
        if (!death && !hiding && !movetoplayer && currentHunger < 20)
        {
            searchitem = true;
        }

        // Hide
        if (currentHealth < 20 && distanceH <2)
        {
            hiding = true;
            movetoplayer = false;
            animator.SetBool("attack", false);
        }
        else { hiding = false; }

        // Death
        if (currentHealth <= 0 || currentHunger <= 0)
        {
            death = true;
            searchitem = false;
            hiding = false;
            sleeping = false;
            moverandom = false;
            movetoplayer = false;
            animator.SetBool("attack", false);
            hiding = false;
            attacking = false;
        }
        else { death = false; }

        // Sleep

        if (myNightDayCircel.hour >= 8 && myNightDayCircel.hour < 20)
        {
            sleeping = false;
        }
        if (!death && myNightDayCircel.hour >= 20 && !searchitem)
        {

            sleeping = true;
        }
        if(sleeping && !searchitem)
        {
            animator.SetBool("sleep", true);
        }
        else { animator.SetBool("sleep", false); }

        // Walk Animation------------------------------------------------

        if (!sleeping && !attackrange && (moverandom || movetoplayer || searchitem || hiding))
        {
            animator.SetBool("walk", true);
            speed = 0.5f;
        }
        else
        {
            animator.SetBool("walk", false);
        }

        // Charakter drehen  (flip) -------------------------------------

        ThisX = this.transform.position.x;
        ThisY = this.transform.position.y;

        if (setleft == true)
        { this.transform.localScale = new Vector3(1, 1, 1); }

        else { this.transform.localScale = new Vector3(-1, 1, 1); }

        if (placeholderX < ThisX)
        { setleft = true; }
        if (placeholderX > ThisX)
        { setleft = false; }


    }
    // Update Ende --------------------------------------------------------------------------------------

    void MoveRandom()
    {
        if (moverandom == true && !sleeping)
        {
            speed = 0.5f;

                Targetposition = new Vector3(placeholderX,placeholderY, 0);
                float step = speed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, Targetposition, step);
        }
    }

    void MovetoPlayer()
    {
        if (movetoplayer == true)
        {
            speed = 0.5f;
            Targetposition = Attacker.transform.position;
            if(this.transform.position.x > Attacker.transform.position.x)
            { Targetposition += new Vector3(+0.15f, -0.2f, 0);
                setleft = true;
            }
            if (this.transform.position.x < Attacker.transform.position.x)
            { Targetposition += new Vector3(-0.15f, -0.2f, 0);
                setleft = false;
            }
            distance = Vector2.Distance(this.transform.position, Attacker.transform.position);
            if (!death && distance <= 0.251f)
            {
                attackrange = true;
                animator.SetBool("attack", true);
            }
            else
            {
                attackrange = false;
                animator.SetBool("attack", false);
            }
             
            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, Targetposition, step);
        }

    }

    void SearchItem()
    {
        if (searchitem == true)
        {
            speed = 0.5f;
            // Items in Array packen und kürzeste Distanz ermitteln und das Object in Item schreiben
            Collider2D[] arrayofItems = Physics2D.OverlapCircleAll(transform.position, 1.5f, layermask);
            float distancetemp = 1.5f;
            foreach (Collider2D ItemTemp in arrayofItems)
            {
                float distance = Vector2.Distance(this.transform.position, ItemTemp.transform.position);
                if (distance < distancetemp)
                {
                    distancetemp = distance;
                    Item = ItemTemp.gameObject;
                }
            }
            // Wenn Item gefunden dann dieses ansteuern, ansonsten Random laufen
            if (Item != null)
            {
                Targetposition = Item.transform.position;

                float step = speed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, Targetposition, step);
            }
            else
            {
                Targetposition = new Vector3(placeholderX,placeholderY, 0);
                float step = speed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, Targetposition, step);
            }
        }
        // Wenn satt SearchItem Modus verlassen
        if (currentHunger > 30)
        {
            searchitem = false;
        }
        if(Item != null && transform.position == Item.transform.position)
        {
            Items myItems = Item.GetComponent<Items>();
           // int value = myItems.essenswerte[Item.name];
            int value = 0;
            myItems.animalfood.TryGetValue(Item.name, out value);
            if (value > 0)
            {
                this.SendMessage("Hungertimer", value);
                Destroy(Item);
                Item = null;
                value = 0;
            }
        }


    }

    void Hide()
    {
        if (hiding == true && distanceH < 2)
        {
            attacking = false;
            speed = 1;
            Targetposition = Attacker.transform.position;
            distanceH = Vector2.Distance(this.transform.position, Attacker.transform.position);
            if (this.transform.position.x > Attacker.transform.position.x)
            {
                Targetposition += new Vector3(2, 0, 0);
                setleft = false;
                animator.SetBool("run", true);
            }
            if (this.transform.position.x < Attacker.transform.position.x)
            {
                Targetposition += new Vector3(-2, 0, 0);
                setleft = true;
                animator.SetBool("run", true);
            }

            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, Targetposition, step);
        }
        if(distanceH >= 2)
        {
            animator.SetBool("run", false); 
            hiding = false;
            attackrange = false;
        }
    }

    void Death()
    {
        if (death == true)
        {
            animator.SetBool("dead", true);
           
            disappeartimer -= Time.deltaTime;
            if (disappeartimer <= 0)
            {
                Itemdrop();
                Destroy(transform.gameObject);
            }
        }
    }

    // Zielposition auswürfeln ---------------------------------------------
    void Direction()
    {      
        waittimer = Random.Range(0, 10);
        placeholderX = Random.Range(-myMapgenerator.mapWidth / 10, myMapgenerator.mapWidth  /10);
        placeholderY = Random.Range(-myMapgenerator.mapHeight / 10, myMapgenerator.mapHeight /10);
    }
    //----------------------------------------------------------------------

    // Damage
    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        Color tmp = this.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        Invoke("resetDamageColor", 0.2f);
        if(!death)
        movetoplayer = true;
    }
    //-----------------------------------------------------------------------

    // Hungertimer
    private void Hungertimer(float hunger)
    {
        currentHunger += hunger;
        currentHunger -= Time.deltaTime * 0.05f;
        if (currentHunger > 100)
            currentHunger = 100;
        if (currentHunger < 0)
            currentHunger = 0;
    }
    //-----------------------------------------------------------------------

    // Damage from Weapon
    void OnTriggerEnter2D(Collider2D Weapon)
    {
        if(Weapon.GetComponent<AnimalCtrl>() != null)
        {
            Attacker = Weapon.transform.root.gameObject;
        }
        //(Weapon.CompareTag("weapon")
        if(myCtrl.attack == true && Weapon.CompareTag("weapon") && Weapon is PolygonCollider2D && !death)
        {
            Attacker = Charakter;
            Items myWeapon = Weapon.GetComponent<Items>();
            int value = 0;
            if(myWeapon.Weapon.TryGetValue(Weapon.name, out value))
            { 
                TakeDamage(value);
            }
        }
        
    }

    // Rote Damage Anzeige zurücksetzen
    void resetDamageColor()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    void SetAttackingtrue()
    {
        attacking = true;
    }
    void SetAttackingfalse()
    {
        attacking = false;
    }

    // Item drop
    void Itemdrop()
    {
        int hide;
        int meat;
        int feather;
        int linen;
        int egg;
        int wool;

        if (Hidedrop.TryGetValue(this.name, out hide))
        { fellcounter = hide;   }
        if (Meatdrop.TryGetValue(this.name, out meat))
        { fleischcounter = meat; }
        if (Featherdrop.TryGetValue(this.name, out feather))
        { federcounter = feather; }
        if (Linendrop.TryGetValue(this.name, out linen))
        { linencounter = linen; }
        if (Eggdrop.TryGetValue(this.name, out egg))
        { eggcounter = egg; }
        if (Wooldrop.TryGetValue(this.name, out wool))
        { woolcounter = wool; }


        if (fleischcounter !=0)
            InstantItem("Meat", fleischcounter);
        if (fellcounter != 0)
            InstantItem("Hide", federcounter);
        if (federcounter != 0)
            InstantItem("Feather", federcounter);
        if (linencounter != 0)
            InstantItem("Linen", linencounter);
        if (eggcounter != 0)
            InstantItem("Egg", eggcounter);
        if (woolcounter != 0)
            InstantItem("Wool", woolcounter);
    }

    public void InstantItem(string name, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float placeX = Random.Range(-0.2f, 0.2f);
            float placeY = Random.Range(-0.2f, 0.2f);
            GameObject Spawn = Instantiate(Prefabliste.Instance().GetGameObject(name), new Vector3(0, 0, 0), Quaternion.identity);
            Spawn.transform.position = this.transform.position;
            Spawn.transform.position += new Vector3(placeX, placeY, 0);
            Spawn.name = name;
        }
    }

}

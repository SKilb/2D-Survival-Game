using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    private Items myItems;
    public Ctrl myCtrl;
    public Inventar myInventar;
    Animator animatorRod;
    private GameObject Charakter;
    private GameObject Rightarm;
    private GameObject Leftarm;
    private GameObject Inventar;
    public GameObject SoundManager;
    private int childcounterR;
    private int childcounterL;
    public int childcounterH;
    public int fishIndex;
    public float Ftimer = 2.1f;
    public bool Ftiming;
    public bool isfishable = false;
    private bool EisUp = false;
    public GameObject Haken;
    private Transform Child;
    public float waitonbitetimer = 1;
    public float reactiontimer = 2f;
    public float takefishtimer =1f;
    public  List<string> Fishsorts;
    public bool takefish;


    // Use this for initialization
    void Start()
    {
        SoundManager = GameObject.Find("SoundManager");
        Charakter = GameObject.Find("Charakter");
        Inventar = GameObject.Find("Inventar");
        myCtrl = Charakter.GetComponent<Ctrl>();
        myInventar = Inventar.GetComponent<Inventar>();

        // Mit Animator verbinden --------------------------------------------------------
        animatorRod = this.GetComponent<Animator>();
        Rightarm = GameObject.Find("Rightarm");
        Leftarm = GameObject.Find("Leftarm");
        Charakter = GameObject.Find("Charakter");
        Fishsorts = new List<string>();
        Fishsorts.Add("Carp");
        Fishsorts.Add("Can");
        Fishsorts.Add("Fishbone");
        Fishsorts.Add("Bottlepost");
        Fishsorts.Add("Frog");
        Fishsorts.Add("Snail");
        Fishsorts.Add("Lobster");
        Fishsorts.Add("Blowfish");
        Fishsorts.Add("Lampfish");
        Fishsorts.Add("Shell");
        Fishsorts.Add("Rainbowtrout");
        Fishsorts.Add("Ink");
        Fishsorts.Add("Octopus");
    }

    // Update is called once per frame
    void Update()
    {
        TakeFishfromRod();
        if (this.transform.parent == Rightarm.transform)
        {
            childcounterH = Haken.transform.childCount;
            childcounterL = Leftarm.transform.childCount;
            if (childcounterH > 0)
            {
                myCtrl.childcounterH = 1;
            }
            else { myCtrl.childcounterH = 0; }

            Schwimmeranimation();

            if (reactiontimer >= 0 && reactiontimer < 2f && (Input.GetMouseButtonUp(0))
                && isfishable == true)
            {
                GetFish();
            }
            if (Input.GetMouseButton(0) && childcounterH == 0 && childcounterL == 0)
            {
                Ftiming = true;
                Fishing();
            }
            else
            {
                Ftiming = false;
                animatorRod.SetBool("fishingrod", false);
                Ftimer = 2.10f;
                reactiontimer = 2f;
            }
            // Sounds abspielen
            if (reactiontimer > 1.9f && reactiontimer < 2.0f && isfishable == true)
            {
                SoundManager.SendMessage("PlaySound", "fishstruggle");
            }
            // Fish von Angel in linken Arm nehmen
            if (Input.GetMouseButton(1))
            {
                myCtrl.GetFishfromRod();

            }
            else { myCtrl.BreakGetFishfromRod(); }
                

            // Fisch von linken Arm in Inventar legen

            if (myCtrl.takefish)             
            {
                myInventar.TakeFishtoInventory();
            }               
        }
    }
    public void Fishing()
    {
        childcounterR = Rightarm.transform.childCount;
        if (childcounterR > 0)
        {
            Transform ChildR = Rightarm.gameObject.transform.GetChild(0);
            if(ChildR.name.Contains("Rod"))
            {
                animatorRod.SetBool("fishingrod", true);
            }
            if (Ftiming == true)
            {
                Ftimer -= Time.deltaTime;
                if (Ftimer < 0)
                {
                    Ftiming = false;
                    if (isfishable == false)
                    {
                        animatorRod.SetBool("fishingrod", false);
                    }
                    Ftimer = 0;
                }
                if (Ftimer <= 0)
                {
                    waitonbitetimer -= Time.deltaTime;

                    if (waitonbitetimer <= 0)
                    {
                        reactiontimer -= Time.deltaTime;
                    }
                }
            }
        }
    }

    public void GetFish()
    {
        string name = Fishsorts[fishIndex];
        GameObject Crafted = Instantiate(Prefabliste.Instance().GetGameObject(name), new Vector3(0, 0, 0), Quaternion.identity);
        Crafted.name = name;
        Crafted.transform.SetParent(Haken.transform);
        Crafted.transform.localPosition = Haken.transform.localPosition;
        Crafted.transform.eulerAngles = new Vector3(0, 0, 225);
        var spritetake = Crafted.GetComponent<SpriteRenderer>();
        spritetake.sortingOrder = 11;
        reactiontimer = 2f;
        myItems = this.GetComponent<Items>();
        myItems.Childlist.Add(Crafted);
    }

    public void Schwimmeranimation()
    {      
            if (reactiontimer > 0f && reactiontimer < 2f)
                animatorRod.SetBool("schwimmer", true);
            else
                animatorRod.SetBool("schwimmer", false);       
    }

    public void PlayThrow()
    {
        SoundManager.SendMessage("PlaySound", "throw");
    }

    public void TakeFishfromRod()
    {
        if (childcounterL == 0 && childcounterH > 0 && myCtrl.takefish)
        {
            Child = Haken.gameObject.transform.GetChild(0);
            Child.transform.SetParent(Leftarm.transform);
            myItems.Childlist.Remove(Child.gameObject);
        }

    }

    public void ActivateHaken()
    {
        Haken.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void DeActivateHaken()
    {
        Haken.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}

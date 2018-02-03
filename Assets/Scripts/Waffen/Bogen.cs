using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bogen : MonoBehaviour {

    private Ctrl myCtrl;
    private GameObject Charakter;
    private GameObject Leftarm;
    public GameObject ActionbarPanel;
    public GameObject SlotPanel;
    public GameObject Rightarm;
    public GameObject Pfeilaufnahme;
    private GameObject Mouse;
    private GameObject Pfeil;
    private Transform ChildL;
    private Vector3 Targetposition;
    public Sprite TargetSprite;
    public int childcounterL;
    public int childcounterP;
    public bool bowing;
    public bool bowthing = false;
    public Animator animator;


    // Use this for initialization
    void Start ()
    {
        Charakter = GameObject.Find("Charakter");
        myCtrl = Charakter.GetComponent<Ctrl>();
        Leftarm = GameObject.Find("Leftarm");
        Rightarm = GameObject.Find("Rightarm");
        Mouse = GameObject.Find("Mouse");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        childcounterL = Leftarm.transform.childCount;
        childcounterP = Pfeilaufnahme.transform.childCount;

        // Bogen laden und Spannen
        if (childcounterL > 0)
        {
            ChildL = Leftarm.gameObject.transform.GetChild(0);
            if (ChildL.name.Contains("Bow") && Input.GetMouseButton(1))
            {
                SearchArrow(Rightarm);
                SearchArrow(ActionbarPanel);
                SearchArrow(SlotPanel);
                if (Pfeil != null)
                {
                    myCtrl.pfeil = true;

                    if (childcounterP == 0 && !bowing)
                    {               
                        Pfeil.transform.SetParent(Pfeilaufnahme.transform);
                        Pfeil.transform.localScale = new Vector3(1, 1, 1);
                        Pfeil.transform.localPosition = new Vector3(0, 0, 0);
                    }
                        var spritetake = ChildL.GetComponent<SpriteRenderer>();
                        spritetake.sortingOrder = 8;
                        var spritetakeP = Pfeil.GetComponent<SpriteRenderer>();
                        Pfeil.GetComponent<SpriteRenderer>().enabled = true;
                        spritetakeP.sortingOrder = 8;                   
                        animator.SetBool("bogen", true);
                        this.transform.eulerAngles = new Vector3(0, 0, 0);
                        if(childcounterP>0)
                        Pfeil.transform.localEulerAngles = new Vector3(0, 0, 0);                   
                }
            }
            else
            {
                myCtrl.animator.SetBool("bogen", false);
                myCtrl.pfeil = false;
                bowthing = false;
                animator.SetBool("bogen", false);
                Mouse.GetComponent<SpriteRenderer>().sprite = null;
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                var spritetake = ChildL.GetComponent<SpriteRenderer>();
                spritetake.sortingOrder = 4;
                if (Pfeil != null)
                {
                    var spritetakeP = Pfeil.GetComponent<SpriteRenderer>();
                    spritetakeP.sortingOrder = 4;
                }
               // if (childcounterP == 0)
              //  Pfeil = null;
            }
        }

        //Bogenschießen
        if (childcounterP > 0 && bowthing == true && Input.GetMouseButtonUp(0))
        {
            myCtrl.attack = true;
            Vector3 mousePos = Input.mousePosition;
            float x = mousePos.x;
            float y = mousePos.y;
            mousePos = new Vector3(x, y, +5);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Targetposition = mousePos;
            bowing = true;
            animator.SetBool("bogen", false);

            Vector3 difference = Targetposition - Pfeil.transform.position;
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Pfeil.transform.eulerAngles = new Vector3(0, 0, rotation_z);
            if (Targetposition.x > this.transform.position.x)
            {
                Pfeil.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
            }
            else { Pfeil.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z+180); }
        }
        // Fadenkreuz der Maus anzeigen
        if (bowthing == true)
        {
            Mouse.GetComponent<SpriteRenderer>().sprite = TargetSprite;
            var spritetake = Mouse.GetComponent<SpriteRenderer>();
            spritetake.sortingOrder = 1;
        }

        if (bowing)
        {
            // attack = true;
            Pfeil.transform.SetParent(null);
            Pfeil.GetComponent<Collider2D>().enabled = true;
            bowthing = false;
            if (Pfeil.transform.position != Targetposition)
            {
                float speedobj = 5f;
                float step = speedobj * Time.deltaTime;
                Pfeil.transform.position = Vector2.MoveTowards(Pfeil.transform.position, Targetposition, step);
            }
            if (Pfeil.transform.position == Targetposition)
            { 
                myCtrl.attack = false;
                bowing = false;
                Pfeil = null;
               // attack = false;
            }

        }

    }

    private void SearchArrow(GameObject Panel)
    {
        int childcounterR = Rightarm.transform.childCount;
        if(childcounterR >0)
        {
            Transform ChildR = Rightarm.gameObject.transform.GetChild(0);
            if (ChildR.name.Contains("Arrow") && Pfeil == null)
                Pfeil = ChildR.gameObject;
        }

        foreach (Transform Obj in Panel.transform) // Actionbarpanel
        {
            // Obj = ActionSlot
            int childcounterObj = Obj.transform.childCount;
            if (childcounterObj > 0)
            {
                // ChildObj = ActionItemSlot
                Transform ChildObj = Obj.gameObject.transform.GetChild(0);

                int childcounterChildObj = ChildObj.transform.childCount;
                if (childcounterChildObj > 1)
                {
                    Transform Item = ChildObj.gameObject.transform.GetChild(1);
                    if (childcounterP > 0 && Pfeil != null)
                        Pfeil = Pfeilaufnahme.gameObject.transform.GetChild(0).gameObject;
                    if (Item.name.Contains("Arrow") && Pfeil ==null)
                        Pfeil = Item.gameObject;
                }
            }

        }
    }

    public void StartBow()
    {
        bowthing = true;
    }
    public void EndBow()
    {
        bowthing = false;
    }
}

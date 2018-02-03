using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour {

    private GameObject PlayerCursor;
    private GameObject Mouse;
    public Sprite BuildCursor;
    public bool buildbool;
    

	// Use this for initialization
	void Start ()
    {
        PlayerCursor = GameObject.Find("PlayerCursor");
        Mouse = GameObject.Find("Mouse");
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Baumenü aktivieren -----------------------------------------------------------------

        int childcounterM = Mouse.transform.childCount;
        int childcounterP = PlayerCursor.transform.childCount;

        // Aktiviert
        if (Input.GetKeyDown(KeyCode.B) && buildbool == false)
        {
            // Sprite Cursor setzen
            PlayerCursor.GetComponent<SpriteRenderer>().sprite = BuildCursor;
            buildbool = true;
        }
        // Deaktiviert
        else if (Input.GetKeyDown(KeyCode.B) && buildbool == true)
        {
            // Sprite Cursor auf null setzen
            PlayerCursor.GetComponent<SpriteRenderer>().sprite = null;
            // Cursor wieder bei Charakter positionieren
            PlayerCursor.transform.localPosition = new Vector3(0, 0);
            buildbool = false;
            if(childcounterP > 0)
            {
                Transform PlayerCursorChild = PlayerCursor.gameObject.transform.GetChild(0);
                PlayerCursorChild.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                PlayerCursorChild.transform.SetParent(Mouse.transform);           
            }

        }
        // Item von Drag and Drop zu PlayerCursor bei aktiviertem Baumenü
        if (childcounterM > 0 && buildbool == true)
        {
            Transform Mousechild = Mouse.gameObject.transform.GetChild(0);
            Mousechild.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            var spritetake = Mousechild.GetComponent<SpriteRenderer>();
            spritetake.sortingOrder = 1;
            Mousechild.transform.SetParent(PlayerCursor.transform);
            Mousechild.transform.localPosition = new Vector3(0, 0);
            Mousechild.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        // Item auf Raster setzen
        if(buildbool == true && childcounterP >0)
        {
            Transform PlayerCursorChild = PlayerCursor.gameObject.transform.GetChild(0);
            float posx = Mouse.transform.position.x;
            float posy = Mouse.transform.position.y;
            float rposx = posx - (posx % 0.16f);
            float rposy = posy - (posy % 0.16f);
            PlayerCursorChild.transform.position = new Vector2(rposx, rposy);
        }

        // Cursor = Mouse Position setzen ---------------------------------------------------------
        if (buildbool == true)
        {
            // Cursor = Mouse Position setzen 
            Vector3 mousePos = Input.mousePosition;
            float x = mousePos.x;
            float y = mousePos.y;
            mousePos = new Vector3(x, y, +5);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            float posx = mousePos.x;
            float posy = mousePos.y; 
            float rposx = posx-(posx % 0.16f);
            float rposy = posy-(posy % 0.16f);
            PlayerCursor.transform.position = new Vector2(rposx, rposy);
        }

        // Platzieren
        if (childcounterP > 0 && Input.GetMouseButtonDown(0))
        {
            Transform PlayerCursorChild = PlayerCursor.gameObject.transform.GetChild(0);
            var spritetake = PlayerCursorChild.GetComponent<SpriteRenderer>();
            spritetake.sortingOrder = 1;
            PlayerCursorChild.GetComponent<Collider2D>().enabled = true;
            ActivateSpriteChilds(PlayerCursorChild.gameObject);
            PlayerCursorChild.transform.SetParent(null);
           // PlayerCursorChild = null;
        }
    }

    public void ActivateSpriteChilds(GameObject Parent)
    {
        int childcounterP = Parent.transform.childCount;

        if (childcounterP > 0)
        {
            for (int i = 0; i < childcounterP; i++)
            {
                Transform Child = Parent.transform.GetChild(i);
                if (Child.GetComponent<SpriteRenderer>() != null)
                {
                    Child.GetComponent<SpriteRenderer>().enabled = true;
                    ActivateSpriteChilds(Child.gameObject);
                }
            }
        }
    }
}

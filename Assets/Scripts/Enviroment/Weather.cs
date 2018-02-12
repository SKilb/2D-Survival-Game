using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weather : MonoBehaviour
{


    public NightDayCircel myNightDayCircel;
    public GameObject NightDayCircel;
    public GameObject Rain;
    public GameObject Snow;
    public int yesorno = 0;
    public bool raining;

    // Use this for initialization
    void Start()
    {
        myNightDayCircel = NightDayCircel.GetComponent<NightDayCircel>();
        Rain.SetActive(false);
        Snow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Schnee auslösen
        if(myNightDayCircel.day % 90 == 0 && myNightDayCircel.day !=0)
        {
            Snow.SetActive(true);
        }
        else { Snow.SetActive(false); }

        // Regen auslösen
        if (myNightDayCircel.hour > 21 && myNightDayCircel.hour <23)
        {
            yesorno = Random.Range(0, 2);
        }
        if(yesorno >0)
        {
            raining = true;
        }
        else { raining = false; }
        if(raining)
        {
            Rain.SetActive(true);
        }


    }
}

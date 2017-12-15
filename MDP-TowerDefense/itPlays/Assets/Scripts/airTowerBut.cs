using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class airTowerBut : MonoBehaviour {

    bool activateAir;
    Text text;
    string og;
    GameObject baseTower;
    // Use this for initialization
    void Start () {
        activateAir = false;
        text = GetComponentInChildren<Text>();
        baseTower = GameObject.FindGameObjectWithTag("base");
        og = text.text;
    }

    public void setAirFalse()
    {
        activateAir = false;
        text.text = og;
    }
    public void setAirTrue()
    {
        if (baseTower.GetComponent<baseScript>().coins > 100)
        {
            activateAir = true;
            text.text = "click to set location";
        }
        else
        {
            text.text = "Not enough coins";
        }
    }
    public bool GetAir()
    {
        return activateAir;
    }
}

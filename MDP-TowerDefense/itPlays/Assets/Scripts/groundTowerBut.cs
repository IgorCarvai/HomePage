using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class groundTowerBut : MonoBehaviour {
    bool activateGround;
    Text text;
    string og;
    GameObject baseTower;
    // Use this for initialization
    void Start()
    {
        activateGround = false;
        baseTower = GameObject.FindGameObjectWithTag("base");
        text = GetComponentInChildren<Text>();
        og = text.text;
    }

    public void setGroundFalse()
    {
        activateGround = false;
        text.text = og;
    }
    public void setGroundTrue()
    {
        if (baseTower.GetComponent<baseScript>().coins > 100)
        {
            activateGround = true;
            text.text = "click to set location";
        }
        else
        {
            text.text = "Not enough coins";
        }

    }
    public bool GetGround()
    {
        return activateGround;
    }
}

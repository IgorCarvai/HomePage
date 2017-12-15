using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseOver : MonoBehaviour {

    MapGen mapgen;
    List<char[]> map;

    public GameObject airTowerBut;
    public GameObject groundTowerBut;

    public GameObject airTower;
    public GameObject groundTower;

    public GameObject baseTower;

    GameObject start_tile;
    GameObject end_tile;

    void Start()
    {
        mapgen = GetComponent<MapGen>();
        map = mapgen.map;
        baseTower = GameObject.FindGameObjectWithTag("base");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (GetComponent<Collider>().Raycast(ray, out hitinfo, Mathf.Infinity))
            handleInput(hitinfo);
    }

    public void handleInput(RaycastHit hitinfo)
    {

        int x = Mathf.FloorToInt(hitinfo.point.x / mapgen.tileSize);
        int y = Mathf.FloorToInt(hitinfo.point.y / mapgen.tileSize);
        if (x - 1 < 0)
            x = 1;
        else if ((x - 1) % 3 == 2)
            x += 1;
        else if ((x - 1) % 3 == 1)
            x -= 1;

        if (y - 1 < 0)
            y = 1;
        else if ((y - 1) % 3 == 2)
            y += 1;
        else if ((y - 1) % 3 == 1)
            y -= 1;

        
        if (Input.GetMouseButtonDown(0))
        {
            if (airTowerBut.GetComponent<airTowerBut>().GetAir())
            {

                start_tile = Instantiate(airTower, new Vector3(x + mapgen.tileSize / 2f, y + mapgen.tileSize / 2f, -2), new Quaternion(0, 0, 0, 0));
                baseTower.GetComponent<baseScript>().spawnTower();
                airTowerBut.GetComponent<airTowerBut>().setAirFalse();
            }
            else if (groundTowerBut.GetComponent<groundTowerBut>().GetGround())
            {

                end_tile = Instantiate(groundTower, new Vector3(x + mapgen.tileSize / 2f, y + mapgen.tileSize / 2f, -2), new Quaternion(0, 0, 0, 0));
                baseTower.GetComponent<baseScript>().spawnTower();
                groundTowerBut.GetComponent<groundTowerBut>().setGroundFalse();
            }
        }
    }
}

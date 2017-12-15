using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionScript1 : MonoBehaviour
{

    public bool playerControl = true;
    float speed = 5f;
    int steps;
    int yDir;
    int xDir;
    Vector3 dir;
    Vector3 Target;
    float counter;
    int path;
    bool blockedleft = false;
    bool blockedright = false;
    bool blockedup = false;
    int path2;
    // Use this for initialization
    void Start()
    {
        counter = 0;
        Target = new Vector3(0, 60, 0);
        path = Random.Range(0, 10);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerControl)
        {
            this.transform.position += speed * Time.deltaTime * (new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0));
        }
        else
        {
            if (counter > 1)
            {
                path = Random.Range(1, 11);
                counter = 0;
            }
            counter += Time.deltaTime;
            if (!blockedleft && !blockedright && !blockedup)
            {
                if (path <= 4)
                {
                    //40% chance of 80% going up
                    Vector3 targetDirection = new Vector3(this.transform.position.x, this.transform.position.y + 50, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
                else if (path <= 8)
                {
                    //50% chance of 80% going left
                    Vector3 targetDirection = new Vector3(this.transform.position.x - 50, this.transform.position.y, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
                else
                {
                    //20% chance or going right
                    Vector3 targetDirection = new Vector3(this.transform.position.x + 50, this.transform.position.y, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
            }
            else if (blockedleft && !blockedright && !blockedup)
            {
                if (path <= 8)
                {
                    //80% chance of going up
                    Vector3 targetDirection = new Vector3(this.transform.position.x, this.transform.position.y + 50, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
                else
                {
                    //20% chance or going right
                    Vector3 targetDirection = new Vector3(this.transform.position.x + 50, this.transform.position.y, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
            }
            else if (!blockedleft && blockedright && !blockedup)
            {
                if (path <= 3)
                {
                    //30% chance of going up
                    Vector3 targetDirection = new Vector3(this.transform.position.x, this.transform.position.y + 50, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
                else
                {
                    //70% chance or going left
                    Vector3 targetDirection = new Vector3(this.transform.position.x - 50, this.transform.position.y, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
            }
            else if (!blockedleft && blockedright && blockedup)
            {
                //100% chance or going left
                Vector3 targetDirection = new Vector3(this.transform.position.x - 50, this.transform.position.y, this.transform.position.z) - this.transform.position;
                this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
            }
            else if (!blockedleft && !blockedright && blockedup)
            {
                if (path <= 8)
                {
                    //80% chance of going left
                    Vector3 targetDirection = new Vector3(this.transform.position.x - 50, this.transform.position.y, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
                else
                {
                    //20% chance or going right
                    Vector3 targetDirection = new Vector3(this.transform.position.x + 50, this.transform.position.y, this.transform.position.z) - this.transform.position;
                    this.transform.position += Vector3.Normalize(targetDirection) * speed * Time.deltaTime;
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        checkblock();
    }
    void checkblock()
    {
        if (this.transform.position.x < 3)
        {
            blockedleft = true;
        }
        else
        {
            blockedleft = false;
        }
        if (this.transform.position.y > 57)
        {
            blockedup = true;
        }
        else
        {
            blockedup = false;
        }
        if (this.transform.position.x > 57)
        {
            blockedright = true;
        }
        else
        {
            blockedright = false;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gtScript : MonoBehaviour {

    public GameObject bullet;
    public int chasing = 0;
    public Transform child;
    public Transform enemy;
    public int fire = 0;
    public float lastUpdate = 0f;
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void FixedUpdate () {
        if (Time.time - lastUpdate >= 1f && fire>0 )
        {
            fire -= 1;
            lastUpdate = Time.time;

        }

        if (chasing==0) {
            child.Rotate(new Vector3(0, 0, -1));
        }
        else
        {
            if (enemy)
            {
                Vector3 difference = enemy.position - transform.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                child.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);

                if (fire == 0)
                {
                    shoot(Quaternion.Euler(0.0f, 0.0f, rotationZ - 90), difference);
                    fire = 3;
                }
            }
        }
        /*
         * asd
         * https://answers.unity.com/questions/1023987/lookat-only-on-z-axis.html
         */

	}

    void shoot(Quaternion q, Vector3 diff)
    {
        var bul = (GameObject)Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y,-2), q);
        bul.GetComponent<groundbulletScript>().dir = diff*.025f;
        Destroy(bul, 2.0f);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "GM")
        {
            enemy = coll.gameObject.transform;
            chasing++;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "GM")
        {
            chasing--;
        }
    }
}

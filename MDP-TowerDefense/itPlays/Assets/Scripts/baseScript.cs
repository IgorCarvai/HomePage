using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class baseScript : MonoBehaviour {

    public int health;
    public int coins;
    float lastUpdate;
    public Text hp;
    public Text cointcount;
    public GameObject GT;
    public GameObject AT;
    List<Vector2> groundTowers;
    List<Vector2> airTowers;
    List<Vector2> groundMinions;
    List<Vector2> airMinions;
    Vector2 baseLoc;
    float speed;
    int count;

    List<Vector2> possibleTowers;


    // Use this for initialization
    void Start () {
        groundTowers = new List<Vector2>();
        airTowers = new List<Vector2>();
        groundMinions = new List<Vector2>();
        airMinions = new List<Vector2>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GM"))
        {
            groundMinions.Add(obj.transform.position);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AM"))
        {
            airMinions.Add(obj.transform.position);
        }

        count = 0;
        speed = 10 * Time.deltaTime;
        health = 100;
        coins = 100;
        baseLoc = this.gameObject.transform.position;

        findGTsATs();
        possibleTowers = new List<Vector2>();
        for(int i =6; i<=54; i = i + 12)
        {
            for (int j = 6; j <= 54; j = j + 12)
            {
                if (i == 6 && j == 54)
                    continue;
                possibleTowers.Add(new Vector2(i, j));
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        groundMinions = new List<Vector2>();
        airMinions = new List<Vector2>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GM"))
        {
            groundMinions.Add(obj.transform.position);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AM"))
        {
            airMinions.Add(obj.transform.position);
        }

        if (count == 30)
        {
            Take_Action();
            count = -1;
        }
        count++;
        //findGTsATs();
        //findGMsAMs();
        UpdateGui();
    }

    void Take_Action()
    {
        //towers cost 100 gold, cant place if too poor
        if (coins < 100)
            return;

        //loop through all possible actions

        
        List<double> movePerc = new List<double>
        {
            .4,//left
            .4,//up
            .2//right
        };
        List<Vector2> movements = new List<Vector2>
        {
            new Vector2(-speed, 0),//left
            new Vector2(0, speed),//up
            new Vector2(speed, 0)//right
        };

        int current_state_index = 0;
        List<int> indx = new List<int>(new int[groundMinions.Count]);

        List<List<Vector2>> allGroundMinionLists = new List<List<Vector2>>();
        List<double> groundProbs = new List<double>();
        while (groundMinions.Count > 0 && indx[0] < 3)
        {
            List<Vector2> temp = new List<Vector2>(groundMinions);
            groundProbs.Add(0);
            for (int i = 0; i < groundMinions.Count; i++)
            {
                temp[i] = temp[i] + movements[indx[i]];
                if (groundProbs[current_state_index] == 0)
                    groundProbs[current_state_index] = movePerc[indx[i]];
                else
                    groundProbs[current_state_index] *= movePerc[indx[i]];
            }

            int curr_ind = groundMinions.Count - 1;

            while (indx[curr_ind] > 1 && curr_ind > 0)
            {
                indx[curr_ind] = 0;
                curr_ind--;
            }
            indx[curr_ind]++;

            allGroundMinionLists.Add(temp);
        }

        current_state_index = 0;
        indx = new List<int>(new int[airMinions.Count]);

        List<List<Vector2>> allAirMinionLists = new List<List<Vector2>>();
        List<double> airProbs = new List<double>();
        while (airMinions.Count > 0 && indx[0] < 3)
        {
            List<Vector2> temp = new List<Vector2>(airMinions);
            airProbs.Add(0);
            for (int i = 0; i < airMinions.Count; i++)
            {
                temp[i] = temp[i] + movements[indx[i]];
                if (airProbs[current_state_index] == 0)
                    airProbs[current_state_index] = movePerc[indx[i]];
                else
                    airProbs[current_state_index] *= movePerc[indx[i]];
            }

            int curr_ind = airMinions.Count - 1;

            while (indx[curr_ind] > 1 && curr_ind > 0)
            {
                indx[curr_ind] = 0;
                curr_ind--;
            }
            indx[curr_ind]++;

            allAirMinionLists.Add(temp);
        }

        int maxReward = 0;
        Vector2 bestAction = new Vector2();
        int bestType = -1;//0 = ground, 1 = air
        foreach (Vector2 tower_loc in possibleTowers)
        {
            if (!groundTowers.Contains(tower_loc) && !airTowers.Contains(tower_loc))
            {
                List<Vector2> newGroundTowers = new List<Vector2>(groundTowers);
                newGroundTowers.Add(tower_loc);

                if (allGroundMinionLists.Count > 0)
                {
                    foreach (List<Vector2> g_minions in allGroundMinionLists)
                    {
                        if (allAirMinionLists.Count > 0)
                        {
                            foreach (List<Vector2> a_minions in allAirMinionLists)
                            {
                                MDPState state = new MDPState(health, coins + 10, newGroundTowers, airTowers, g_minions, a_minions);
                                int reward = GetReward(state);
                                if (reward > maxReward)
                                {
                                    maxReward = reward;
                                    bestAction = tower_loc;
                                    bestType = 0;
                                }
                            }
                        }
                        else
                        {
                            MDPState state = new MDPState(health, coins + 10, newGroundTowers, airTowers, g_minions, new List<Vector2>());
                            int reward = GetReward(state);
                            if (reward > maxReward)
                            {
                                maxReward = reward;
                                bestAction = tower_loc;
                                bestType = 0;
                            }
                        }
                    }
                }
                else
                {
                    foreach (List<Vector2> a_minions in allAirMinionLists)
                    {
                        MDPState state = new MDPState(health, coins + 10, newGroundTowers, airTowers, new List<Vector2>(), a_minions);
                        int reward = GetReward(state);
                        if (reward > maxReward)
                        {
                            maxReward = reward;
                            bestAction = tower_loc;
                            bestType = 0;
                        }
                    }
                }
            }

            if (!airTowers.Contains(tower_loc) && !groundTowers.Contains(tower_loc))
            {
                List<Vector2> newAirTowers = new List<Vector2>(airTowers);
                newAirTowers.Add(tower_loc);

                if (allGroundMinionLists.Count > 0)
                {
                    foreach (List<Vector2> g_minions in allGroundMinionLists)
                    {
                        if (allAirMinionLists.Count > 0)
                        {
                            foreach (List<Vector2> a_minions in allAirMinionLists)
                            {
                                MDPState state = new MDPState(health, coins + 10, groundTowers, newAirTowers, g_minions, a_minions);
                                int reward = GetReward(state);
                                if (reward > maxReward)
                                {
                                    maxReward = reward;
                                    bestAction = tower_loc;
                                    bestType = 1;
                                }
                            }
                        }
                        else
                        {
                            MDPState state = new MDPState(health, coins + 10, groundTowers, newAirTowers, g_minions, new List<Vector2>());
                            int reward = GetReward(state);
                            if (reward > maxReward)
                            {
                                maxReward = reward;
                                bestAction = tower_loc;
                                bestType = 1;
                            }
                        }
                    }
                }
                else
                {
                    foreach (List<Vector2> a_minions in allAirMinionLists)
                    {
                        MDPState state = new MDPState(health, coins + 10, groundTowers, newAirTowers, new List<Vector2>(), a_minions);
                        int reward = GetReward(state);
                        if (reward > maxReward)
                        {
                            maxReward = reward;
                            bestAction = tower_loc;
                            bestType = 1;
                        }
                    }
                }
            }
        }

        //now take that action
        if (bestType == 0)
        {
            Instantiate(GT, bestAction, new Quaternion(0, 0, 0, 0));
            coins -= 100;
            groundTowers.Add(bestAction);
        }
            
        else if (bestType == 1)
        {
            Instantiate(AT, bestAction, new Quaternion(0, 0, 0, 0));
            coins -= 100;
            airTowers.Add(bestAction);
        }
        else
            print("ERROR: NO ACTIONS AVAILABLE");
    }

    int GetReward(MDPState state)
    {
        int new_coins = coins;
        int new_health = health;
        if (state.groundTowers.Count > groundTowers.Count || state.airTowers.Count > airTowers.Count)
            new_coins -= 100;

        foreach (Vector2 loc in state.groundMinions)
            if (GetComponent<Collider2D>().bounds.Contains(loc))
                health -= 10;
        foreach (Vector2 loc in state.airMinions)
            if (GetComponent<Collider2D>().bounds.Contains(loc))
                health -= 10;

        int score = 0;
        foreach (Vector3 g_tower in state.groundTowers)
        {
            foreach(Vector3 g_min in state.groundMinions)
            {
                if (Vector3.Distance(g_min, g_tower) < 6)
                    score += 100;
            }
        }

        foreach (Vector3 a_tower in state.airTowers)
        {
            foreach (Vector3 a_min in state.airMinions)
            {
                if (Vector3.Distance(a_min, a_tower) < 6)
                    score += 100;
            }
        }
        return score + (new_coins / 100) + (new_health / 10);
    }

    Vector2 furthestSpot()
    {
        return new Vector2(1f, 1f);
    }
    //since minions can be destroyed, we need to remove the ones that are null
    //goes through all the objects in the game and finds the Ground and Air towers and appends them to the appropriate lists
    void findGTsATs()
    {
        foreach(GameObject tower in GameObject.FindGameObjectsWithTag("GT"))
        {
            if (!groundTowers.Contains(tower.transform.position))
            {
                groundTowers.Add(tower.transform.position);
            }
        }
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("AT"))
        {
            if (!airTowers.Contains(tower.transform.position))
            {
                airTowers.Add(tower.transform.position);
            }
        }
    }


    //goes through all the objects in the game and finds the Ground and Air minions and appends them to the appropriate lists
    void findGMsAMs()
    {
        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("GM"))
        {
            if (!groundMinions.Contains(minion.transform.position))
            {
                groundMinions.Add(minion.transform.position);
            }
        }
        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("AM"))
        {
            if (!airMinions.Contains(minion.transform.position))
            {
                airMinions.Add(minion.transform.position);
            }
        }
    }


    public void decreaseHealth()
    {
        health = health-10;
    }
	
    void UpdateGui()
    {    //coins go up 10 at a time
        if (Time.time - lastUpdate >= 1f)
        {
            lastUpdate = Time.time;
            coins = coins + 30;
        }

        hp.text = "HP = " + health;
        cointcount.text = "Gold = " + coins;
    }
  
    public void groundMinionKilled(Vector2 t)
    {
        groundMinions.Remove(t);
        coins = coins + 1;
    }

    public void airMinionKilled(Vector2 t)
    {
        airMinions.Remove(t);
        coins = coins + 1;
    }

    public void spawnTower()
    {
        coins = coins - 100;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        decreaseHealth();

        Destroy(other.gameObject);
    }
}


public class MDPState
{
    public int health;
    public int coins;

    public List<Vector2> groundTowers;
    public List<Vector2> airTowers;
    public List<Vector2> groundMinions;
    public List<Vector2> airMinions;

    public MDPState(int h, int c, List<Vector2> GT, List<Vector2> AT, List<Vector2> GM, List<Vector2> AM)
    {
        health = h;
        coins = c;

        groundTowers = GT;
        airTowers = AT;
        groundMinions = GM;
        airMinions = AM;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteriodSpawner : MonoBehaviour {

    public GameObject[] asteriods;
    private float timer;
    public float interval;
    private int side;
    private int ast;
    private float range;
    GameObject asteriod;
    GameObject asteriodSmall1;
    GameObject asteriodSmall2;

    Dictionary<GameObject, Vector3> astDir;
    public List<GameObject> asts;
    public List<bool> types;
    public List<Vector3> directions;
    Vector3 down;
    Vector3 up;
    Vector3 left;
    Vector3 right;

    // Use this for initialization
    void Start ()
    {
        asteriods = new GameObject[5];
        types = new List<bool>();
        //grabs all the asteriod prefabs
        asteriods = Resources.LoadAll<GameObject>("Prefabs");
        down = new Vector3(0, -.3f, 0);
        up = new Vector3(0, .3f, 0);
        right = new Vector3(.3f, 0, 0);
        left = new Vector3(-.3f, 0, 0);
        asts = new List<GameObject>();
        directions = new List<Vector3>();
        interval = 100;
    }

    // Update is called once per frame
    void Update ()
    {
        //if the timer hits the interval between asteriod spawns, spawns another one
		if(timer % interval == 0)
        {
            side = Random.Range(0, 4);
            ast = Random.Range(0, 5);

            //randomizes the side and exact spot the asteriod appears
            if(side == 0)
            {
                asteriod = Instantiate(asteriods[ast]);
                range = Random.Range(-50f, 50f);
                asteriod.transform.position = new Vector3(range,30, 0);
                asts.Add(asteriod);
                directions.Add(down);
                types.Add(true);
            }
            else if (side == 1)
            {
                asteriod = Instantiate(asteriods[ast]);
                range = Random.Range(-20f, 20f);
                asteriod.transform.position = new Vector3(70, range, 0);
                asts.Add(asteriod);
                directions.Add(left);
                types.Add(true);

            }
            else if (side == 2)
            {
                asteriod = Instantiate(asteriods[ast]);
                range = Random.Range(-50f, 50f);
                asteriod.transform.position = new Vector3(range, -30, 0);
                asts.Add(asteriod);
                directions.Add(up);
                types.Add(true);

            }
            else if (side == 3)
            {
                asteriod = Instantiate(asteriods[ast]);
                range = Random.Range(-20f, 20f);
                asteriod.transform.position = new Vector3(-70, range, 0);
                asts.Add(asteriod);
                directions.Add(right);
                types.Add(true);

            }

        }
        
        //checks for when each asteriod passes off the screen, and deactivates it when it does
        for(int x = 0;x < asts.Count; x++)
        {
            if (asts[x].activeSelf)
            {
                asts[x].transform.position += directions[x];
                if (asts[x].transform.position.x > 80f || asts[x].transform.position.x < -80f || asts[x].transform.position.y > 40f || asts[x].transform.position.y < -40f)
                {
                    asts[x].SetActive(false);
                }
            }
        }

        timer++;

	}

    /// <summary>
    /// Called for when a bullet hits an asteriod, if it's a 1st level it splits it into 2 smaller ones with slightly different paths
    /// </summary>
    /// <param name="type"></param>
    /// <param name="dir"></param>
    /// <param name="posit"></param>
    public void Impact(bool type, Vector3 dir, Vector3 posit)
    {
        if (type)
        {
            Vector3 dir1;
            Vector3 dir2;

            asteriodSmall1 = Instantiate(asteriods[Random.Range(0,5)]);
            asteriodSmall1.transform.localScale = new Vector3(2f, 2f, 1);
            //asteriodSmall1.transform.Translate(new Vector3(10f, 0, 0));

            asteriodSmall2 = Instantiate(asteriods[Random.Range(0, 5)]);
            asteriodSmall2.transform.localScale = new Vector3(2f, 2f, 1);
            //asteriodSmall2.transform.Translate(new Vector3(-10f, 0, 0));

            if(dir == down || dir == up)
            {
                asteriodSmall2.transform.position = posit + new Vector3(-1f, 0, 0);
                asteriodSmall1.transform.position = posit + new Vector3(1f, 0, 0);

                dir1 = dir + new Vector3(.05f, 0, 0);
                dir2 = dir + new Vector3(-.05f, 0, 0);
            }
            else
            {
                asteriodSmall2.transform.position = posit + new Vector3(0, 1f, 0);
                asteriodSmall1.transform.position = posit + new Vector3(0, -1f, 0);

                dir1 = dir + new Vector3(0, -.05f, 0);
                dir2 = dir + new Vector3(0, .05f, 0);
            }
           

            asts.Add(asteriodSmall1);
            asts.Add(asteriodSmall2);
            directions.Add(dir1);
            directions.Add(dir2);
            types.Add(false);
            types.Add(false);
            
        }
    }
}

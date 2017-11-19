using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    GameObject ship;
    List<GameObject> asteriods;
    List<GameObject> bulletsList;
    SpriteRenderer shipSR;
    SpriteRenderer asteriodSR;
    SpriteRenderer bulletSR;
    float shipMinX;
    float shipMinY;
    float shipMaxX;
    float shipMaxY;
    bool AABBOn;
    float radius;
    float distance;
    int lives;
    GameObject SceneManager;
    AsteriodSpawner script;
    movement scriptMove;
    Texture2D shipTexture;
    float score;
   

    // Use this for initialization
    void Start ()
    {
        asteriods = new List<GameObject>();
        SceneManager = GameObject.Find("SceneManager");
        script = SceneManager.GetComponent<AsteriodSpawner>();
        ship = GameObject.Find("Ship");
        scriptMove = ship.GetComponent<movement>();
        shipSR = ship.GetComponent<SpriteRenderer>();
        lives = 3;
        AABBOn = false;
        shipTexture = shipSR.sprite.texture;
        score = 0;

    }

    // Update is called once per frame
    void Update ()
    {
        asteriods = script.asts;
        bulletsList = scriptMove.bullets;
        shipMinX = shipSR.bounds.min.x;
        shipMinY = shipSR.bounds.min.y;
        shipMaxX = shipSR.bounds.max.x;
        shipMaxY = shipSR.bounds.max.y;

        //checks for collision between the ship and asteriods using AABB, and the bullets and asteriods using circular collision
        AABB();
        BulletImpact();

    }

    /// <summary>
    /// Straightforward AABB collision using the list of asteriods
    /// </summary>
    void AABB()
    {
        for(int x = 0;x < asteriods.Count; x++)
        {
            asteriodSR = asteriods[x].GetComponent<SpriteRenderer>();

            if (asteriodSR.bounds.min.x < shipMaxX && asteriodSR.bounds.max.x > shipMinX && asteriodSR.bounds.min.y < shipMaxY && asteriodSR.bounds.max.y > shipMinY)
            {
                lives--;
                asteriods[x].SetActive(false);
                asteriods[x].transform.position = new Vector3(100, 100, 0);
                if(lives == 0)
                {
                    ship.SetActive(false);
                }
            }
         }
     } 

    /// <summary>
    /// Straightforward Circular collision using the lists of asteriods and bullets
    /// </summary>
    void BulletImpact()
    {
        for(int x =0;x < bulletsList.Count; x++)
        {
            if (bulletsList[x].activeSelf)
            {
                for (int y = 0; y < asteriods.Count; y++)
                {
                    if (asteriods[y].activeSelf)
                    {
                        asteriodSR = asteriods[y].GetComponent<SpriteRenderer>();
                        //the radius changes depending on if the asteriod is 1st and 2nd level
                        if (script.types[y])
                        {
                            radius = 3;
                        }
                        else
                        {
                            radius = 2;
                        }
                        distance = Mathf.Sqrt(Mathf.Pow(asteriodSR.bounds.center.x - bulletsList[x].transform.position.x, 2) + Mathf.Pow(asteriodSR.bounds.center.y - bulletsList[x].transform.position.y, 2));
                        if (distance < radius)
                        {
                            //if they interest, ups points depending on what level the asteriod is
                            if (script.types[y])
                            {
                                score += 20;

                            }
                            else
                            {
                                score += 50;
                            }

                            //moves and disables the bullet, calls the impact method from the Asteriod Spawner Script, and moves/deactivates the asteriod
                            bulletsList[x].SetActive(false);
                            bulletsList[x].transform.position = new Vector3(-100, -100, 0);
                            script.Impact(script.types[y],script.directions[y],asteriods[y].transform.position);
                            asteriods[y].SetActive(false);
                            asteriods[y].transform.position = new Vector3(100, 100, 0);
                        }
                    }
                }
            }
        }
    }

    //Shows the lives and score, as well as the final screen when you lose
    void OnGUI()
    {
        if(lives > 0)
        {
            GUI.Box(new Rect(0, 0, 100, 25), "Lives: ");
            GUI.Box(new Rect(0, 25, 100, 25), "Score: " + score);

            if (lives == 3)
            {
                GUI.Box(new Rect(100, 0, 25, 25), shipTexture);
                GUI.Box(new Rect(125, 0, 25, 25), shipTexture);
                GUI.Box(new Rect(150, 0, 25, 25), shipTexture);

            }
            if (lives == 2)
            {
                GUI.Box(new Rect(100, 0, 25, 25), shipTexture);
                GUI.Box(new Rect(125, 0, 25, 25), shipTexture);
            }
            if (lives == 1)
            {
                GUI.Box(new Rect(100, 0, 25, 25), shipTexture);
            }
        }
        else
        {
            GUI.Box(new Rect((Screen.width - 200) / 2, (Screen.height - 50) / 2, 200, 50), "YOU LOSE\n\nFinal Score: " + score);

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    GameObject powerup;
    movement moveScript;
    GameObject Ship;
    SpriteRenderer shipRend;
    SpriteRenderer powerRend;
    int randPower;
    bool active;
    public int delay;
    public int timer;
    
    // Use this for initialization
    void Start ()
    {
        //grabs the ship and powerup objects, and their sprite renderers
        powerup = new GameObject();
        powerup = GameObject.Find("Powerup");
        Ship = GameObject.Find("Ship");
        moveScript = Ship.GetComponent<movement>();
        shipRend = Ship.GetComponent<SpriteRenderer>();
        powerRend = powerup.GetComponent<SpriteRenderer>();
        //active = false;
        delay = 1;
        powerup.SetActive(false);
        timer = 1;
    }

    // Update is called once per frame
    void Update ()
    {
        //checks to see if the timer that activates once a powerup is started has run out, if so it resets and deactivates it
        if(timer % 300 == 0)
        {
            active = false;
            timer = 1;
            moveScript.fireMode = "normal";
        }
        //if the powerup sprite is active in the game, continually checks for collision
        if (powerup.activeSelf)
        {
            AABB();
        }

        //if the sprite isn't active, waits for the delay between spawning another one
        if (delay % 600 == 0 && !powerup.activeSelf)
        {
            powerup.transform.position = new Vector3(Random.Range(-50,50),Random.Range(-15,15),1);
            delay = 1;
            powerup.SetActive(true);
        }
        //when the powerup isn't active, increases the delay counter
        if (!powerup.activeSelf)
        {
            delay++;
        }

        //if a powerup is currently in use, increases the timer counter
        if (active)
        {
            timer++;
        }
    }

    void AABB()
    {
        //checks for basic AABB collision between the ship and the powerup sprite
        if (powerRend.bounds.min.x < shipRend.bounds.max.x && powerRend.bounds.max.x > shipRend.bounds.min.x && powerRend.bounds.min.y < shipRend.bounds.max.y && powerRend.bounds.max.y > shipRend.bounds.min.y)
        {
            randPower = Random.Range(0, 3);
            if (randPower == 0)
            {
                moveScript.fireMode = "aoe";
            }
            if (randPower == 1)
            {
                moveScript.fireMode = "rapid";
            }
            if (randPower == 2)
            {
                moveScript.fireMode = "tri";
            }
            powerup.SetActive(false);
            active = true;
        }
        
    }

    //shows the current firing mode 
    void OnGUI()
    {
        if(moveScript.fireMode == "normal")
        {
            GUI.Box(new Rect(0, Screen.height-25, 200, 25), "Firing Mode: Normal");
        }
        if (moveScript.fireMode == "aoe")
        {
            GUI.Box(new Rect(0, Screen.height - 25, 200, 25), "Firing Mode: AOE");
        }
        if (moveScript.fireMode == "tri")
        {
            GUI.Box(new Rect(0, Screen.height - 25, 200, 25), "Firing Mode: Tri Shot");
        }
        if (moveScript.fireMode == "rapid")
        {
            GUI.Box(new Rect(0, Screen.height - 25, 200, 25), "Firing Mode: Rapid Fire");
        }
    }
}

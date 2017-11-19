using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

    [SerializeField]
    public Vector3 pos;
    public Vector3 velocity;
    public Vector3 direction;
    public float magnitude;
    public float speed;
    public Vector3 speed2;
    Quaternion angle;
    //trakcs the current state of the vehicle
    private string dir;
    public float accel;
    public float decel;
    public float maxSpeed;
    public List<GameObject> bullets;
    GameObject bullet;
    public Dictionary<GameObject, Vector3> bulletsD;
    Vector3 bulletDir;
    public float delay;
    public float maxDelay;
    bool ready;
    public string fireMode;
    Quaternion rotate;

    // Use this for initialization
    void Start ()
    {
        pos = transform.position;
        speed = .2f;
        maxSpeed = .8f;
        accel = .01f;
        decel = .02f;
        dir = "still";
        bullets = new List<GameObject>();
        bulletsD = new Dictionary<GameObject, Vector3>();
        ready = true;
        maxDelay = 30f;
        fireMode = "normal";
    }
	
	// Update is called once per frame
	void Update ()
    {
        direction.Normalize();

        //Calculate Velocity: Velocity = Speed * Direction
        velocity = speed * direction;
        angle = transform.rotation;

        //Allows the User to use the WASD keys to move the vehicle around the scene
        //rotates to the left
        if (Input.GetKey("a"))
        {
            transform.Rotate(Vector3.forward * 250 * Time.deltaTime);
        }
        //rotates to the right
        if (Input.GetKey("d"))
        {
            transform.Rotate(Vector3.back * 250 * Time.deltaTime);
        }
        //checks to see if the user is moving forward or backward, then differentiates between the two
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            //if w is clicked and it is in the forward or still state, applies the movement and increases the speed through acceleration, if it is not at max speed
            if (Input.GetKey("w"))
            {
                if(dir == "forward" || dir == "still")
                {
                    pos += (angle * velocity);
                    transform.position = pos;
                    if (speed < maxSpeed)
                    {
                        speed += accel;
                    }
                    dir = "forward";

                }
                //if it's in the backwad state, slows the speed to 0 slowly then switches states
                else
                {
                    if(speed > 0)
                    {
                        speed -= (2 * decel);
                        pos -= (angle * velocity);
                        transform.position = pos;
                    }
                    else
                    {
                        dir = "forward";
                    }

                }


            }

            //if backward, does the same, but subtracts the pos to move back
            else if (Input.GetKey("s"))
            {
                
                if(dir == "backward" || dir == "still")
                {
                    pos -= (angle * velocity);
                    transform.position = pos;
                    if (speed < maxSpeed)
                    {
                        speed += accel;
                        
                    }
                    dir = "backward";
                }
                else
                {
                    if (speed > 0)
                    {
                        speed -= (2 * decel);
                        pos += (angle * velocity);
                        transform.position = pos;

                    }
                    else
                    {
                        dir = "backward";
                    }
                }


            }
        }
        //if the input is not going forward or backward, starts deceleration process
        else
        {
            //if speed isn't 0, slowly lowers the speed while continuing to move the car until it stops on its own
            if(speed > 0)
            {
                speed -= decel;

                if (dir == "forward")
                {
                    pos += (angle * velocity);
                    transform.position = pos;
                }
                if (dir == "backward")
                {
                    pos -= (angle * velocity);
                    transform.position = pos;
                }

            }
            else
            {
                dir = "still";
            }

        }

        //calls screenwrap method
        ScreenWrap();

        //waits for the delay to hit the max delay between bullets, once it does it allows the fire method to be called again
        if(delay % maxDelay == 0)
        {
            ready = true;
            delay = 0;
        }

        //if the proper time interval has occured since the last firing, and space bar is pressed, calls the fire method and resets the delay
        if (Input.GetKeyDown("space") && ready)
        {
            Fire(fireMode);
            ready = false;
        }

        //steps through the dictionary of bullets and their direction vectors to check for them going offscreen
        foreach (KeyValuePair<GameObject,Vector3> bul in bulletsD)
        {
            bul.Key.transform.position += (2*bul.Value);
            if(bul.Key.transform.position.x > 65f || bul.Key.transform.position.x < -65f || bul.Key.transform.position.y > 25f || bul.Key.transform.position.y < -25f)
            {
                //once a bullet goes offscreen, it is deactivated and moved out of the way
                bul.Key.SetActive(false);
                bul.Key.transform.position = new Vector3(200, -200, 1);
            }
        }
        //if the delay between bullets is still not done, increments ready
        if (!ready)
        {
            delay += 1;
        }

      

    }

    /// <summary>
    /// if the vehicle fully leaves the view of the camera, it appears on the other side of the camera
    /// </summary>
    void ScreenWrap()
    {
        if (pos.x > 60f)
        {
            pos.x = -60f;
        }
        if (pos.x < -60f)
        {
            pos.x = 60f;
        }
        if (pos.y > 35)
        {
            pos.y = -35f;
        }
        if (pos.y < -35f)
        {
            pos.y = 35f;
        }
    }

    //instructions on how to use it
    void OnGUI()
    {
       // GUI.Box(new Rect(0, 0, 300, 40),"Use WASD to controller the vehicle.\nIt is capable of moving forward and backward.");

    }
    
    //called whenever a bullet is fired
    void Fire(string mode)
    {

        //the default firing mode, shoots one bullet forward at a slow pace
        if(mode == "normal")
        {
            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            bulletDir = (angle * direction);
            bulletsD.Add(bullet, bulletDir);
            maxDelay = 30f;
        }

        //fires 8 bullets at once all around the ship
        if (mode == "aoe")
        {
            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            bulletDir = (angle * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            bulletDir = -1 * (angle * direction);
            bulletsD.Add(bullet, bulletDir);

            
            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, 90);
            bulletDir = ((rotate *angle) * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, -90);
            bulletDir = ((rotate * angle) * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, 45);
            bulletDir = ((rotate * angle) * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, -45);
            bulletDir = ((rotate * angle) * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, -135);
            bulletDir = ((rotate * angle) * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, 135);
            bulletDir = ((rotate * angle) * direction);
            bulletsD.Add(bullet, bulletDir);
        }

        //reduces the firing delay interval drastically
        if (mode == "rapid")
        {
            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            bulletDir = (angle * direction);
            bulletsD.Add(bullet, bulletDir);
            maxDelay = 5f;
        }

        //fires three bullets in a cone shape
        if (mode == "tri")
        {
            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            bulletDir = (angle * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, 20);
            bulletDir = ((rotate * angle) * direction);
            bulletsD.Add(bullet, bulletDir);

            bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = pos;
            bullets.Add(bullet);
            rotate = Quaternion.Euler(0, 0, -20);
            bulletDir = ((rotate * angle) * direction);
            bulletsD.Add(bullet, bulletDir);
        }

        

    }
}

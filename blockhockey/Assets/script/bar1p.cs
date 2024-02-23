using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar1p : MonoBehaviour
{
    const float barspeed = 18f;
    const float defPosX = -11f;
    const float smashspeed = 20f;
    const float smashtime = 8f;
    float velY;

    float velX;

    int time;

    bool space;

    //int times;

    Rigidbody rd;

    float PosX;

    float PosY;

    float SceY;

    float ycon;

    void Start()
    {
        velY = 0;

        velX = 0;

        time = 0;

        //times = 0;

        rd = GetComponent<Rigidbody>();

        PosX = -11f;

        PosY = 0f;

        SceY = 3f;

        ycon = 0f;

        space = true;
    }


    void Update()
    {
        ycon = Input.GetAxis("Vertical1p");

        PosX = this.transform.position.x;

        PosY = this.transform.position.y;

        //縦横移動
        if (Input.GetKey(KeyCode.W) || ycon < 0f)
        {
            velY = barspeed;
        }
        else if (Input.GetKey(KeyCode.S) || ycon > 0f)
        {
            velY = -barspeed;
        }
        else 
        {
            velY = 0f;
        }

        //smash
        if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("1pA"))
        {
            if(space)time = 0;
            space = false;
        }
        if (space == false)
        {
            if(time < smashtime)
            {
                velX = smashspeed;
            }
            else if(time > smashtime*3f/2f)
            {
                velX = 0f;
                space = true;
            }
            else
            {
                velX = -2f * smashspeed;
            }

            time++;
        }
        else if (PosX != defPosX)
        {
        
            PosX = defPosX;
        }
        transform.position = new Vector3(PosX, PosY, 0f);
        /*if (Input.GetKeyUp(KeyCode.W))
        {

        }
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
        if (Input.GetKeyUp(KeyCode.W))
        {

        }*/
        rd.velocity = new Vector3(velX, velY, 0f);

    }
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "ballitem1p")
        {
            SceY = SceY + 0.5f;
            this.transform.localScale = new Vector3(0.5f,SceY,1f);

        }
    }
}


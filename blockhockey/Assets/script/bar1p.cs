using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar1p : MonoBehaviour
{
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

        if (Input.GetKey(KeyCode.W) || ycon < 0f)
        {
            velY = 18f;
        }
        if (ycon == 0f)
        {
            velY = 0f;
        }
        if (Input.GetKey(KeyCode.S) || ycon > 0f)
        {
            velY = -18f;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            velY = 0f;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            velY = 0f;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("1pA"))
        {
            space = false;

            velX = 10f;

        }
        if (space == false)
        {
            time++;
        }
        if (time > 4f)
        {
            velX =  -40f;
        }
        if (time > 7f)
        {
            velX = 0f;

            time = 0;

            space = true;
        }
        if (PosX < -11f)
        {
            PosX = -11f;
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


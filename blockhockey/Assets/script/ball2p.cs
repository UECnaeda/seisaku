using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball2p : MonoBehaviour
{
    bool whichbar;

    Rigidbody rd;

    int[] per = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

    GameObject ball1ps;

    GameObject ball2ps;

    GameObject ballitem1p;

    GameObject ballitem2p;

    float speed = 10.0f;

    AudioSource bound;

    AudioSource blockdes;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        bound = audioSources[0];
        blockdes = audioSources[1];
        ball1ps = Resources.Load<GameObject>("ball1p");
        ball2ps = Resources.Load<GameObject>("ball2p");
        ballitem1p = Resources.Load<GameObject>("ballitem1p");
        ballitem2p = Resources.Load<GameObject>("ballitem2p");
        this.GetComponent<Rigidbody>().AddForce(
            (transform.up + transform.right) * speed,
            ForceMode.VelocityChange);
        print("2parrive");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        bound.PlayOneShot(bound.clip);
        if (collision.gameObject.tag == "bar1p")
        {
            whichbar = true;
            rd = GetComponent<Rigidbody>();
            /*GameObject bar1p = GameObject.Find("bar1p");
            GameObject balls = Instantiate(ball1ps);
            float bar1pPosY = bar1p.transform.position.y;
            float ballPosY = this.transform.position.y;
            float don = ballPosY - bar1pPosY;*/
            float Velx = rd.velocity.x;
            float Vely = rd.velocity.y;
            float r = Mathf.Sqrt(Mathf.Pow(Velx, 2) + Mathf.Pow(Vely, 2));
            int theta = Random.Range(-80, 80);
            print(theta);
            this.rd.velocity = new Vector3(r*Mathf.Cos(Mathf.Deg2Rad*theta), r*Mathf.Sin(Mathf.Deg2Rad*theta), 0f);
        }
        if (collision.gameObject.tag == "bar2p")
        {
            whichbar = false;
            rd = GetComponent<Rigidbody>();
            /*GameObject bar1p = GameObject.Find("bar1p");
            GameObject balls = Instantiate(ball1ps);
            float bar1pPosY = bar1p.transform.position.y;
            float ballPosY = this.transform.position.y;
            float don = ballPosY - bar1pPosY;*/
            float Velx = rd.velocity.x;
            float Vely = rd.velocity.y;
            float r = Mathf.Sqrt(Mathf.Pow(Velx, 2) + Mathf.Pow(Vely, 2));
            print(r);
            int theta = Random.Range(-80, 80);
            print(theta);
            this.rd.velocity = new Vector3(-r*Mathf.Cos(Mathf.Deg2Rad * theta), r*Mathf.Sin(Mathf.Deg2Rad * theta), 0f);
        }
        if (collision.gameObject.tag == "goal1p")
        {
            FindObjectOfType<score>().AddPoint2p(1);
            GameObject bar1p = GameObject.Find("bar1p");
            GameObject balls = Instantiate(ball1ps);
            float bar1pPosX = bar1p.transform.position.x;
            float bar1pPosY = bar1p.transform.position.y;
            float PosX = bar1pPosX + 1f;
            balls.transform.position = new Vector3(PosX, bar1pPosY, 0f);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "goal2p")
        {
            FindObjectOfType<score>().AddPoint1p(1);
            GameObject bar2p = GameObject.Find("bar2p");
            GameObject balls = Instantiate(ball2ps);
            float bar2pPosX = bar2p.transform.position.x;
            float bar2pPosY = bar2p.transform.position.y;
            float PosX = bar2pPosX - 1f;
            balls.transform.position = new Vector3(PosX, bar2pPosY, 0f);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "block")
        {
            blockdes.PlayOneShot(blockdes.clip);
            if (whichbar == true)
            {
                int index = Random.Range(0, per.Length);
                print(index);
                if (index >= 7)
                {
                    GameObject ballitem = Instantiate(ballitem1p);
                    ballitem.transform.position = this.transform.position;
                }
            }
            if (whichbar == false)
            {
                int index = Random.Range(0, per.Length);
                print(index);
                if (index >= 7)
                {
                    GameObject ballitem = Instantiate(ballitem2p);
                    ballitem.transform.position = this.transform.position;
                }
            }
        }
    }
}

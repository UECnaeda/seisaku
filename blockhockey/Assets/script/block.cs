using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    GameObject ball1ps;

    GameObject ball2ps;

    int random;

   
    // Start is called before the first frame update
    void Start()
    {
        ball1ps = Resources.Load<GameObject>("ball1p");
        ball2ps = Resources.Load<GameObject>("ball2p");
        
    }

    // Update is called once per frame
    void Update()
    {
        random++;
        if(random >= 10)
        {
            random = 0;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ball")
        {

            /*float blockposx = this.transform.position.x;
            float blockposy = this.transform.position.y;
            ball1.transform.position = new Vector3(blockposx, blockposy, 0f);
            ball2.transform.position = new Vector3(blockposx, blockposy, 0f);
            if (random >= 5 && random < 7)
            {
                GameObject ball1 = Instantiate(ball1ps);
                GameObject bar1p = GameObject.Find("bar1p");
                float bar1pPosX = bar1p.transform.position.x;
                float bar1pPosY = bar1p.transform.position.y;
                float PosX1 = bar1pPosX + 1f;
                ball1.transform.position = new Vector3(PosX1, bar1pPosY, 0f);
            }
            else if(random >= 8 && random < 10) { 
                GameObject ball2 = Instantiate(ball2ps);
                GameObject bar2p = GameObject.Find("bar2p");
                float bar2pPosX = bar2p.transform.position.x;
                float bar2pPosY = bar2p.transform.position.y;
                float PosX2 = bar2pPosX - 1f;
                ball2.transform.position = new Vector3(PosX2, bar2pPosY, 0f);
            }*/
            
            Destroy(this.gameObject);
        }
    }
}

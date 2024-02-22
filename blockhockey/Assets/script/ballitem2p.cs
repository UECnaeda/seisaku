using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballitem2p : MonoBehaviour
{
    Rigidbody rd;
    GameObject ball2p;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        ball2p = Resources.Load<GameObject>("ball2p");
        
    }

    // Update is called once per frame
    void Update()
    {
rd.velocity = new Vector3(10f, 0f, 0f);
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "bar2p")
        {
            GameObject ball = Instantiate(ball2p);
            ball.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}

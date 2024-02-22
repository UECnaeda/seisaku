using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballitem1p : MonoBehaviour
{
    Rigidbody rd;
    GameObject ball1p;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>(); 
        ball1p = Resources.Load<GameObject>("ball1p");
    }

    // Update is called once per frame
    void Update()
    {
        rd.velocity = new Vector3(-10f, 0f, 0f);
    }
    void OnTriggerEnter(Collider c)
    {
        print(99362663497);
        if(c.gameObject.tag == "bar1p")
        {
            GameObject ball = Instantiate(ball1p);
            ball.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal2p : MonoBehaviour
{
    int colorchange1;

    int colorchange2;

    int colorchange3;

    bool which = false;
    // Start is called before the first frame update
    void Start()
    {
        colorchange1 = 0;

        colorchange2 = 1;

        colorchange3 = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (which == true)
        {
            print("start");
            colorchange3 = 1;
            colorchange1 += 2;
            //colorchange1 = (colorchange1 + 1) % 360;
            //360で割った余りをcolorchange1に代入しているらしい by遠藤
        }
        if (colorchange1 >= 360)
        {
            print("finish");
            colorchange1 = 0;

            colorchange3 = 0;

            which = false;
        }

        gameObject.GetComponent<Renderer>().material.color =
        //UnityEngine.Color.HSVToRGB(colorchange1, colorchange2, colorchange3);
        UnityEngine.Color.HSVToRGB((float)(colorchange1) / 360, (float)(colorchange2), (float)(colorchange3));

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            print("true");
            which = true;
        }
    }
}

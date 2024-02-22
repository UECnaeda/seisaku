using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text : MonoBehaviour
{

    int colorchange1;

    int colorchange2;

    int colorchange3;

    Text texts;

    // Start is called before the first frame update
    void Start()
    {
        texts = this.GetComponent<Text>();

        colorchange1 = 0;

        colorchange2 = 1;

        colorchange3 = 0;

        // Textコンポーネントを取得
      
            

    }

    // Update is called once per frame
    void Update()
    {
        
            print("start");
            colorchange3 = 1;
            colorchange1 += 2;
            //colorchange1 = (colorchange1 + 1) % 360;
            //360で割った余りをcolorchange1に代入しているらしい by遠藤
        
        if (colorchange1 >= 360)
        {
            print("finish");
            colorchange1 = 0;

            colorchange3 = 0;

        }

       
            texts.color = Color.HSVToRGB((float)(colorchange1) / 360, (float)(colorchange2), (float)(colorchange3));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }
    }
}
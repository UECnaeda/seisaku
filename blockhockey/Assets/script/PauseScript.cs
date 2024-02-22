using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{

    Text text;

    int count;

    int counts;

    int score1p;

    int score2p;
    
    // Update is called once per frame
    void Start()
    {
        counts = 0;
        text = GetComponent<Text>();
        text.text = "Ready";
        Time.timeScale = 0f;

    }
    void Update()
    {
        score1p = score.score1;
        score2p = score.score2;
        text = GetComponent<Text>();
        count++;
        if (count++ >= 180)
        {
            text.text = "Go";
        }
        if (count++ >= 270)
        {
            text.text = " ";
            Time.timeScale = 1f;
            
            //Destroy(this.gameObject);
        }
        if (score1p >= 50)
        {
            text.text = "Finish!";
            counts++;
            Time.timeScale = 0f;
            if (counts >= 180)
            {
                text.text = "1P WIN !";
            }
            if (counts >= 360)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Title");
                score.score1 = 0;
                score.score2 = 0;
                
            }
        }
        if (score2p >= 50)
        {
            text.color = new Color(0f,0.2166667f,1f);
            text.text = "Finish!";
            counts++;
            Time.timeScale = 0f;
            if(counts >= 180)
            {
                text.text = "2P WIN !";
            }
            if (counts >= 360)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Title");
                score.score1 = 0;
                score.score2 = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    public Text score1ps;

    public Text score2ps;

    public static int score1;

    public static int score2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if ((score1 <= 30 && score2 <= 30) || (score1>=50 || score2>= 50))
        {*/
            score1ps.text = score1.ToString();
            score2ps.text = score2.ToString();
        /*}
        else
        {
            score1ps.text = "？";
            score2ps.text = "？";
        }*/
    }
    public void AddPoint1p(int point1p)
    {
        score1 = score1 + point1p;
    }
    public void AddPoint2p(int point2p)
    {
        score2 = score2 + point2p;
    }
}

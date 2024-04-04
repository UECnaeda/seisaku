using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greatline : MonoBehaviour
{
    public GameObject thisline;
    // Start is called before the first frame update
    void Start()
    {
        if(GameMaker.realkaraoke==false){
            thisline = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float onteiba = GameMaker.instance.onteiba;
        float time4sec = GameMaker.instance.time4sec;
        //thisline.transform.localPosition += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
        if(GameMaker.instance.destroybar)Destroy(gameObject);
        if(GameMaker.realkaraoke==false){
            thisline.transform.localPosition -= new Vector3(onteiba/time4sec*Time.deltaTime/2f,0f,0f);
        }
    }
}

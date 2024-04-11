using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class karaokeloc : MonoBehaviour
{
    GameObject thisline;
    Vector3 firstloc;
    // Start is called before the first frame update
    void Start()
    {
        if(GameMaker.realkaraoke==false){
            thisline = this.gameObject;
            firstloc = thisline.transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float notemoving = GameMaker.instance.notemoving;
        //thisline.transform.localPosition += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
        if(GameMaker.realkaraoke==false){
            if(GameMaker.instance.musicstart){
                thisline.transform.localPosition = new Vector3(firstloc.x-notemoving,firstloc.y,firstloc.z);
            }
        }
    }
}

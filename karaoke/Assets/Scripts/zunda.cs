using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zunda : MonoBehaviour
{
    Animator animator;
    SpriteRenderer zundaSR;
    int Singface = 1;
    int sabun = 3;//ç∑ï™êî
    public Sprite zundagood0,zundagood1,zundagood2,zundagoodmute;
    public Sprite zundanormal0,zundanormal1,zundanormal2,zundanormalmute;
    public Sprite zundanogood0,zundanogood1,zundanogood2,zundanogoodmute;
    public Sprite zundabad0,zundabad1,zundabad2,zundabadmute;
    int zundajoutai = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        zundaSR = GetComponent<SpriteRenderer>();
        animator.SetFloat("Speed",(GameMaker.instance.bpm)/120);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

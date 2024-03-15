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

    void zundajoutai_change(){
        
    }

    // Update is called once per frame
    void Update()
    {
        zundajoutai = GameMaker.instance.zunda_joutai;
        if(GameMaker.instance.zunda_speak){
            Singface++;
            if(zundajoutai==0){
                if(Singface%sabun==0){
                    zundaSR.sprite = zundagood0;
                }else if(Singface%sabun == 1){
                    zundaSR.sprite = zundagood1;
                }else if(Singface%sabun == 2){
                    zundaSR.sprite = zundagood2;
                }
            }else if(zundajoutai==1){
                if(Singface%sabun==0){
                    zundaSR.sprite = zundanormal0;
                }else if(Singface%sabun == 1){
                    zundaSR.sprite = zundanormal1;
                }else if(Singface%sabun == 2){
                    zundaSR.sprite = zundanormal2;
                }
            }else if(zundajoutai==2){
                if(Singface%sabun==0){
                    zundaSR.sprite = zundanogood0;
                }else if(Singface%sabun == 1){
                    zundaSR.sprite = zundanogood1;
                }else if(Singface%sabun == 2){
                    zundaSR.sprite = zundanogood2;
                }
            }else if(zundajoutai==3){
                if(Singface%sabun==0){
                    zundaSR.sprite = zundabad0;
                }else if(Singface%sabun == 1){
                    zundaSR.sprite = zundabad1;
                }else if(Singface%sabun == 2){
                    zundaSR.sprite = zundabad2;
                }
            }
        }else if(!GameMaker.instance.zunda_singnow){
            if(zundajoutai==0){
                zundaSR.sprite = zundagoodmute;
            }else if(zundajoutai==1){
                zundaSR.sprite = zundanormalmute;
            }else if(zundajoutai==2){
                zundaSR.sprite = zundanogoodmute;
            }else if(zundajoutai==3){
                zundaSR.sprite = zundabadmute;
            }
        }
    }
}

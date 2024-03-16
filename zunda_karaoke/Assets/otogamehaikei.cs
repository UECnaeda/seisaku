using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otogamehaikei : MonoBehaviour
{
    [SerializeField] Sprite nogood_haikei;
    [SerializeField] Sprite normal_haikei;
    [SerializeField] Sprite good_haikei;
    SpriteRenderer karaoke;
    // Start is called before the first frame update
    void Start()
    {
        karaoke = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMaker.score>GameMaker.instance.face_good_score){
            karaoke.sprite = good_haikei;
        }else if(GameMaker.score>GameMaker.instance.face_normal_score){
            karaoke.sprite = normal_haikei;
        }else if(GameMaker.score>GameMaker.instance.face_nogood_score){
            karaoke.sprite = nogood_haikei;
        }else{
            karaoke.color = new Color32(70,70,70,255);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameMaker : MonoBehaviour
{
    //シングルトンにします　わけわからんかったらhttps://umistudioblog.com/singletonhowto/
    public static GameMaker instance;
    /*
        画面サイズ…xが-8.85～8.85
    */
    float gamescreenx = 17.7f;
    public float bpm = 158;
    public float beats = 4;
    float time4sec;
    
    //カラオケバー
    public GameObject karaokeloc;
    public GameObject karaokebar;
    //バーの大きさ
    float karaokebarheight = 8;
    //ロケーション
    float karaokebarloc = 0.97f;//本当の値から+0.13fしてください　なんかずれてる
    float karaokelocy;
    //バー
    Vector3 karaokebarpos;
    public Slider soundslider;
    public Sprite speak, mute;
    public Image images;
    //音設定
    public AudioClip a3e, a3si,a4so, a4u, a5a, a5se, b3u, b4a, b5i, c4o, c4sa, c5e, c5si,c6u, d4e, d4si, d5so, d5u, e4u, e5a, f4o, f4sa, f5e, f5si, g3o, g3sa, g4e, g4si, g5so, g5u;
    AudioSource audioSource;
    //数値関連
    float sing1,sing2;
    float jump;
    float soundvalue = 30;
    int selectsound;
    int beforesound = 30;
    float soundtime = 0;
    float onteiba = 18.1f; //音程の奴の最大横幅
    float onteitani; //バーの移動と音程の奴の大きさをインタラクトさせるための単位
    Gamecontrols gamecontrols;
    //生成する奴
    public GameObject greatbar;
    GameObject nowbar;
    //生成した場所
    float geneposx;
    //音程のやつをすべてこわす
    public bool destroybar = false;
    // Start is called before the first frame update
    void Awake()
    {
        //シングルトンの呪文　自分以外のinstanceが合ったらデストロイする
        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            //
            //インスタンスが複数存在しないように、既に存在していたら自身を消去する
            Destroy(gameObject);
        }
        //FPSの初期設定　変えたいならinstance使う
        SetFps(60);
        //karaokeloc = GameObject.Find("karaokeloc").GetComponent<GameObject>();
        soundslider = GameObject.Find("Slider").GetComponent<Slider>();
        audioSource = GetComponent<AudioSource>();
        //publicなのでとりあえずここで設定する　関数外だとunityの値が有線される
        bpm = 158;
        beats = 4;
        time4sec = 240*beats/bpm; //4小節終わるまでの時間
        onteitani = onteiba/gamescreenx; //これにタイムデルタしたらちょうどよくなるはず     
    }
    public void SetFps(int fps){
        Application.targetFrameRate = fps;
        Time.fixedDeltaTime = 1.0f/(float)fps;
    }
    private void OnEnable()
    {
        gamecontrols = new Gamecontrols();
        gamecontrols.Player.Jumppress.started += OnJumppress;
        gamecontrols.Player.Jumppress.performed += OnJumppress;
        gamecontrols.Player.Jumppress.canceled += OnJumppress;
        gamecontrols.Player.Jumprelease.started += OnJumprelease;
        gamecontrols.Player.Jumprelease.performed += OnJumprelease;
        gamecontrols.Player.Jumprelease.canceled += OnJumprelease;
        gamecontrols.Enable();
    }
    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        gamecontrols?.Dispose();
    }
    public void OnJumppress(InputAction.CallbackContext context)
    {   
        if (!context.performed) return;
        Debug.Log("press");
        Sing();
        jump = context.ReadValue<float>();
        images.sprite = speak;
    }

    // 離された瞬間のコールバック
    public void OnJumprelease(InputAction.CallbackContext context)
    {

        if (!context.performed) return;
        Debug.Log("release");
        audioSource.Stop();
        jump = context.ReadValue<float>();
        images.sprite = mute;
        nowbar = null;
    }

    void OnSing1(InputValue context)
    {
        sing1 = context.Get<float>();
    }
    void OnSing2(InputValue context)
    {
        sing2 = context.Get<float>();
    }
    void OnJump(InputValue context)
    {
        jump = context.Get<float>();
    }
    void Sing()
    {   
        audioSource.Stop();//元々なってる音を止める;
        Debug.Log("Singcall");
        switch(selectsound){
            case 0: 
                audioSource.PlayOneShot(g3o);
                break;
            case 1: 
                audioSource.PlayOneShot(g3sa);
                break;
            case 2: 
                audioSource.PlayOneShot(a3e);
                break;
            case 3: 
                audioSource.PlayOneShot(a3si);
                break;
            case 4: 
                audioSource.PlayOneShot(b3u);
                break;
            case 5: 
                audioSource.PlayOneShot(c4o);
                break;
            case 6: 
                audioSource.PlayOneShot(c4sa);
                break;
            case 7: 
                audioSource.PlayOneShot(d4e);
                break;
            case 8: 
                audioSource.PlayOneShot(d4si);
                break;
            case 9: 
                audioSource.PlayOneShot(e4u);
                break;
            case 10: 
                audioSource.PlayOneShot(f4o);
                break;
            case 11: 
                audioSource.PlayOneShot(f4sa);
                break;
            case 12: 
                audioSource.PlayOneShot(g4e);
                break;
            case 13: 
                audioSource.PlayOneShot(g4si);
                break;
            case 14: 
                audioSource.PlayOneShot(a4u);
                break;
            case 15: 
                audioSource.PlayOneShot(a4so);
                break;
            case 16: 
                audioSource.PlayOneShot(b4a);
                break;
            case 17: 
                audioSource.PlayOneShot(c5e);
                break;
            case 18: 
                audioSource.PlayOneShot(c5si);
                break;
            case 19: 
                audioSource.PlayOneShot(d5u);
                break;
            case 20: 
                audioSource.PlayOneShot(d5so);
                break;
            case 21: 
                audioSource.PlayOneShot(e5a);
                break;
            case 22: 
                audioSource.PlayOneShot(f5e);
                break;
            case 23: 
                audioSource.PlayOneShot(f5si);
                break;
            case 24: 
                audioSource.PlayOneShot(g5u);
                break;
            case 25: 
                audioSource.PlayOneShot(g5so);
                break;
            case 26: 
                audioSource.PlayOneShot(a5a);
                break;
            case 27: 
                audioSource.PlayOneShot(a5se);
                break;
            case 28: 
                audioSource.PlayOneShot(b5i);
                break;
            case 29: 
                audioSource.PlayOneShot(c6u);
                break;
            default:
                Debug.Log("Default??");
                break;
        }
        Debug.Log(selectsound);
        karaokelocy = karaokebarloc-(karaokebarheight/2) +  (karaokebarheight*(float)selectsound/soundvalue);
        nowbar = Instantiate(greatbar, new Vector3(karaokebarpos.x, karaokelocy, -5), Quaternion.identity);
        nowbar.GetComponent<SpriteRenderer>().color = Color.red;
        geneposx = karaokebarpos.x;
     }
    // Update is called once per frame
    void Update()
    {
        //nowbarの操作
        if(nowbar!=null){
            nowbar.transform.localPosition += new Vector3(onteiba/time4sec*Time.deltaTime/2f,0f,0f);
            nowbar.transform.localScale += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
        }
        soundslider.value = selectsound; //バーを動かす
        //soundtime += Time.deltaTime;
        //selectsound = (int)((soundvalue-1)*(sing1+sing2+2)/4);//スティックの傾きから0～soundvalue-1の値を出す
        selectsound = (int)((soundvalue-1)*(sing1+1)/2);　///sing1だけでやるならこっち
        //karaokebarを動かす　白いやつ
        karaokebarpos = karaokebar.transform.position;
        if(karaokebarpos.x >= gamescreenx/2){
            destroybar = true;
            karaokebar.transform.position += new Vector3((gamescreenx/time4sec*Time.deltaTime)-gamescreenx,0f,0f);
        }else {
            destroybar = false;
            karaokebar.transform.position += new Vector3(gamescreenx/time4sec*Time.deltaTime,0f,0f);
        }
        //karaokelocを動かす　赤い奴
        karaokebarpos = karaokebar.transform.position;
        karaokeloc.transform.position = new Vector3(karaokebarpos.x,karaokelocy,karaokebarpos.z-3f);
        if(jump==1&&(beforesound!=selectsound||soundtime>=5f)){
            Sing();//音が変わったので呼ぶ
            soundtime = 0;
            beforesound = selectsound;
        }
    }
}

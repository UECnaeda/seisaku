using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameMaker : MonoBehaviour
{
    public int setfpsvalue = 60;
    //シングルトンにします　わけわからんかったらhttps://umistudioblog.com/singletonhowto/
    public static GameMaker instance;
    /*
        画面サイズ…xが-8.85〜8.85
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
    public AudioSource zundavoice_AS;
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

    //jump機能したときのボタン
    bool jumpbothcheck = false;
    float jumpposx = 0f;

    //ずんだもんが歌ってるかどうか
    public bool zunda_singnow = false;

    //歌い始め
    public bool zunda_speak = false;

    //ゲーム速度変化
    public float gamespeed = 1.0f;
    
    //ゲーム同期
    public bool musicstart = false;
    public int delaytime = 0;
    int waitstart = 90;

    //判定の厳しさ プラスマイナスms表記
    public float Judge_score_timing_good = 0.033f;
    public float Judge_score_timing_safe = 0.075f;
    public float Judge_score_timing_bad = 0.125f;

    //スコア関連
    //musicオブジェクトのnoteスクリプトからリストを値のみ参照
    public GameObject music;
    public note notescript;
    //noteに関する情報List
    List<List<float>> notelist = new List<List<float>>();
    int notenumber = 0;
    int notecount;

    //現在考えるべきnoteの情報
    float note_timingposx;
    float note_ontei;


    float score = 60;
    float add_good_timingScore = 0.3f;
    float add_safe_timingScore = 0.05f;
    float add_bad_timingScore = -1;

    public TMP_Text score_display;


    void Notelistmaker(ref List<List<float>> NL){
        Debug.Log("GameMaker:Notelistmaker");
        notenumber = 0;
        notelist = NL;
        notecount = notelist.Count;
        if(notenumber < notecount){
            note_timingposx = notelist[notenumber][0];
            note_ontei = notelist[notenumber][1];
        }
    }

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
        SetFps(setfpsvalue);
        //karaokeloc = GameObject.Find("karaokeloc").GetComponent<GameObject>();
        soundslider = GameObject.Find("Slider").GetComponent<Slider>();
        music = GameObject.Find("music");
        notescript = music.GetComponent<note>();
        zundavoice_AS = GetComponent<AudioSource>();
        //publicなのでとりあえずここで設定する　関数外だとunityの値が有線される
        bpm = 158;
        beats = 4;
        time4sec = 240*beats/bpm; //4小節終わるまでの時間
        onteitani = onteiba/gamescreenx; //これにタイムデルタしたらちょうどよくなるはず
        score_display = GameObject.Find("Score").GetComponent<TMP_Text>();
        Notelistmaker(ref notescript.notes_timingposx_list);
    }
    public void SetFps(int fps){
        Application.targetFrameRate = fps;
        Time.fixedDeltaTime = 1.0f/(float)fps;
    }
    private void OnEnable()
    {
        gamecontrols = new Gamecontrols();
        gamecontrols.Player.Jumpboth.started += OnJumpboth;
        gamecontrols.Player.Jumpboth.performed += OnJumpboth;
        gamecontrols.Player.Jumpboth.canceled += OnJumpboth;
        gamecontrols.Enable();
        InputSystem.pollingFrequency = 121;
    }
    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        gamecontrols?.Dispose();
    }

    /*
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
        zundavoice_AS.Stop();
        jump = context.ReadValue<float>();
        images.sprite = mute;
        nowbar = null;
    }
    */
    void OnSing1(InputValue context)
    {
        sing1 = context.Get<float>();
    }
    void OnSing2(InputValue context)
    {
        sing2 = context.Get<float>();
    }
    void OnJumpboth(InputAction.CallbackContext context)
    {
        jumpbothcheck = true;
        jump = context.ReadValue<float>();
    }
    void Sing()
    {   
        zundavoice_AS.Stop();//元々なってる音を止める;
        //Debug.Log("Singcall");
        switch(selectsound){
            case 0: 
                zundavoice_AS.PlayOneShot(g3o);
                break;
            case 1: 
                zundavoice_AS.PlayOneShot(g3sa);
                break;
            case 2: 
                zundavoice_AS.PlayOneShot(a3e);
                break;
            case 3: 
                zundavoice_AS.PlayOneShot(a3si);
                break;
            case 4: 
                zundavoice_AS.PlayOneShot(b3u);
                break;
            case 5: 
                zundavoice_AS.PlayOneShot(c4o);
                break;
            case 6: 
                zundavoice_AS.PlayOneShot(c4sa);
                break;
            case 7: 
                zundavoice_AS.PlayOneShot(d4e);
                break;
            case 8: 
                zundavoice_AS.PlayOneShot(d4si);
                break;
            case 9: 
                zundavoice_AS.PlayOneShot(e4u);
                break;
            case 10: 
                zundavoice_AS.PlayOneShot(f4o);
                break;
            case 11: 
                zundavoice_AS.PlayOneShot(f4sa);
                break;
            case 12: 
                zundavoice_AS.PlayOneShot(g4e);
                break;
            case 13: 
                zundavoice_AS.PlayOneShot(g4si);
                break;
            case 14: 
                zundavoice_AS.PlayOneShot(a4u);
                break;
            case 15: 
                zundavoice_AS.PlayOneShot(a4so);
                break;
            case 16: 
                zundavoice_AS.PlayOneShot(b4a);
                break;
            case 17: 
                zundavoice_AS.PlayOneShot(c5e);
                break;
            case 18: 
                zundavoice_AS.PlayOneShot(c5si);
                break;
            case 19: 
                zundavoice_AS.PlayOneShot(d5u);
                break;
            case 20: 
                zundavoice_AS.PlayOneShot(d5so);
                break;
            case 21: 
                zundavoice_AS.PlayOneShot(e5a);
                break;
            case 22: 
                zundavoice_AS.PlayOneShot(f5e);
                break;
            case 23: 
                zundavoice_AS.PlayOneShot(f5si);
                break;
            case 24: 
                zundavoice_AS.PlayOneShot(g5u);
                break;
            case 25: 
                zundavoice_AS.PlayOneShot(g5so);
                break;
            case 26: 
                zundavoice_AS.PlayOneShot(a5a);
                break;
            case 27: 
                zundavoice_AS.PlayOneShot(a5se);
                break;
            case 28: 
                zundavoice_AS.PlayOneShot(b5i);
                break;
            case 29: 
                zundavoice_AS.PlayOneShot(c6u);
                break;
            default:
                Debug.Log("Default??");
                break;
        }
        //Debug.Log(selectsound);
        karaokelocy = karaokebarloc-(karaokebarheight/2) + (karaokebarheight*(float)selectsound/soundvalue);
        nowbar = Instantiate(greatbar, new Vector3(karaokebarpos.x, karaokelocy, -5), Quaternion.identity);
        geneposx = karaokebarpos.x;
    }

    //音程に関するスコア加点
    void Score_ontei(){
        //音程が1音ずれまではnormal、2音以上でbad
        //音を鳴らせてない場合はgood判定以上空いたらbad判定に
    }

    //タイミングに関するスコア加点
    void Score_timing(){
        //これ以上noteのlistがない場合タイミングに関する判定はまだないのでスルー
        Debug.Log($"GameMaker:Start Score_timing");
        Debug.Log($"GameMaker:Judge_score posx is +- {onteiba/time4sec * Judge_score_timing_good},{onteiba/time4sec * Judge_score_timing_safe},{onteiba/time4sec * Judge_score_timing_bad}");
        Debug.Log($"jumpposx = {jumpposx}, note_timingposx = {note_timingposx}, note_ontei = {note_ontei}");
        if(notenumber < notecount - 1){
            if((jumpposx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_good)) && (jumpposx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_good))){
                Debug.Log("GameMaker:Add_good_timingScore");
                // ± onteiba/time4sec * Judge_score_timing_good がgood判定のx範囲
                // タイミングがgoodの場合の処理をする
                // note_onteiとnote_timingposxの更新
                notenumber++;
                note_ontei = notelist[notenumber][1];
                if(notenumber < notecount - 1)note_timingposx = notelist[notenumber][0];
                //add_good_timingScoreスコア追加
                if(note_ontei >= 0){
                    score += add_good_timingScore;
                }
            }else if((jumpposx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_safe)) && (jumpposx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_safe))){
                // タイミングがsafeの場合の処理をする
                // note_onteiとnote_timingposxの更新
                Debug.Log("GameMaker:Add_safe_timingScore");
                notenumber++;
                note_ontei = notelist[notenumber][1];
                if(notenumber < notecount - 1)note_timingposx = notelist[notenumber][0];
                //add_safe_timingScoreスコア追加
                if(note_ontei >= 0){
                    score += add_safe_timingScore;
                }
            }else if((jumpposx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_bad)) && (jumpposx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_bad))){
                // タイミングが悪い場合の処理をする
                // note_onteiとnote_timingposxの更新
                Debug.Log("GameMaker:Add_safe_timingScore");
                notenumber++;
                note_ontei = notelist[notenumber][1];
                if(notenumber < notecount - 1)note_timingposx = notelist[notenumber][0];
                //add_bad_timingScoreスコア追加
                if(note_ontei >= 0){
                    score += add_bad_timingScore;
                }
            }
        //badの範囲外の場合の判定はなし(から撃ちあり)
            Debug.Log("GameMaker:NO_Add_good_timingScore");
        }
        Debug.Log($"GameMaker:notenumber = {notenumber}, notecount = {notecount}");
    }

    //ノーツをタップせずスルーした場合
    //badの範囲外に出たとき専用に呼ばれる(これ作る必要ないかも)
    void Score_timing_pass(){
        if(notenumber < notecount-1){
            // note_onteiとnote_timingposxの更新
            notenumber++;
            note_ontei = notelist[notenumber][1];
            if(notenumber < notecount - 1)note_timingposx = notelist[notenumber][0];
            if(note_ontei >= 0){
                score += add_bad_timingScore;
            }
        }
        Debug.Log("Score_timign_pass");
    }


    // Update is called once per frame
    void Update()
    {
        //ポーズ、曲開始までちょっと待つ
        waitstart--;
        if(waitstart==0){
            gamespeed = 1f;
            musicstart = true;
        }else{

        }
        Time.timeScale = gamespeed;
        //nowbarの操作
        if(nowbar!=null){
            nowbar.transform.localPosition += new Vector3(onteiba/time4sec*Time.deltaTime/2f,0f,0f);
            nowbar.transform.localScale += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
        }
        //soundtime += Time.deltaTime;
        //selectsound = (int)((soundvalue-1)*(sing1+sing2+2)/4);//スティックの傾きから0〜soundvalue-1の値を出す
        selectsound = (int)((soundvalue-1)*(sing1+1)/2);　///sing1だけでやるならこっち
        soundslider.value = selectsound; //バーを動かす
        karaokebarpos = karaokebar.transform.position;//karaokebarを動かす　白いやつ
        if(waitstart>=delaytime){

        }else if(karaokebarpos.x >= gamescreenx/2){
            destroybar = true;
            karaokebar.transform.position += new Vector3((gamescreenx/time4sec*Time.deltaTime)-gamescreenx,0f,0f);
        }else{
            destroybar = false;
            karaokebar.transform.position += new Vector3(gamescreenx/time4sec*Time.deltaTime,0f,0f);
        }

        //音を変更
        zunda_speak = false;
        if(jumpbothcheck){
            if(jump==1){
                Sing();
                images.sprite = speak;
                zunda_singnow = true;
                zunda_speak = true;
                //jumpが押された瞬間のxをカウント、ノーツと比べてタイミング判定を行う
                jumpposx = karaokebarpos.x;
                Score_timing();
            }else{
                zundavoice_AS.Stop();
                images.sprite = mute;
                nowbar = null;
                zunda_singnow = false;
            }
        }else if(jump==1&&(beforesound!=selectsound)){
            Sing();//音が変わったので呼ぶ
            beforesound = selectsound;
            zunda_speak = true;
        }
        if(jumpbothcheck){
            jumpbothcheck = false;
        }
        //karaokelocを動かす　赤い奴
        karaokebarpos = karaokebar.transform.position;
        karaokeloc.transform.position = new Vector3(karaokebarpos.x,karaokelocy,karaokebarpos.z-3f);

        //スコア関連_タップ忘れ確認
        if(karaokebarpos.x >= note_timingposx - (onteiba/time4sec * Judge_score_timing_bad)){
            Score_timing_pass();
        }
        score_display.text =  $"Score:{score}";
    }
    void LateUpdate(){
        //新しく生成されたノーツのデータと、jumpを押し続けていた場合の処理
        if(destroybar){
            Notelistmaker(ref notescript.notes_timingposx_list);
            //destroybar直前に押したxの確認を行う
            jumpposx -= gamescreenx;
            Score_timing();
            if(jump==1){
                destroybar = false;
                nowbar = Instantiate(greatbar, new Vector3(karaokebarpos.x, karaokelocy, -5), Quaternion.identity);
                nowbar.GetComponent<SpriteRenderer>().color = Color.red;
                //Score_ontei();
            }
        }
    }
}

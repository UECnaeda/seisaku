using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameMaker : MonoBehaviour
{
    //fps
    public int setfpsvalue = 60;
    //ゲーム速度変化
    public float gamespeed = 1.0f;

    //ゲームモード
    //0ならeasy、1ならnormal、2ならhard、3ならkeyboard
    public float select_difficult = 2;

    //normalモード用の変数　前の音程を記録しておく
    //最初は-1;
    float before_note = -1;

    //デバッグモード
    public bool score_mode = true;
    public bool debug_mode = true;
    //シングルトン　わけわからんかったらhttps://umistudioblog.com/singletonhowto/
    public static GameMaker instance;
    /*
        画面サイズ…xが-8.85〜8.85
    */
    float gamescreenx = 17.7f;
    public float bpm = 158;
    public float beats = 4;
    public float time4sec;

    //input system、配置数値関連
    float sing1,sing2;
    float jump;
    float jump1;
    float jump2;
    float soundvalue = 30;
    int selectsound;
    int maxsound = 29;
    int beforesound = 30;
    float soundtime = 0;
    public float onteiba = 18.1f; //音程の奴の最大横幅
    float onteitani; //バーの移動と音程の奴の大きさをインタラクトさせるための単位
    Gamecontrols gamecontrols;
    Vector2 moving;
    bool onmove_started = false;
    bool onmove_performing = false;

    //音程のやつをすべてこわす
    public bool destroybar = false;

    //jump機能したときのボタン
    bool jumpbothcheck = false;
    float jump_posx = 0f;

    //ずんだもんが歌ってるかどうか
    public bool zunda_singnow = false;

    //歌い始め
    public bool zunda_speak = false;
    
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
    int note_number = 0;
    int notecount;

    //判定に使うノーツの情報
    //1番目のノーツとバーが重なってるとき、判定のposxを見るのは2番目のノーツ
    //note_numberの位置の値がnote_timingposx、note_number-1の位置の値がnote_onteiに
    float note_timingposx;
    float note_ontei = -1;

    //スコア計算用

    public static float score_ontei_sum = 40;
    public static float score_timing_sum = 40;
    public static float score = 0;

    public float add_good_timingScore = 0.3f;
    public float add_safe_timingScore = 0.05f;
    public float add_bad_timingScore = -1;

    public float add_good_onteiScore = 0.01f;
    public float add_safe_onteiScore = -0.01f;
    public float add_bad_onteiScore = -0.03f;
    public float add_nosing_onteiScore = -0.05f;

    public float nosing_safetime = 0.1f;
    public float nosing_time = 0f;

    public static int count_good_timing = 0;
    public static int count_safe_timing = 0;
    public static int count_bad_timing = 0;
    public static int count_pass_timing = 0;

    //ずんだの顔を変える用

    public float face_good_score = 100f;
    public float face_normal_score = 66f;
    public float face_nogood_score = 33f;
    public float face_bad_score = 0f;

    public int zunda_joutai = 0;

    public TMP_Text score_display;
    public TMP_Text hantei_display;
    //debug用に

    public TMP_Text debug_display;
    float timing_ms;
    //0~3はgood^pass
    int timing_hantei;
    int ontei_hantei;

    //から撃ち判定

    bool timing_karauchi;

    //生成する奴
    public GameObject greatbar;
    GameObject nowbar;

    //生成した場所
    float geneposx;

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
    public Slider scoreslider;
    //バーの動きをどれだけ滑らかにするか
    int slider_mag = 100;
    public Image fill_slider;

    //音設定
    public AudioClip a3e, a3si,a4so, a4u, a5a, a5se, b3u, b4a, b5i, c4o, c4sa, c5e, c5si,c6u, d4e, d4si, d5so, d5u, e4u, e5a, f4o, f4sa, f5e, f5si, g3o, g3sa, g4e, g4si, g5so, g5u;
    public AudioSource zundavoice_AS;
    public int volume_music;
    public int volume_voice;

    //pause用
    bool ispause = false;
    int pause_select = 0;
    string[] pause_words = {"ポーズ解除","最初から","曲選択"};

    //result用
    float sec2count;
    [SerializeField] TMP_Text result1_text;
    [SerializeField] TMP_Text result2_text;
    [SerializeField] TMP_Text result3_text;
    [SerializeField] TMP_Text result4_text;
    [SerializeField] TMP_Text result5_text;
    [SerializeField] Image result_panel;
    int result_select = 0;

    //キーボード操作用
    bool keyboard_press = false;
    char keyboard_input;
    bool keyboard_press_before = false;

    //追加オプション用

    //赤いバーが動かないモード
    public static bool karaokeloc_nomoving = false;
    //ノーツじゃなくてバーが動くモード
    public static bool realkaraoke = true;


    void Notelistmaker(ref List<List<float>> NL){
        Debug.Log("GameMaker:Notelistmaker");
        note_number = 0;
        notelist = NL;
        notecount = notelist.Count;
        if(note_number < notecount){
            note_timingposx = notelist[note_number][0];
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
            //インスタンスが複数存在しないように、既に存在していたら自身を消去する
            Destroy(gameObject);
        }
        Initial_Value();
        SetFps(setfpsvalue);

        //slider初期設定
        scoreslider = GameObject.Find("Slider").GetComponent<Slider>();
        scoreslider.maxValue = slider_mag*100;
        music = GameObject.Find("music");
        notescript = music.GetComponent<note>();
        zundavoice_AS = GetComponent<AudioSource>();
        zundavoice_AS.volume = ((float)volume_voice)/10;
        Debug.Log($"volume_voice = {volume_voice},ZAS.volume = {zundavoice_AS.volume}");

        //bpm、beatsをここで設定
        time4sec = 240*beats/bpm; //4小節終わるまでの時間
        sec2count = time4sec;
        onteitani = onteiba/gamescreenx; //これにタイムデルタしたらちょうどよくなる
        score_display = GameObject.Find("Score").GetComponent<TMP_Text>();
        debug_display = GameObject.Find("debugtext").GetComponent<TMP_Text>();
        if(!score_mode){
            score_display.text = "";
        }
        if(!debug_mode){
            debug_display.text = "";
        }
        
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
        gamecontrols.Player.Move.started += OnMove;
        gamecontrols.Player.Move.performed += OnMove;
        gamecontrols.Player.Move.canceled += OnMove;
        gamecontrols.Player.Jumpboth2.started += OnJumpboth2;
        gamecontrols.Player.Jumpboth2.performed += OnJumpboth2;
        gamecontrols.Player.Jumpboth2.canceled += OnJumpboth2;
        gamecontrols.Player.Start.performed += OnStart;
        gamecontrols.Enable();
        InputSystem.pollingFrequency = setfpsvalue*2+1;
    }
    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        
        gamecontrols.Player.Jumpboth.started -= OnJumpboth;
        gamecontrols.Player.Jumpboth.performed -= OnJumpboth;
        gamecontrols.Player.Jumpboth.canceled -= OnJumpboth;
        gamecontrols.Player.Jumpboth2.started -= OnJumpboth2;
        gamecontrols.Player.Jumpboth2.performed -= OnJumpboth2;
        gamecontrols.Player.Jumpboth2.canceled -= OnJumpboth2;
        gamecontrols.Player.Move.started -= OnMove;
        gamecontrols.Player.Move.performed -= OnMove;
        gamecontrols.Player.Move.canceled -= OnMove;
        gamecontrols.Player.Start.performed -= OnStart;
        gamecontrols.Dispose();
    }
    void OnMove(InputAction.CallbackContext context)
    {
        if(context.started){
            Debug.Log("context.started");
            onmove_started = true;
            onmove_performing = true;
        }
        if(context.performed){
            
        }
        if(context.canceled){
            Debug.Log("context.canceled");
            onmove_performing = false;
        }
        moving = context.ReadValue<Vector2>();
    }
    void OnJumpboth(InputAction.CallbackContext context)
    {
        if(select_difficult!=3){
            jumpbothcheck = true;
            jump1 = context.ReadValue<float>();
        }
    }

    void OnJumpboth2(InputAction.CallbackContext context)
    {
        if(select_difficult!=3){
            jumpbothcheck = true;
            jump2 = context.ReadValue<float>();
        }
    }

    void OnStart(InputAction.CallbackContext context){
        ispause = true;
    }
    //初期化関連
    //設定の値の適用などなど
    void Initial_Value(){

        //ゲームモード
        select_difficult = select_songBehaviour.select_difficult;

        //音量
        volume_music = titleBehaviour.volume_music;
        volume_voice = titleBehaviour.volume_voice;

        //タイミング調整
        delaytime = titleBehaviour.delaytime;

        //判定時間(ms)
        Judge_score_timing_good = titleBehaviour.Judge_score_timing_good;
        Judge_score_timing_safe = titleBehaviour.Judge_score_timing_safe;
        Judge_score_timing_bad = titleBehaviour.Judge_score_timing_bad;

        //スコア(音程)
        if(select_difficult==0){
            //easyモードは音程関係なし
            add_good_onteiScore = 0f;
            add_safe_onteiScore = 0f;
            add_bad_onteiScore = 0f;
        }else{
            add_good_onteiScore = titleBehaviour.add_good_onteiScore;
            add_safe_onteiScore = titleBehaviour.add_safe_onteiScore;
            add_bad_onteiScore = titleBehaviour.add_bad_onteiScore;
        }
        add_nosing_onteiScore = titleBehaviour.add_nosing_onteiScore;

        //スコア(タイミング)
        add_good_timingScore = titleBehaviour.add_good_timingScore;
        add_safe_timingScore = titleBehaviour.add_safe_timingScore;
        add_bad_timingScore = titleBehaviour.add_bad_timingScore;
        setfpsvalue = titleBehaviour.setfpsvalue;

        //スコア(カウント数)
        count_good_timing = 0;
        count_safe_timing = 0;
        count_bad_timing = 0;
        count_pass_timing = 0;

        //スコア合計
        if(select_difficult==0){
            score_ontei_sum = 0;
            score_timing_sum = 60;
        }else{
            score_ontei_sum = 40;
            score_timing_sum = 40;
        }

        //開幕待機時間
        //そのうちカウント入れたい
        waitstart = 90;

        //曲情報
        bpm = 158;
        beats = 4;

        //result初期設定
        result_panel.color = new Color32(255,255,255,0);
        result1_text.text = "";
        result2_text.text = "";
        result3_text.text = "";
        result4_text.text = "";
        result5_text.text = "";
    }

    void Jumpcheck(){
        //jumpボタンが2つあるのでその処理
        //どちらかも押されていなければjumpの値を0にする
        //ボタンが1つだけ離れた瞬間であればjumpbothcheckの処理は行わない
        if(jump2==0&&jump1==0){
            jump = 0;
        }else if(jump==0){
            jump = 1;
        }else if(jump1!=1||jump2!=1){
            jumpbothcheck = false;
        }
    }

    int Moving_change_int_x(int a, int b,int c){
        //aをmoving.xの値が+か-かで+bする関数
        //aが0以下の場合a=a+cとする
        if(a<=0)a+=c;
        if(onmove_started){
            Debug.Log("onmove");
            if(moving.x>0){
                a += b;
            }else{
                a -= b;
            }
        }
        return a;
    }

    /*
    //キーボード操作
    //難易度に応じて入力の値を変更する
    //OnTextInputの仕様的にこれで継続入力は無理　長押ししても規定時間ごとに入力が繰り返されるタイプなので
    void exchange_keyboardinput(){
       print($"OnTextInput: {keyboard_input}({(int) keyboard_input:X02})");
        if(select_difficult==3){
            if(keyboard_input=='z'){
                selectsound = 0;
            }else if(keyboard_input=='x'){
                selectsound = 1;
            }else if(keyboard_input=='c'){
                selectsound = 2;
            }else if(keyboard_input=='v'){
                selectsound = 3;
            }else if(keyboard_input=='b'){
                selectsound = 4;
            }else if(keyboard_input=='n'){
                selectsound = 5;
            }else if(keyboard_input=='m'){
                selectsound = 6;
            }else if(keyboard_input==','){
                selectsound = 7;
            }else if(keyboard_input=='.'){
                selectsound = 8;
            }else if(keyboard_input=='/'){
                selectsound = 9;
            }else if(keyboard_input=='a'){
                selectsound = 10;
            }else if(keyboard_input=='s'){
                selectsound = 11;
            }else if(keyboard_input=='d'){
                selectsound = 12;
            }else if(keyboard_input=='f'){
                selectsound = 13;
            }else if(keyboard_input=='g'){
                selectsound = 14;
            }else if(keyboard_input=='h'){
                selectsound = 15;
            }else if(keyboard_input=='j'){
                selectsound = 16;
            }else if(keyboard_input=='k'){
                selectsound = 17;
            }else if(keyboard_input=='l'){
                selectsound = 18;
            }else if(keyboard_input==';'){
                selectsound = 19;
            }else if(keyboard_input=='q'){
                selectsound = 20;
            }else if(keyboard_input=='w'){
                selectsound = 21;
            }else if(keyboard_input=='e'){
                selectsound = 22;
            }else if(keyboard_input=='r'){
                selectsound = 23;
            }else if(keyboard_input=='t'){
                selectsound = 24;
            }else if(keyboard_input=='y'){
                selectsound = 25;
            }else if(keyboard_input=='u'){
                selectsound = 26;
            }else if(keyboard_input=='i'){
                selectsound = 27;
            }else if(keyboard_input=='o'){
                selectsound = 28;
            }else if(keyboard_input=='p'){
                selectsound = 29;
            }
        }
    }
    */

    //normalの処理がちょっとややこしいので個別で関数を作る
    //note_onteiの値が更新されるタイミングで呼ぶ
    void normal_change_ontei(){
        int i=0;
        if(before_note==-1||before_note==note_ontei){
                
        }else if(before_note>note_ontei){
        //前の音程が今の音程より上なら1回下に入力する必要がある
            i = 1;
        }else{
            i = -1;
        }
        if(note_ontei>=0){
            selectsound = (int)note_ontei + i;
            if(selectsound<0)selectsound=0;
            else if(selectsound>maxsound)selectsound=maxsound;
        }
    }

    void Apply_difficult(){
        if(select_difficult==0){
            //easyモード
            //音程自動追尾
            if(note_ontei>=0){
                selectsound = (int)note_ontei;
            }
        }else if(select_difficult==1){
            //normalモード
            //音程はmoveを入れるたびに変化させる
            //
            if(onmove_started){
                if(moving.y>0){
                    if(selectsound<maxsound){
                        selectsound++;
                    }
                }else{
                    if(selectsound>0){
                        selectsound--;
                    }
                }
                onmove_started = false;
            }
        }else if(select_difficult==2){
            //hardモード、コントローラー、スティック1本
            //音程はダイレクトに変化する
            selectsound = (int)((soundvalue-1)*(moving.y+1)/2);
        }else if(select_difficult==3){
            //keyboardモード
            //バグだらけ、要修正
            //exchange_keyboardinput();
            if(keyboard_press_before){
                if(!keyboard_press){
                    jumpbothcheck=true;
                    jump = 0;
                }
            }
        }
    }

    void Sing()
    {   
        zundavoice_AS.Stop();//元々なってる音を止める;
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
        karaokelocy = karaokebarloc-(karaokebarheight/2) + (karaokebarheight*(float)selectsound/soundvalue);
        nowbar = Instantiate(greatbar, new Vector3(karaokebarpos.x, karaokelocy, -5), Quaternion.identity);
        geneposx = karaokebarpos.x;
    }

    void Nowbar_moving(){
        if(nowbar!=null){
            if(realkaraoke){
                nowbar.transform.localPosition += new Vector3(onteiba/time4sec*Time.deltaTime/2f,0f,0f);
                nowbar.transform.localScale += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
            }else{
                nowbar.transform.localScale += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
            }
        }
    }

    //音程に関するスコア加点
    void Score_ontei(){
        //音程が1音ずれまではnormal、2音以上でbad
        //音を鳴らせてない場合はsafe_ms判定以上空いたらbad判定に
        //判定はnosing_timeに+deltaTimeでやってみる
        if(zunda_singnow){
            nosing_time = 0;
            if(note_ontei>=0){
                if(selectsound==note_ontei){

                    score_ontei_sum += add_good_onteiScore;
                    if(nowbar!=null)nowbar.GetComponent<SpriteRenderer>().color = Color.green;
                    ontei_hantei = 0;

                }else if(selectsound-1==note_ontei||selectsound+1==note_ontei){

                    score_ontei_sum += add_safe_onteiScore;
                    if(nowbar!=null)nowbar.GetComponent<SpriteRenderer>().color = Color.yellow;
                    ontei_hantei = 1;

                }else{

                    score_ontei_sum += add_bad_onteiScore;
                    if(nowbar!=null)nowbar.GetComponent<SpriteRenderer>().color = Color.red;
                    ontei_hantei = 2;

                }
            }
        }else{
        //歌ってないときの判定
        //休符ではないなら時間をカウントし、一定時間になったらnosingのスコアを適用
        //休符の時はカウント用の変数をリセット
            if(note_ontei>=0){
                nosing_time += Time.deltaTime;
                if(nosing_time>nosing_safetime){
                    score_ontei_sum += add_nosing_onteiScore;
                    ontei_hantei = 3;
                }
            }else{
                nosing_time = 0;
            }
        }
    }

    //タイミングに関するスコア加点
    void Score_timing(){
        /*
            使用する値
            jump_posx:生成された発声バーのx座標
            note_timingposx:次の判定のx座標
            judge_*:good、safe、bad判定の許容時間(1なら1秒　音ゲーの良判定は基本+-33msとかで調整)
            notelist[][]:画面上に生成されているノーツ情報の2次元List　note.csからlistを参照渡しで受け取っている(noteMaker())
            notelist[][0]がnote_timingposx、[1]が音程
        */
        /*
            jumpを押したタイミングによって各種加点操作を行い、その後notelistを一つ進める
        */
        Debug.Log($"GameMaker:Start Score_timing");
        Debug.Log($"GameMaker:Judge_score posx is +- {onteiba/time4sec * Judge_score_timing_good},{onteiba/time4sec * Judge_score_timing_safe},{onteiba/time4sec * Judge_score_timing_bad}");
        Debug.Log($"jump_posx = {jump_posx}, note_timingposx = {note_timingposx}, note_ontei = {note_ontei}");
        if(note_number < notecount){
            timing_karauchi = false;
            if((jump_posx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_good)) && (jump_posx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_good))){
                // ± onteiba/time4sec * Judge_score_timing_good がgood判定のx範囲
                // タイミングがgoodの場合の処理をする
                // note_onteiとnote_timingposxの更新
                note_number++;
                before_note = note_ontei;
                if(note_number < notecount){
                    note_timingposx = notelist[note_number][0];
                }
                note_ontei = notelist[note_number-1][1];
                if(select_difficult==1){
                    normal_change_ontei();
                }
                //add_good_timingScoreスコア追加
                if(note_ontei >= 0){
                    Debug.Log("GameMaker:Add_good_timingScore");
                    score_timing_sum += add_good_timingScore;
                    count_good_timing++;
                    timing_hantei = 0;
                }
            }else if((jump_posx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_safe)) && (jump_posx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_safe))){
                // タイミングがsafeの場合の処理をする
                // note_onteiとnote_timingposxの更新
                note_number++;
                before_note = note_ontei;
                if(note_number < notecount)note_timingposx = notelist[note_number][0];
                note_ontei = notelist[note_number-1][1];
                if(select_difficult==1){
                    normal_change_ontei();
                }
                //add_safe_timingScoreスコア追加
                if(note_ontei >= 0){
                    Debug.Log("GameMaker:Add_safe_timingScore");
                    score_timing_sum += add_safe_timingScore;
                    count_safe_timing++;
                    timing_hantei = 1;
                }
            }else if((jump_posx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_bad)) && (jump_posx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_bad))){
                // タイミングが悪い場合の処理をする
                // note_onteiとnote_timingposxの更新
                
                note_number++;
                before_note = note_ontei;
                if(note_number < notecount)note_timingposx = notelist[note_number][0];
                note_ontei = notelist[note_number-1][1];
                if(select_difficult==1){
                    normal_change_ontei();
                }
                //add_bad_timingScoreスコア追加
                if(note_ontei >= 0){
                    Debug.Log("GameMaker:Add_safe_timingScore");
                    score_timing_sum += add_bad_timingScore;
                    count_bad_timing++;
                    timing_hantei = 2;
                }
            }else{
                //badの範囲外の場合の判定はなし(から撃ちあり)
                Debug.Log("GameMaker:NO_Add_good_timingScore");
                timing_karauchi = true;
            }
        }else{
            Debug.Log("GameMaker:note_number==notecount");
            timing_karauchi = true;
        }
        Debug.Log($"GameMaker:note_number = {note_number}, notecount = {notecount}");
    }

    //ノーツをタップせずスルーした場合
    //badの範囲外に出たとき専用に呼ばれる(これ作る必要ないかも)
    void Score_timing_pass(){
        if(note_number < notecount){
            // note_onteiとnote_timingposxの更新
            note_number++;
            before_note = note_ontei;
            note_ontei = notelist[note_number-1][1];
            if(select_difficult==1){
                 normal_change_ontei();
            }
            if(note_number < notecount){
                note_timingposx = notelist[note_number][0];
            }
            if(note_ontei >= 0){
                score_timing_sum += add_bad_timingScore;
                count_pass_timing++;
                Debug.Log("Score_timign_pass");
                timing_hantei = 3;
            }
        }
    }

    //スコアゲージの操作
    void Scoreslider_move(){
        scoreslider.value = score*slider_mag;
        if(score>face_good_score){
            fill_slider.color = Color.HSVToRGB(Time.time % 1,1,1);
        }else if(score>face_normal_score){
            fill_slider.color = Color.green;
        }else if(score>face_nogood_score){
            fill_slider.color = Color.yellow;
        }else{
            fill_slider.color = Color.red;
        }
    }

    //ずんだもんの状態を変化
    void Zunda_joutai_change(){
        if(score>face_good_score){
            zunda_joutai = 0;
        }else if(score>face_normal_score){
            zunda_joutai = 1;
        }else if(score>face_nogood_score){
            zunda_joutai = 2;
        }else if(score>face_bad_score){
            zunda_joutai = 3;
        }
    }

    //テキスト表示のヘルプ用、色つけて選択することが多いので
    void Displaytext_selecthelper(string[] str, int a, TMP_Text t){
        t.text = "";
        for(int i=0;i<str.Length;i++){
            if(i==a){
                t.text += "<color=red>";
                t.text += str[i];
                t.text += "</color>";
                t.text += "　";
            }else{
                t.text += str[i];
                t.text += "　";
            }
        }
    }

    //デバッグ用
    void ScoreTextdisplay(){
        score_display.text =  $"Timing:{score_timing_sum.ToString("f1")} ontei:{score_ontei_sum.ToString("f4")}\n";
        score_display.text += $"Score:{score.ToString("f4")}\n";
        if(notescript.musicend==true){
            score_display.text += "Music End";
        }else{
            score_display.text += "Music now";
        }
    }

    void DebugTextdisplay(){

        debug_display.text = $"jump_posx:{jump_posx.ToString("f4")}\n";
        debug_display.text += $"note_timingposx:{note_timingposx.ToString("f4")}\n";
        debug_display.text += $"note_uptimingposx:{note_timingposx + (onteiba/time4sec * Judge_score_timing_bad)}\n";
        debug_display.text += $"note_dwtimingposx:{note_timingposx - (onteiba/time4sec * Judge_score_timing_bad)}\n";
        debug_display.text += $"note_number:{note_number}, notecount:{notecount}\n";
        debug_display.text += $"good:{count_good_timing},safe:{count_safe_timing},bad:{count_bad_timing},pass:{count_pass_timing}\n";
        switch(timing_hantei){
            case 0:
                debug_display.text += "timing_hantei = good\n";
                break;
            case 1:
                debug_display.text += "timing_hantei = safe\n";
                break;
            case 2:
                debug_display.text += "timing_hantei = bad\n";
                break;
            case 3:
                debug_display.text += "timing_hantei = pass\n";
                break;
            default:
                break;
        }
        if(timing_karauchi)debug_display.text += "karauchi\n";
        else debug_display.text += "No karauchi\n";
        debug_display.text += $"note_ontei = {note_ontei}";
        switch(ontei_hantei){
            case 0:
                debug_display.text += "ontei_hantei = good\n";
                break;
            case 1:
                debug_display.text += "ontei_hantei = safe\n";
                break;
            case 2:
                debug_display.text += "ontei_hantei = bad\n";
                break;
            case 3:
                debug_display.text += "ontei_hantei = plz sing!\n";
                break;
            default:
                break;
        }
        if(onmove_started)debug_display.text += "onmove_started = true\n";
        else debug_display.text += "onmove_started = false\n";
        debug_display.text += $"moving.y = {moving.y}";
        debug_display.text += $"jump,jump2 = {jump},";
        debug_display.text += $"{jump2}\n";
    }

    //pauseを出力
    void Pausedisplay(){
        gamespeed = 0f;
        result_panel.color = new Color32(0,0,0,210);
        result2_text.text = "Pause";
        pause_select = Moving_change_int_x(pause_select,1,3);
        onmove_started = false;
        Displaytext_selecthelper(pause_words,pause_select%3,result5_text);
        if(jumpbothcheck){
            if(jump==1){
                if(pause_select%3==0){
                    //ポーズ解除
                    ispause = false;
                    result_panel.color = new Color32(255,255,255,0);
                    result2_text.text = "";
                    result5_text.text = "";
                    gamespeed = 1f;
                }else if(pause_select%3==1){
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }else if(pause_select%3==2){
                    SceneManager.LoadScene("select_song");
                }
            }
        }  
    }

    //resultを出力
    void Resultdisplay(){
        result_panel.color = new Color32(0,0,0,210);
        result1_text.text = "あなたの得点は";
        if(sec2count<-1f){
            result2_text.text = $"{score.ToString("f3")}\n";
            if(score>face_good_score){
                result2_text.color = Color.HSVToRGB(Time.time % 1,1,1);
            }else if(score>face_normal_score){
                result2_text.color = Color.green;
            }else if(score>face_nogood_score){
                result2_text.color = Color.yellow;
            }else{
                result2_text.color = Color.red;
            }
        }
        if(sec2count<-2f){
            if(score>face_good_score){
                result2_text.text += "EXCELLENT!";
            }else if(score>face_normal_score){
                result2_text.text += "GREAT!";
            }else if(score>face_nogood_score){
                result2_text.text += "ok";
            }else{
                result2_text.text += "TERRIBLE!";
            }
        }
        if(sec2count<-3f){
            result3_text.text = $"リズム:{score_timing_sum.ToString("f2")}\n";
            result3_text.text += $"　音程:{score_ontei_sum.ToString("f2")}\n";
            result4_text.text = $"good:{count_good_timing} safe:{count_safe_timing}\n";
            result4_text.text += $"bad:{count_bad_timing} pass:{count_pass_timing}\n";
            result_select = Moving_change_int_x(result_select,1,3);
            onmove_started = false;
            if(result_select%3==0){
                result5_text.text = $"　<color=red>もう一度遊ぶ</color>　曲選択　タイトルに戻る";
            }else if(result_select%3==1){
                result5_text.text = $"　もう一度遊ぶ　<color=red>曲選択</color>　タイトルに戻る";
            }else if(result_select%3==2){
                result5_text.text = $"　もう一度遊ぶ　曲選択　<color=red>タイトルに戻る</color>";
            }
            if(jumpbothcheck){
                if(jump==1){
                    if(result_select%3==0){
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }else if(result_select%3==1){
                        SceneManager.LoadScene("select_song");
                    }else if(result_select%3==2){
                        SceneManager.LoadScene("title");
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //jumpの入力系統の処理
        if(jumpbothcheck)Jumpcheck();
        
        //リザルト表示
        if(notescript.noteend==true){
            ispause = false;
            sec2count -= Time.deltaTime;
            if(sec2count<0){
                Resultdisplay();
            }
        }
        
        //曲開始までちょっと待つ
        waitstart--;
        if(waitstart==0){
            gamespeed = 1f;
            musicstart = true;
        }else{

        }
        
        //Pause機能
        if(ispause){
            Pausedisplay();
        }


        Time.timeScale = gamespeed;

        //画面に生成される、歌った音程のバーをnowbarで管理
        //リアルタイムで動かす
        Nowbar_moving();

        //selectsoundの設定
        //先に設定したほうが良いか後に設定したほうが良いかで処理を分ける
        if(select_difficult>0){
            Apply_difficult();
        }

        //karaokebarを動かす　白いやつ
        karaokebarpos = karaokebar.transform.position;
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
                //images.sprite = speak;
                zunda_singnow = true;
                zunda_speak = true;
                //jumpが押された瞬間のxをカウント、ノーツと比べてタイミング判定を行う
                jump_posx = karaokebarpos.x;
                Score_timing();
                //音程が譜面の音程に依存するeasy、normalモードの場合ここでselectsoundを変更する必要がある
                if(select_difficult==1){
                    if(moving.y>0){
                        if(selectsound<maxsound){
                            selectsound++;
                        }
                    }else if(moving.y<0){
                        if(selectsound>0){
                            selectsound--;
                        }
                    }
                //select_difficultが0ならselectsoundをここで変える
                }else if(select_difficult==0){
                    Apply_difficult();
                }
                Sing();
            }else{
                zundavoice_AS.Stop();
                //images.sprite = mute;
                nowbar = null;
                zunda_singnow = false;
            }
        }else if(jump==1&&(beforesound!=selectsound)){
            //初回に限り2重で呼ばれている　修正案件
            Sing();//音が変わったので呼ぶ
            beforesound = selectsound;
            zunda_speak = true;
        }
        if(jumpbothcheck){
            jumpbothcheck = false;
        }
        //karaokelocを動かす　赤い奴
        if(!karaokeloc_nomoving){
            karaokelocy = karaokebarloc-(karaokebarheight/2) + (karaokebarheight*(float)selectsound/soundvalue);
        }
        karaokebarpos = karaokebar.transform.position;
        karaokeloc.transform.position = new Vector3(karaokebarpos.x,karaokelocy,karaokebarpos.z-3f);

        //スコア関連_タップ忘れ確認
        //ispauseならストップ
        if(karaokebarpos.x >= note_timingposx + (onteiba/time4sec * Judge_score_timing_bad)){
            if(!ispause)Score_timing_pass();
        }
        //Score_onteiは毎回呼ぶがdestroybarがtrueの時はnote生成を待つためLateに回す、ispauseならパス
        if(!destroybar&&!ispause){
            Score_ontei();
        }
        Zunda_joutai_change();

        //判定テキスト表示
        switch(timing_hantei){
            case 0:
                hantei_display.text = "<color=green>good</color>\n";
                //if(nowbar!=null)nowbar.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 1:
                hantei_display.text = "<color=yellow>safe</color>\n";
                //if(nowbar!=null)nowbar.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 2:
                hantei_display.text = "<color=red>bad</color>\n";
                //if(nowbar!=null)nowbar.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 3:
                hantei_display.text = "<color=blue>pass</color>\n";
                break;
            default:
                break;
        }
        //debug用
        if(score_mode){
            ScoreTextdisplay();
        }else{
            score_display.text = "";
        }
        if(debug_mode){
            DebugTextdisplay();
        }else{
            debug_display.text = "";
        }
    }
    void LateUpdate(){
        //新しく生成されたノーツのデータと、jumpを押し続けていた場合の処理
        if(destroybar){
            Notelistmaker(ref notescript.notes_timingposx_list);
            //destroybar直前に押したxの確認を行う
            jump_posx -= gamescreenx;
            if(jump==1){
                destroybar = false;
                if(select_difficult<2){
                    //前のノーツから引継ぎかどうかで処理を変える
                    //引継ぎならnotelist[note_number][0]!=-8.85f;
                    if(notelist[note_number][0]==-8.85f){
                        before_note = note_ontei;
                        note_ontei = notelist[note_number][1];
                    }
                    Apply_difficult();
                    if(beforesound!=selectsound){
                        Sing();
                    }
                }
                nowbar = Instantiate(greatbar, new Vector3(karaokebarpos.x, karaokelocy, -5), Quaternion.identity);
                if(!ispause){
                    Score_ontei();
                }
            }
            Score_timing();
        }
        score = score_timing_sum + score_ontei_sum;
        Scoreslider_move(); //バーを動かす
    }
}

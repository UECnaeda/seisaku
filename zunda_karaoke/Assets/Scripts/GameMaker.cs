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
    //�Q�[�����x�ω�
    public float gamespeed = 1.0f;

    //�Q�[�����[�h
    public float select_difficult = 2;

    //normal���[�h�p�̕ϐ��@�O�̉������L�^���Ă���
    //�ŏ���-1;
    float before_note = -1;

    //�f�o�b�O���[�h
    public bool score_mode = true;
    public bool debug_mode = true;
    //�V���O���g���@�킯�킩��񂩂�����https://umistudioblog.com/singletonhowto/
    public static GameMaker instance;
    /*
        ��ʃT�C�Y�cx��-8.85�`8.85
    */
    float gamescreenx = 17.7f;
    public float bpm = 158;
    public float beats = 4;
    float time4sec;

    //input system�A�z�u���l�֘A
    float sing1,sing2;
    float jump;
    float soundvalue = 30;
    int selectsound;
    int maxsound = 29;
    int beforesound = 30;
    float soundtime = 0;
    float onteiba = 18.1f; //�����̓z�̍ő剡��
    float onteitani; //�o�[�̈ړ��Ɖ����̓z�̑傫�����C���^���N�g�����邽�߂̒P��
    Gamecontrols gamecontrols;
    Vector2 moving;
    bool onmove_started = false;
    bool onmove_performing = false;

    //�����̂�����ׂĂ��킷
    public bool destroybar = false;

    //jump�@�\�����Ƃ��̃{�^��
    bool jumpbothcheck = false;
    float jump_posx = 0f;

    //���񂾂��񂪉̂��Ă邩�ǂ���
    public bool zunda_singnow = false;

    //�̂��n��
    public bool zunda_speak = false;
    
    //�Q�[������
    public bool musicstart = false;
    public int delaytime = 0;
    int waitstart = 90;

    //����̌����� �v���X�}�C�i�Xms�\�L
    public float Judge_score_timing_good = 0.033f;
    public float Judge_score_timing_safe = 0.075f;
    public float Judge_score_timing_bad = 0.125f;

    //�X�R�A�֘A
    //music�I�u�W�F�N�g��note�X�N���v�g���烊�X�g��l�̂ݎQ��
    public GameObject music;
    public note notescript;
    //note�Ɋւ�����List
    List<List<float>> notelist = new List<List<float>>();
    int note_number = 0;
    int notecount;

    //����Ɏg���m�[�c�̏��
    //1�Ԗڂ̃m�[�c�ƃo�[���d�Ȃ��Ă�Ƃ��A�����posx������̂�2�Ԗڂ̃m�[�c
    //note_number�̈ʒu�̒l��note_timingposx�Anote_number-1�̈ʒu�̒l��note_ontei��
    float note_timingposx;
    float note_ontei = -1;

    //�X�R�A�v�Z�p

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

    //���񂾂̊��ς���p

    public float face_good_score = 100f;
    public float face_normal_score = 66f;
    public float face_nogood_score = 33f;
    public float face_bad_score = 0f;

    public int zunda_joutai = 0;

    public TMP_Text score_display;
    public TMP_Text hantei_display;
    //debug�p��

    public TMP_Text debug_display;
    float timing_ms;
    //0~3��good^pass
    int timing_hantei;
    int ontei_hantei;

    //���猂������

    bool timing_karauchi;

    //��������z
    public GameObject greatbar;
    GameObject nowbar;

    //���������ꏊ
    float geneposx;

    //�J���I�P�o�[
    public GameObject karaokeloc;
    public GameObject karaokebar;

    //�o�[�̑傫��
    float karaokebarheight = 8;

    //���P�[�V����
    float karaokebarloc = 0.97f;//�{���̒l����+0.13f���Ă��������@�Ȃ񂩂���Ă�
    float karaokelocy;

    //�o�[
    Vector3 karaokebarpos;
    public Slider scoreslider;
    //�o�[�̓������ǂꂾ�����炩�ɂ��邩
    int slider_mag = 100;
    public Image fill_slider;

    //���ݒ�
    public AudioClip a3e, a3si,a4so, a4u, a5a, a5se, b3u, b4a, b5i, c4o, c4sa, c5e, c5si,c6u, d4e, d4si, d5so, d5u, e4u, e5a, f4o, f4sa, f5e, f5si, g3o, g3sa, g4e, g4si, g5so, g5u;
    public AudioSource zundavoice_AS;
    public int volume_music;
    public int volume_voice;

    //result�p
    float sec2count;
    [SerializeField] TMP_Text result1_text;
    [SerializeField] TMP_Text result2_text;
    [SerializeField] TMP_Text result3_text;
    [SerializeField] TMP_Text result4_text;
    [SerializeField] TMP_Text result5_text;
    [SerializeField] Image result_panel;
    int result_select = 0;


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
        //�V���O���g���̎����@�����ȊO��instance����������f�X�g���C����
        if (instance == null)
        {
            // ���g���C���X�^���X�Ƃ���
            instance = this;
        }
        else
        {
            //
            //�C���X�^���X���������݂��Ȃ��悤�ɁA���ɑ��݂��Ă����玩�g����������
            Destroy(gameObject);
        }
        Initial_Value();
        SetFps(setfpsvalue);
        //slider�����ݒ�
        scoreslider = GameObject.Find("Slider").GetComponent<Slider>();
        scoreslider.maxValue = slider_mag*100;
        music = GameObject.Find("music");
        notescript = music.GetComponent<note>();
        zundavoice_AS = GetComponent<AudioSource>();
        zundavoice_AS.volume = ((float)volume_voice)/10;
        Debug.Log($"volume_voice = {volume_voice},ZAS.volume = {zundavoice_AS.volume}");
        //bpm�Abeats���Ƃ肠���������Őݒ�
        time4sec = 240*beats/bpm; //4���ߏI���܂ł̎���
        sec2count = time4sec;
        onteitani = onteiba/gamescreenx; //����Ƀ^�C���f���^�����炿�傤�ǂ悭�Ȃ�͂�
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
        gamecontrols.Enable();
        InputSystem.pollingFrequency = setfpsvalue*2+1;
    }
    private void OnDestroy()
    {
        // ���g�ŃC���X�^���X������Action�N���X��IDisposable���������Ă���̂ŁA
        // �K��Dispose����K�v������
        /*
        gamecontrols.Player.Jumpboth.started -= OnJumpboth;
        gamecontrols.Player.Jumpboth.performed -= OnJumpboth;
        gamecontrols.Player.Jumpboth.canceled -= OnJumpboth;
        gamecontrols.Player.Move.started -= OnMove;
        gamecontrols.Player.Move.performed -= OnMove;
        gamecontrols.Player.Move.canceled -= OnMove;
        */
        gamecontrols.Dispose();
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

    // �����ꂽ�u�Ԃ̃R�[���o�b�N
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
        jumpbothcheck = true;
        jump = context.ReadValue<float>();
    }
    void Initial_Value(){

        //�Q�[�����[�h
        select_difficult = select_songBehaviour.select_difficult;

        //����
        volume_music = titleBehaviour.volume_music;
        volume_voice = titleBehaviour.volume_voice;

        //�^�C�~���O����
        delaytime = titleBehaviour.delaytime;

        //���莞��(ms)
        Judge_score_timing_good = titleBehaviour.Judge_score_timing_good;
        Judge_score_timing_safe = titleBehaviour.Judge_score_timing_safe;
        Judge_score_timing_bad = titleBehaviour.Judge_score_timing_bad;

        //�X�R�A(����)
        if(select_difficult==0){
            //easy���[�h�͉����֌W�Ȃ�
            add_good_onteiScore = 0f;
            add_safe_onteiScore = 0f;
            add_bad_onteiScore = 0f;
        }else{
            add_good_onteiScore = titleBehaviour.add_good_onteiScore;
            add_safe_onteiScore = titleBehaviour.add_safe_onteiScore;
            add_bad_onteiScore = titleBehaviour.add_bad_onteiScore;
        }
        add_nosing_onteiScore = titleBehaviour.add_nosing_onteiScore;

        //�X�R�A(�^�C�~���O)
        add_good_timingScore = titleBehaviour.add_good_timingScore;
        add_safe_timingScore = titleBehaviour.add_safe_timingScore;
        add_bad_timingScore = titleBehaviour.add_bad_timingScore;
        setfpsvalue = titleBehaviour.setfpsvalue;

        //�X�R�A(�J�E���g��)
        count_good_timing = 0;
        count_safe_timing = 0;
        count_bad_timing = 0;
        count_pass_timing = 0;

        //�X�R�A���v
        if(select_difficult==0){
            score_ontei_sum = 0;
            score_timing_sum = 60;
        }else{
            score_ontei_sum = 40;
            score_timing_sum = 40;
        }

        //�J���ҋ@����
        //���̂����J�E���g���ꂽ��
        waitstart = 90;

        //�ȏ��
        bpm = 158;
        beats = 4;

        //result�����ݒ�
        result_panel.color = new Color32(255,255,255,0);
        result1_text.text = "";
        result2_text.text = "";
        result3_text.text = "";
        result4_text.text = "";
        result5_text.text = "";
    }

    //normal�̏�����������Ƃ�₱�����̂ŌʂŊ֐������
    //note_ontei�̒l���X�V�����^�C�~���O�ŌĂ�
    void normal_change_ontei(){
        int i=0;
        if(before_note==-1||before_note==note_ontei){
                
        }else if(before_note>note_ontei){
        //�O�̉��������̉�������Ȃ�1�񉺂ɓ��͂���K�v������
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
            //easy���[�h
            //���������ǔ�
            if(note_ontei>=0){
                selectsound = (int)note_ontei;
            }
        }else if(select_difficult==1){
            //normal���[�h
            //������move�����邽�тɕω�������
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
            //hard���[�h�A�R���g���[���[�A�X�e�B�b�N1�{
            //�����̓_�C���N�g�ɕω�����
            selectsound = (int)((soundvalue-1)*(moving.y+1)/2);
        }
        
    }

    void Sing()
    {   
        zundavoice_AS.Stop();//���X�Ȃ��Ă鉹���~�߂�;
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

    //�����Ɋւ���X�R�A���_
    void Score_ontei(){
        //������1������܂ł�normal�A2���ȏ��bad
        //����点�ĂȂ��ꍇ��safe_ms����ȏ�󂢂���bad�����
        //�����nosing_time��+deltaTime�ł���Ă݂�
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
        //�̂��ĂȂ��Ƃ��̔���
        //�x���ł͂Ȃ��Ȃ玞�Ԃ��J�E���g���A��莞�ԂɂȂ�����nosing�̃X�R�A��K�p
        //�x���̎��̓J�E���g�p�̕ϐ������Z�b�g
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

    //�^�C�~���O�Ɋւ���X�R�A���_
    void Score_timing(){
        /*
            �g�p����l
            jump_posx:�������ꂽ�����o�[��x���W
            note_timingposx:���̔����x���W
            judge_*:good�Asafe�Abad����̋��e����(1�Ȃ�1�b�@���Q�[�̗ǔ���͊�{+-33ms�Ƃ��Œ���)
            notelist[][]:��ʏ�ɐ�������Ă���m�[�c����2����List�@note.cs����list���Q�Ɠn���Ŏ󂯎���Ă���(noteMaker())
            notelist[][0]��note_timingposx�A[1]������
        */
        /*
            jump���������^�C�~���O�ɂ���Ċe����_������s���A���̌�notelist����i�߂�
        */
        Debug.Log($"GameMaker:Start Score_timing");
        Debug.Log($"GameMaker:Judge_score posx is +- {onteiba/time4sec * Judge_score_timing_good},{onteiba/time4sec * Judge_score_timing_safe},{onteiba/time4sec * Judge_score_timing_bad}");
        Debug.Log($"jump_posx = {jump_posx}, note_timingposx = {note_timingposx}, note_ontei = {note_ontei}");
        if(note_number < notecount){
            timing_karauchi = false;
            if((jump_posx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_good)) && (jump_posx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_good))){
                // �} onteiba/time4sec * Judge_score_timing_good ��good�����x�͈�
                // �^�C�~���O��good�̏ꍇ�̏���������
                // note_ontei��note_timingposx�̍X�V
                note_number++;
                before_note = note_ontei;
                if(note_number < notecount){
                    note_timingposx = notelist[note_number][0];
                }
                note_ontei = notelist[note_number-1][1];
                if(select_difficult==1){
                    normal_change_ontei();
                }
                //add_good_timingScore�X�R�A�ǉ�
                if(note_ontei >= 0){
                    Debug.Log("GameMaker:Add_good_timingScore");
                    score_timing_sum += add_good_timingScore;
                    count_good_timing++;
                    timing_hantei = 0;
                }
            }else if((jump_posx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_safe)) && (jump_posx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_safe))){
                // �^�C�~���O��safe�̏ꍇ�̏���������
                // note_ontei��note_timingposx�̍X�V
                note_number++;
                before_note = note_ontei;
                if(note_number < notecount)note_timingposx = notelist[note_number][0];
                note_ontei = notelist[note_number-1][1];
                if(select_difficult==1){
                    normal_change_ontei();
                }
                //add_safe_timingScore�X�R�A�ǉ�
                if(note_ontei >= 0){
                    Debug.Log("GameMaker:Add_safe_timingScore");
                    score_timing_sum += add_safe_timingScore;
                    count_safe_timing++;
                    timing_hantei = 1;
                }
            }else if((jump_posx <= note_timingposx + (onteiba/time4sec * Judge_score_timing_bad)) && (jump_posx >= note_timingposx - (onteiba/time4sec * Judge_score_timing_bad))){
                // �^�C�~���O�������ꍇ�̏���������
                // note_ontei��note_timingposx�̍X�V
                
                note_number++;
                before_note = note_ontei;
                if(note_number < notecount)note_timingposx = notelist[note_number][0];
                note_ontei = notelist[note_number-1][1];
                if(select_difficult==1){
                    normal_change_ontei();
                }
                //add_bad_timingScore�X�R�A�ǉ�
                if(note_ontei >= 0){
                    Debug.Log("GameMaker:Add_safe_timingScore");
                    score_timing_sum += add_bad_timingScore;
                    count_bad_timing++;
                    timing_hantei = 2;
                }
            }else{
                //bad�͈̔͊O�̏ꍇ�̔���͂Ȃ�(���猂������)
                Debug.Log("GameMaker:NO_Add_good_timingScore");
                timing_karauchi = true;
            }
        }else{
            Debug.Log("GameMaker:note_number==notecount");
            timing_karauchi = true;
        }
        Debug.Log($"GameMaker:note_number = {note_number}, notecount = {notecount}");
    }

    //�m�[�c���^�b�v�����X���[�����ꍇ
    //bad�͈̔͊O�ɏo���Ƃ���p�ɌĂ΂��(������K�v�Ȃ�����)
    void Score_timing_pass(){
        if(note_number < notecount){
            // note_ontei��note_timingposx�̍X�V
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
    }

    void Resultdisplay(){
        result_panel.color = new Color32(0,0,0,210);
        result1_text.text = "���Ȃ��̓��_��";
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
            result3_text.text = $"���Y��:{score_timing_sum.ToString("f2")}\n";
            result3_text.text += $"�@����:{score_ontei_sum.ToString("f2")}\n";
            result4_text.text = $"good:{count_good_timing} safe:{count_safe_timing}\n";
            result4_text.text += $"bad:{count_bad_timing} pass:{count_pass_timing}\n";
            if(onmove_started){
                result_select++;
                onmove_started = false;
            }
            if(result_select%2==0){
                result5_text.text = $"�@<color=red>������x�V��</color>�@�@�@�^�C�g���ɖ߂�";
            }else{
                result5_text.text = $"�@������x�V�ԁ@�@�@<color=red>�^�C�g���ɖ߂�</color>";
            }
            if(jumpbothcheck){
                if(jump==1){
                    if(result_select%2==0){
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }else{
                        SceneManager.LoadScene("title");
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(notescript.noteend==true){
            sec2count -= Time.deltaTime;
            if(sec2count<0){
                Resultdisplay();
            }
        }
        //�|�[�Y�A�ȊJ�n�܂ł�����Ƒ҂�
        waitstart--;
        if(waitstart==0){
            gamespeed = 1f;
            musicstart = true;
        }else{

        }
        Time.timeScale = gamespeed;

        //��ʂɐ��������A�̂��������̃o�[��nowbar�ŊǗ�
        //���A���^�C���œ�����
        if(nowbar!=null){
            nowbar.transform.localPosition += new Vector3(onteiba/time4sec*Time.deltaTime/2f,0f,0f);
            nowbar.transform.localScale += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
        }
        //selectsound�̐ݒ�
        //��ɐݒ肵���ق����ǂ�����ɐݒ肵���ق����ǂ����ŏ����𕪂���
        if(select_difficult>0){
            Apply_difficult();
        }
        //selectsound = (int)((soundvalue-1)*(sing1+sing2+2)/4);//�X�e�B�b�N�̌X������0�`soundvalue-1�̒l���o��
        karaokebarpos = karaokebar.transform.position;//karaokebar�𓮂����@�������
        if(waitstart>=delaytime){

        }else if(karaokebarpos.x >= gamescreenx/2){
            destroybar = true;
            karaokebar.transform.position += new Vector3((gamescreenx/time4sec*Time.deltaTime)-gamescreenx,0f,0f);
        }else{
            destroybar = false;
            karaokebar.transform.position += new Vector3(gamescreenx/time4sec*Time.deltaTime,0f,0f);
        }

        //����ύX
        zunda_speak = false;
        if(jumpbothcheck){
            if(jump==1){
                //images.sprite = speak;
                zunda_singnow = true;
                zunda_speak = true;
                //jump�������ꂽ�u�Ԃ�x���J�E���g�A�m�[�c�Ɣ�ׂă^�C�~���O������s��
                jump_posx = karaokebarpos.x;
                Score_timing();
                //���������ʂ̉����Ɉˑ�����easy�Anormal���[�h�̏ꍇ������selectsound��ύX����K�v������
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
            //����Ɍ���2�d�ŌĂ΂�Ă���@�C���Č�
            Sing();//�����ς�����̂ŌĂ�
            beforesound = selectsound;
            zunda_speak = true;
        }
        if(jumpbothcheck){
            jumpbothcheck = false;
        }
        //karaokeloc�𓮂����@�Ԃ��z
        karaokebarpos = karaokebar.transform.position;
        karaokeloc.transform.position = new Vector3(karaokebarpos.x,karaokelocy,karaokebarpos.z-3f);

        //�X�R�A�֘A_�^�b�v�Y��m�F
        if(karaokebarpos.x >= note_timingposx + (onteiba/time4sec * Judge_score_timing_bad)){
            Score_timing_pass();
        }
        //Score_ontei�͖���ĂԂ�destroybar��true�̎���note������҂���Late�ɉ�
        //easy�Anormal���[�h�̏ꍇ������selectsound��ύX����K�v������
        /*if(select_difficult<2){
            Apply_difficult();
        }*/
        if(!destroybar){
            Score_ontei();
        }
        Zunda_joutai_change();
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
        //debug�p
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
        //�V�����������ꂽ�m�[�c�̃f�[�^�ƁAjump�����������Ă����ꍇ�̏���
        if(destroybar){
            Notelistmaker(ref notescript.notes_timingposx_list);
            //destroybar���O�ɉ�����x�̊m�F���s��
            jump_posx -= gamescreenx;
            if(jump==1){
                destroybar = false;
                if(select_difficult<2){
                    //�O�̃m�[�c������p�����ǂ����ŏ�����ς���
                    //���p���Ȃ�notelist[note_number][0]!=-8.85f;
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
                Score_ontei();
            }
            Score_timing();
        }
        score = score_timing_sum + score_ontei_sum;
        Scoreslider_move(); //�o�[�𓮂���
    }
}
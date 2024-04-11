using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class titleBehaviour : MonoBehaviour
{
    [SerializeField] Sprite first_sprite;
    [SerializeField] Sprite start_sprite;
    [SerializeField] Sprite option_sprite;
    [SerializeField] Sprite exit_sprite;
    [SerializeField] Image panel;
    [SerializeField] Image zundatitle;
    [SerializeField] TMP_Text Option_text;
    [SerializeField] private AudioSource zunda_voice;
    [SerializeField] private AudioSource se;
    [SerializeField] private AudioClip AC_zunda_change_timing;
    [SerializeField] private AudioClip AC_zunda_changed;
    [SerializeField] private AudioClip AC_zunda_changenow;
    [SerializeField] private AudioClip AC_zunda_change_ms;
    [SerializeField] private AudioClip AC_zunda_finish;
    [SerializeField] private AudioClip AC_zunda_fps;
    [SerializeField] private AudioClip AC_zunda_gameend;
    [SerializeField] private AudioClip AC_zunda_kyoku_volume;
    [SerializeField] private AudioClip AC_zunda_option;
    [SerializeField] private AudioClip AC_zunda_reset;
    [SerializeField] private AudioClip AC_zunda_score_ontei;
    [SerializeField] private AudioClip AC_zunda_score_timing;
    [SerializeField] private AudioClip AC_zunda_start;
    [SerializeField] private AudioClip AC_zunda_voice_volume;

    //�Z���N�g���Ă��郂�[�h�̊Ǘ�
    int selectbutton = 0;
    bool titlesprite = true;
    bool jump = false;
    bool onmove_started = false;
    int start_select_sum = 4;

    //option�֘A
    bool mode_option = false;
    byte color_option = 50;
    int option_select_number = 0;
    int option_select_sum = 17;
    bool option_changevalue = false;

    //�󂯓n���l�@static��������
    public static int volume_music = 1;
    public static int volume_voice = 3;

    public static int delaytime = -5;

    public static float Judge_score_timing_good = 0.033f;
    public static float Judge_score_timing_safe = 0.066f;
    public static float Judge_score_timing_bad = 0.100f;

    public static float add_good_onteiScore = 0.01f;
    public static float add_safe_onteiScore = -0.01f;
    public static float add_bad_onteiScore = -0.03f;
    public static float add_nosing_onteiScore = -0.05f;

    public static float add_good_timingScore = 0.3f;
    public static float add_safe_timingScore = 0.05f;
    public static float add_bad_timingScore = -1;
    public static int setfpsvalue = 120;

    Vector2 moving;
    
    //���������
    //�C���^���N�g��hold�ɂ��Ă�����ꂻ��
    bool onmove_performing = false;

    float moving_holdtime1 = 0.7f;
    float moving_holdtime2 = 2f;
    float holdtime = 0;
    int holdcount = 0;

    //audio source


    Gamecontrols gamecontrols;

    private void OnEnable()
    {
        gamecontrols = new Gamecontrols();
        gamecontrols.Player.Move.started += OnMove;
        gamecontrols.Player.Move.canceled += OnMove;
        gamecontrols.Player.Jumppress.performed += OnJumppress;
        gamecontrols.Enable();
    }

    // ������
    private void OnDestroy()
    {
        gamecontrols.Player.Move.started -= OnMove;
        gamecontrols.Player.Move.canceled -= OnMove;
        gamecontrols.Player.Jumppress.performed -= OnJumppress;
        gamecontrols.Dispose();
    }

    void Awake(){
        SetFps(setfpsvalue);
    }

    // Start is called before the first frame update
    void Start()
    {
        Option_text.text ="";
    }
    public void SetFps(int fps){
        Application.targetFrameRate = fps;
        Time.fixedDeltaTime = 1.0f/(float)fps;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if(context.started){
            Debug.Log("context.started");
            moving = context.ReadValue<Vector2>();
            onmove_started = true;
            onmove_performing = true;
        }
        if(context.canceled){
            Debug.Log("context.canceled");
            onmove_performing = false;
            holdtime = 0;
        }
    }

    void OnJumppress(InputAction.CallbackContext context){
        jump = true;
    }


    //onmove���@�\���Ă�Ƃ��ɗ^����ꂽ�l��ύX����֐��Aint��
    //�����\����if���������Ȃ肷�����̂�
    int Moving_change_int(int a, int b){
        if(onmove_started){
            if(moving.y>0){
                a += b;
            }else{
                a -= b;
            }
        }
        return a;
    }

    //float��
    float Moving_change_float(float a, float b){
        if(onmove_started){
            if(moving.y>0){
                a += b;
            }else{
                a -= b;
            }
        }
        return a;
    }

    void Option_start(){
        //�w�i���Â߂�
        panel.sprite = first_sprite;
        panel.color = new Color32(color_option,color_option,color_option,255);
        zundatitle.color = new Color32(color_option,color_option,color_option,255);
        //��ɃI�v�V������ʂ�\��
        Option_text.color = new Color32(255,255,255,255);
    }

    void Initial_value(){
        //�����l�ݒ�

        volume_music = 3;
        volume_voice = 3;

        delaytime = -5;

        Judge_score_timing_good = 0.033f;
        Judge_score_timing_safe = 0.066f;
        Judge_score_timing_bad = 0.100f;

        add_good_onteiScore = 0.01f;
        add_safe_onteiScore = -0.01f;
        add_bad_onteiScore = -0.03f;
        add_nosing_onteiScore = -0.05f;

        add_good_timingScore = 0.3f;
        add_safe_timingScore = 0.05f;
        add_bad_timingScore = -1;
        setfpsvalue = 120;
        option_changevalue = false;
    }

    void Option_texthelper(int i, int selected_number, string s, int value_int, float value_float, string ts, bool isfloat, bool isnewline){
        /*
            i,selected_number:�I�����Ă��镶����ƕ�����̔ԍ�
            s:�\�����镶����
            value_int:�\������l(int)
            value_float:�\������l(float)
            ts:ToString�I�v�V�����p
            isfloat,isnewline:float�A���s�K�v����
        */
        if(i==selected_number){
            if(option_changevalue){
                if(isfloat){
                    Option_text.text += $"<color=red>{s}<u>{value_float.ToString(ts)}</u></color>�@";
                    if(isnewline)Option_text.text += "\n";
                }
                else{
                    Option_text.text += $"<color=red>{s}<u>{value_int}</u></color>�@";
                    if(isnewline)Option_text.text += "\n";
                }
            }else{
                if(isfloat){
                    Option_text.text += $"<color=red>{s}{value_float.ToString(ts)}</color>�@";
                    if(isnewline)Option_text.text += "\n";
                }
                else {
                    Option_text.text += $"<color=red>{s}{value_int}</color>�@";
                    if(isnewline)Option_text.text += "\n";
                }
            }
        }else{
            if(isfloat){
                Option_text.text += $"{s}{value_float.ToString(ts)}�@";
                if(isnewline)Option_text.text += "\n";
            }else{
                Option_text.text += $"{s}{value_int}�@";
                if(isnewline)Option_text.text += "\n";
            }
        }
    }

    void Option_writetext(){
        //�e��ʂ̑J�ڂ�ǉ�
        //��肢�������肻�������ǂƂ肠�����͋Z
        int i = option_select_number % option_select_sum;
        Option_text.text = "<color=yellow>���ʐݒ�</color>\n";
        Option_texthelper(i,0,"�@��(0�`10)�@",volume_music,0f,"",false,false);
        Option_texthelper(i,1,"�@��(0�`10)�@",volume_voice,0f,"",false,true);
        Option_text.text += "<color=yellow>���蒲��</color>\n";
        Option_texthelper(i,2,"�@�^�C�~���O����(+�Ŕ���x��)�@",delaytime,0f,"",false,true);
        Option_text.text += "<color=yellow>���莞��(ms)</color>\n";
        Option_texthelper(i,3,"�@good: ",0,Judge_score_timing_good,"f3",true,false);
        Option_texthelper(i,4,"safe: ",0,Judge_score_timing_safe,"f3",true,false);
        Option_texthelper(i,5,"bad: ",0,Judge_score_timing_bad,"f3",true,true);
        Option_text.text += "<color=yellow>�X�R�A(����)</color>\n";
        Option_texthelper(i,6,"�@good: ",0,add_good_onteiScore,"f2",true,false);
        Option_texthelper(i,7,"safe: ",0,add_safe_onteiScore,"f2",true,true);
        Option_texthelper(i,8,"�@bad: ",0,add_bad_onteiScore,"f2",true,false);
        Option_texthelper(i,9,"nosing: ",0,add_nosing_onteiScore,"f2",true,true);
        Option_text.text += "<color=yellow>�X�R�A(�^�C�~���O)</color>\n";
        Option_texthelper(i,10,"�@good: ",0,add_good_timingScore,"f2",true,false);
        Option_texthelper(i,11,"safe: ",0,add_safe_timingScore,"f2",true,false);
        Option_texthelper(i,12,"bad: ",0,add_bad_timingScore,"f2",true,true);
        Option_text.text += "<color=yellow>FPS�ݒ�</color>\n";
        Option_texthelper(i,13,"�@fps: ",setfpsvalue,0,"",false,true);
        Option_text.text += "\n";
        if(i==14){
            Option_text.text += "<color=red>�@�����ݒ�ɖ߂�</color>\n";
        }else{
            Option_text.text += "�@�����ݒ�ɖ߂�\n";
        }
        if(i==15){
            Option_text.text += "<color=red>�@�I�v�V���������</color>\n";
        }else{
            Option_text.text += "�@�I�v�V���������\n";
        }
        if(i==16){
            Option_text.text += "<color=red>�@�Q�[�����I��</color>\n";
        }else{
            Option_text.text += "�@�Q�[�����I��\n";
        }
    }

    void Option_finish(){
        Option_text.text = "";
        panel.color = new Color32(255,255,255,255);
        zundatitle.color = new Color32(255,255,255,255);
        option_select_number = 0;
        selectbutton = 0;
        option_changevalue = false;
        SetFps(setfpsvalue);
    }

    void Option_behaviour(){
        //option_select_number�ŃI�v�V�����I���Ǘ�
        if(jump){
            option_changevalue = !option_changevalue;
        }
        if(option_changevalue){
            //�I��ł��鍀��
            //��x�����I��ł��鎞�̃{�C�X�𗬂�
            if(jump){
                zunda_voice.Stop();
                zunda_voice.PlayOneShot(AC_zunda_changenow);
            }
            int i = option_select_number % option_select_sum;
            if(i==0){
                volume_music = Moving_change_int(volume_music,1)%11;
                if(volume_music<0)volume_music += 11;
            }else if(i==1){
                volume_voice = Moving_change_int(volume_voice,1)%11;
                if(volume_voice<0)volume_voice += 11;
            }else if(i==2){
                delaytime = Moving_change_int(delaytime,1);
            }else if(i==3){
                Judge_score_timing_good = Moving_change_float(Judge_score_timing_good,0.001f);
            }else if(i==4){
                Judge_score_timing_safe = Moving_change_float(Judge_score_timing_safe,0.001f);
            }else if(i==5){
                Judge_score_timing_bad = Moving_change_float(Judge_score_timing_bad,0.001f);
            }else if(i==6){
                add_good_onteiScore = Moving_change_float(add_good_onteiScore,0.01f);
            }else if(i==7){
                add_safe_onteiScore = Moving_change_float(add_safe_onteiScore,0.01f);
            }else if(i==8){
                add_bad_onteiScore = Moving_change_float(add_bad_onteiScore,0.01f);
            }else if(i==9){
                add_nosing_onteiScore = Moving_change_float(add_nosing_onteiScore,0.01f);
            }else if(i==10){
                add_good_timingScore = Moving_change_float(add_good_timingScore,0.05f);
            }else if(i==11){
                add_safe_timingScore = Moving_change_float(add_safe_timingScore,0.05f);
            }else if(i==12){
                add_bad_timingScore = Moving_change_float(add_safe_timingScore,0.05f);
            }else if(i==13){
                setfpsvalue = Moving_change_int(setfpsvalue,1);
                if(setfpsvalue<30)setfpsvalue = 30;
            }else if(i==14){
                Initial_value();
            }else if(i==15){
                //�I�v�V��������
                mode_option = false;
                Option_finish();
            }else if(i==16){
                //�Q�[�����I��
                Application.Quit();

            }
        }else{
            if(jump){
                zunda_voice.Stop();
                zunda_voice.PlayOneShot(AC_zunda_changed);
            }
            //�ォ��1�`14�Ɛݒ肵���̂Ő��l�������t�ɂ��Ȃ��Ƃ����Ȃ�
            option_select_number = Moving_change_int(option_select_number,-1);
            if(option_select_number<0){
                //number % sum���}�C�i�X�ɂȂ�Ɩʓ|�Ȃ̂�
                option_select_number += option_select_sum;
            }
            //�l���ω����Ă���Ȃ炸�񂾂��������ׂ点��
            if(onmove_started){
                zunda_voice_forOption(option_select_number);
            }
        }
        if(mode_option)Option_writetext();
    }

    //���񂾂���̃{�C�X�֘A�̑���
    void zunda_voice_forStart(int a){
        int b = a%start_select_sum;
        if(b==0){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_start);
        }else if(b==1){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_option);
        }else if(b==2){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_gameend);
        }
    }

    void zunda_voice_forOption(int a){
        int b = a%option_select_sum;
        if(b==0){
            
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_kyoku_volume);
        }else if(b==1){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_voice_volume);
        }else if(b==2){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_change_timing);
        }else if(b==3){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_change_ms);
        }else if(b==6){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_score_ontei);
        }else if(b==10){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_score_timing);
        }else if(b==13){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_fps);
        }else if(b==14){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_reset);
        }else if(b==15){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_finish);
        }else if(b==16){
            zunda_voice.Stop();
            zunda_voice.PlayOneShot(AC_zunda_gameend);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //zunda_voice.volume = (float)volume_voice/10;
        if(mode_option){
            //�I�v�V�������[�h���ǂ���
            Option_behaviour();
        }else{
            //���j���[���[�h
            selectbutton = Moving_change_int(selectbutton,-1);
            if(selectbutton<0)selectbutton += start_select_sum;
            if(selectbutton%start_select_sum==0){
                panel.sprite = start_sprite;

            }else if(selectbutton%start_select_sum==1){
                panel.sprite = option_sprite;
            }else{
                panel.sprite = exit_sprite;
            }
            if(onmove_started){
                zunda_voice_forStart(selectbutton%start_select_sum);
            }
            if(jump){
                if(selectbutton%start_select_sum==0){
                    SceneManager.LoadScene("select_song");
                }else if(selectbutton%3==1){
                    mode_option = true;
                    Option_start();
                }else{
                    Application.Quit();
                }
            }
        }
        onmove_started = false;
        if(onmove_performing){
            holdtime += Time.deltaTime;
            if(holdtime>moving_holdtime2){
                onmove_started = true;
            }else if(holdtime>moving_holdtime1){
                holdcount++;
                if(holdcount%4==0){
                    onmove_started = true;
                }
            }
        }
        jump = false;
    }
}

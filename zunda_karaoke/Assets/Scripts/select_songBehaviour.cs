using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;


public class select_songBehaviour : MonoBehaviour
{
    [SerializeField] Image easy_no_select;
    [SerializeField] Image normal_no_select;
    [SerializeField] Image hard_no_select;
    [SerializeField] Image option_no_select;
    [SerializeField] AudioSource AS_zunda_voice;
    [SerializeField] AudioClip AC_easy;
    [SerializeField] AudioClip AC_normal;
    [SerializeField] AudioClip AC_hard;
    [SerializeField] Image tutorial_panel;
    [SerializeField] TMP_Text tutorialtext1;
    [SerializeField] TMP_Text tutorialtext2;
    [SerializeField] VideoPlayer demo_VP;
    [SerializeField] VideoClip easy_barmove_mp4,normal_barmove_mp4,hard_barmove_mp4,easy_notemove_mp4,normal_notemove_mp4,hard_notemove_mp4;
    [SerializeField] RawImage demo_RI;

    bool difficult_selected = false;

    //���\�J�ڂ�����̂Ń��[�h�ԍ����Ǘ�
    //��Փx�I����0,�`���[�g���A���I����1�Ƃ���
    int select_thismode = 0;
    public static int select_difficult = 2;
    int select_difficult_sum = 4;
    int option_number = 3;
    int title_number = 3;
    
    //tutorial�֘A
    string[] tutorial_words = {"�͂�","�X�L�b�v","��O�ɖ߂�"};
    string[] realkaraoke_words = {"�m�[�c������(���񂽂�)\n","�o�[������(�ނ�������)\n","��O�ɖ߂�"};
    int select_tutorial = 0;
    public static bool tutorialmode = false;

    //realkaraoke
    public static bool realkaraoke = false;
    int select_realkaraoke = 0;


    //input system�֘A
    Gamecontrols gamecontrols;
    float moving_holdtime1 = 0.7f;
    float moving_holdtime2 = 2f;
    float holdtime = 0;
    int holdcount = 0;
    bool onmove_started = false;
    bool onmove_performing = false;
    bool jump;
    Vector2 moving;

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
        gamecontrols.Player.Jumpboth.performed -= OnJumppress;
        gamecontrols.Dispose();
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
            onmove_performing = false;
            holdtime = 0;
        }
    }

    void OnJumppress(InputAction.CallbackContext context){
        jump = true;
    }

    void zunda_voice_changedifficult(int a){
        int b = a% select_difficult_sum;
        if(b==0){
            AS_zunda_voice.Stop();
            AS_zunda_voice.PlayOneShot(AC_easy);
        }else if(b==1){
            AS_zunda_voice.Stop();
            AS_zunda_voice.PlayOneShot(AC_normal);
        }else if(b==2){
            AS_zunda_voice.Stop();
            AS_zunda_voice.PlayOneShot(AC_hard);
        }else if(b==3){

        }
    }

    //select_difficult�ɍ��킹�ĕ\����ύX
    void Change_difficult(){
        easy_no_select.color = new Color32(255,255,255,210);
        normal_no_select.color = new Color32(255,255,255,210);
        hard_no_select.color = new Color32(255,255,255,210);
        option_no_select.color = new Color32(255,255,255,210);
        if(onmove_started){
            zunda_voice_changedifficult(select_difficult);
        }
        if(select_difficult==0){
            easy_no_select.color = new Color32(255,255,255,0);
        }else if(select_difficult==1){
            normal_no_select.color = new Color32(255,255,255,0);
        }else if(select_difficult==2){
            hard_no_select.color = new Color32(255,255,255,0);
        }else if(select_difficult==3){
            option_no_select.color = new Color32(255,255,255,0);
        }
    }

    int Moving_change_int_x(int a, int b, int c){
        if(a<=0){
            a+=c;
        }
        if(onmove_started){
            if(moving.x>0){
                a += b;
            }else{
                a -= b;
            }
        }
        return a % c;
    }

    int Moving_change_int_y(int a, int b, int c){
        if(a<=0){
            a+=c;
        }
        if(onmove_started){
            if(moving.y>0){
                a += b;
            }else{
                a -= b;
            }
        }
        return a % c;
    }

    //a��b��c�������������Aa%d��Ԃ��֐�
    //a<0�Ȃ�a=a+d�����炩���߂���
    int Moving_change_int_xy(int a, int b,int c,int d){
        if(a<=0){
            a+= d;
        }
        if(onmove_started){
            if(Mathf.Abs(moving.x)>Mathf.Abs(moving.y)){
                if(moving.x>0){
                    a += b;
                }else{
                    a -= b;
                }
            }else{
                if(moving.y>0){
                    a += c;
                }else{
                    a -= c;
                }
            }
        }
        return a % d;
    }

    //str[a]�����Ԃ�����p�̊֐�
    //�o�͉͂����
    void Displaytext_selecthelper(string[] str, int a, TMP_Text t){
        t.text = "";
        for(int i=0;i<str.Length;i++){
            if(i==a){
                t.text += "<color=red>";
                t.text += str[i];
                t.text += "</color>";
                t.text += "�@";
            }else{
                t.text += str[i];
                t.text += "�@";
            }
        }

    }

    void Video_start(){
        demo_VP.Stop();
        if(select_realkaraoke==0){
            if(select_difficult==0){
                demo_VP.clip = easy_notemove_mp4;
            }else if(select_difficult==1){
                demo_VP.clip = normal_notemove_mp4;
            }else if(select_difficult==2){
                demo_VP.clip = hard_notemove_mp4;
            }
            demo_VP.Play();
        }else if(select_realkaraoke==1){
            if(select_difficult==0){
                demo_VP.clip = easy_barmove_mp4;
            }else if(select_difficult==1){
                demo_VP.clip = normal_barmove_mp4;
            }else if(select_difficult==2){
                demo_VP.clip = hard_barmove_mp4;
            }
            demo_VP.Play();
        }
    }

    void Realkaraoke(){
        demo_RI.color = new Color32(255,255,255,255);
        if(difficult_selected){
            Video_start();
            difficult_selected = false;
        }
        tutorial_panel.color = new Color32(0,0,0,210);
        select_realkaraoke = Moving_change_int_y(select_realkaraoke,-1,3);
        if(onmove_started){
            if(select_realkaraoke==0||select_realkaraoke==1){
                Video_start();
            }
        }
        tutorialtext1.text = "������@��I��ł�������";
        Displaytext_selecthelper(realkaraoke_words,select_realkaraoke,tutorialtext2);
        if(jump){
            if(select_realkaraoke==0){
                realkaraoke = false;
                select_thismode++;
            }else if(select_realkaraoke==1){
                realkaraoke = true;
                select_thismode++;
            }else if(select_realkaraoke==2){
                Initial_tutorial();
                demo_VP.Stop();
                select_thismode--;
                demo_RI.color = new Color32(255,255,255,0);
            }
        }
    }

    void Start_tutorial(){
        tutorial_panel.color = new Color32(0,0,0,210);
        select_tutorial = Moving_change_int_x(select_tutorial,1,3);
        tutorialtext1.text = "�`���[�g���A�����v���C���܂����H(����)";
        Displaytext_selecthelper(tutorial_words,select_tutorial,tutorialtext2);
        if(jump){
            if(select_tutorial==0){
                tutorialmode = true;
                SceneManager.LoadScene("otogame");
            }else if(select_tutorial==1){
                tutorialmode = false;
                SceneManager.LoadScene("otogame");
            }else if(select_tutorial==2){
                Initial_tutorial();
                select_thismode--;
            }
        }
    }

    void Initial_tutorial(){
        tutorial_panel.color = new Color32(255,255,255,0);
        select_tutorial = 0;
        tutorialtext1.text = "";
        tutorialtext2.text = "";
    }

    //���肳�ꂽ���̋���
    void Deside_difficult_mode(){
        if(select_difficult==title_number){
            SceneManager.LoadScene("title");
        }else{
            difficult_selected = true;
            select_thismode++;
        }
    }

    //update���Ă΂�邽�тɕύX������input system�Ɋւ���l�̐ݒ�
    //�Ă΂ꂽ�u�Ԃ���on�ɂ�����onmove_started�Ajump�̏�����
    void Update_input_system_values(){
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

    // Start is called before the first frame update
    void Start()
    {
        select_thismode = 0;
        Initial_tutorial();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(select_thismode==0){
            //��Փx�I��
            select_difficult = Moving_change_int_xy(select_difficult,1,-2,select_difficult_sum);
            Change_difficult();
            if(jump){
                Deside_difficult_mode();
            }
        }else if(select_thismode==1){
            //�m�[�c�𓮂������o�[�𓮂�����
            Realkaraoke();

        }else if(select_thismode==2){
            //�`���[�g���A���I�����
            Start_tutorial();
        }
        //input system�֘A�̒l���X�V
        Update_input_system_values();
    }
}

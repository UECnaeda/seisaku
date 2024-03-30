using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public static int select_difficult = 2;
    int select_difficult_sum = 4;
    int option_number = 3;
    bool option_hoge = false;


    //input system関連
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
        var keyboard = Keyboard.current;
        if(keyboard!=null){
            keyboard.onTextInput += OnTextInput;
        }
        gamecontrols.Player.Move.started += OnMove;
        gamecontrols.Player.Move.canceled += OnMove;
        gamecontrols.Player.Jumppress.performed += OnJumppress;
        gamecontrols.Enable();
    }

    // 無効化
    private void OnDestroy()
    {
        gamecontrols.Player.Move.started -= OnMove;
        gamecontrols.Player.Move.canceled -= OnMove;
        gamecontrols.Player.Jumpboth.performed -= OnJumppress;
        var keyboard = Keyboard.current;
        if(keyboard != null)keyboard.onTextInput -= OnTextInput;
        gamecontrols.Dispose();
    }
    private void OnTextInput(char ch)
    {
        // 入力された文字を文字コード（16進数）と共に表示
        print($"OnTextInput: {ch}({(int) ch:X02})");
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

    //select_difficultに合わせて表示を変更
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

    int Moving_change_int_x(int a, int b){
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

    void Start_Option(){
        
    }

    //決定された時の挙動
    void Deside_difficult_mode(){
        if(select_difficult==option_number){
            Start_Option();
        }else{
            SceneManager.LoadScene("otogame");
        }
    }

    //updateが呼ばれるたびに変更したいinput systemに関する値の設定
    //呼ばれた瞬間だけonにしたいonmove_started、jumpの初期化
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
        
    }

    // Update is called once per frame
    void Update()
    {
        select_difficult = Moving_change_int_x(select_difficult,1);
        if(select_difficult<0){
            select_difficult += select_difficult_sum;
        }
        select_difficult = select_difficult % select_difficult_sum;
        Change_difficult();
        if(jump){
            Deside_difficult_mode();
        }
        //input system関連の値を更新
        Update_input_system_values();
    }
}

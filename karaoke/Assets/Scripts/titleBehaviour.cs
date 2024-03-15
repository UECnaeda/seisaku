using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class titleBehaviour : MonoBehaviour
{
    [SerializeField] Sprite startselect;
    [SerializeField] Sprite optionselect;
    [SerializeField] Image panel;
    int selectbutton = 0;
    bool titlesprite = true;
    bool jump = false;
    Gamecontrols gamecontrols;

    private void OnEnable()
    {
        gamecontrols = new Gamecontrols();
        gamecontrols.Player.Move.started += OnMove;
        gamecontrols.Player.Jumpboth.performed += OnJumpboth;
        gamecontrols.Enable();
    }

    // –³Œø‰»
    private void OnDestroy()
    {
        gamecontrols.Player.Move.started -= OnMove;
        gamecontrols.Player.Jumpboth.performed -= OnJumpboth;
        gamecontrols.Dispose();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMove(InputAction.CallbackContext context)
    {
        titlesprite = false;
        Vector2 moving = context.ReadValue<Vector2>();
        if(moving.x>0)selectbutton++;
        else selectbutton--;
    }

    void OnJumpboth(InputAction.CallbackContext context){
        if(context.ReadValue<float>()==0){
            jump = false;
        }else{
            jump = true;
        }
    }

    void Scene_otogame(){
        SceneManager.LoadScene("otogame");
    }

    void Scene_option(){

    }

    // Update is called once per frame
    void Update()
    {
        if(selectbutton%2==0)panel.sprite = startselect;
        else panel.sprite = optionselect;
        if(jump){
            if(selectbutton%2==0){
                Scene_otogame();
            }
        }
    }
}

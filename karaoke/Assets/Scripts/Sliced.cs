using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sliced : MonoBehaviour
{
    Vector2 move;
    float x;
    float jump;

    Gamecontrols gamecontrols;
    // Start is called before the first frame update
    
    void Awake()
    {
        /*
        // Input Actionインスタンス生成
        _gameInputs = new Gamecontrols();

        // Actionイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        // Input Actionを機能させるためには、
        // 有効化する必要がある
        _gameInputs.Enable();

        //_gameInputs.Player.Jump.performed += OnJump;
        */
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
    /*
     private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        _gameInputs?.Dispose();
    }

       private void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }


    */
    void OnMove(InputValue context)
    {
        move = context.Get<Vector2>();
    }

    void OnSing2(InputValue context)
    {
        x = context.Get<float>();
        
    }
    public void OnJumppress(InputAction.CallbackContext context)
    {
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        jump = context.ReadValue<float>();
    }

    // 離された瞬間のコールバック
    public void OnJumprelease(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;
        jump = context.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 move3d = new Vector3 (move.x,move.y,0) * Time.deltaTime * 3f;
        transform.position += move3d;
    }
}

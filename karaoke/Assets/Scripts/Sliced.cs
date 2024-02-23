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
        // Input Action�C���X�^���X����
        _gameInputs = new Gamecontrols();

        // Action�C�x���g�o�^
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        // Input Action���@�\�����邽�߂ɂ́A
        // �L��������K�v������
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
        // ���g�ŃC���X�^���X������Action�N���X��IDisposable���������Ă���̂ŁA
        // �K��Dispose����K�v������
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
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;
        jump = context.ReadValue<float>();
    }

    // �����ꂽ�u�Ԃ̃R�[���o�b�N
    public void OnJumprelease(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
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

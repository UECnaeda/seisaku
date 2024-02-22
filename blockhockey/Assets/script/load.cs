using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class load : MonoBehaviour
{ //xxxxにはスクリプト自体のファイル名が入る



    public int ScreenWidth;

    public int ScreenHeight;



    void Awake()

    {

        // PC向けビルドだったらサイズ変更

        if (Application.platform == RuntimePlatform.WindowsPlayer ||

        Application.platform == RuntimePlatform.OSXPlayer ||

        Application.platform == RuntimePlatform.LinuxPlayer)

        {

            Screen.SetResolution(ScreenWidth, ScreenHeight, false);

        }
    }
}
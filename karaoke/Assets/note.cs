using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Burst.Intrinsics;

public class note : MonoBehaviour
{
    //��������m�[�c
    public GameObject notes;
    List<GameObject> nownotes = new List<GameObject>();
    int listsize = 0;
    //��ʂɕ\������Ă���m�[�c�ԍ�
    int notenumbers = 1;
    int song4syo = 256;
    float onteiba = 18.1f;//�m�[�c�̑傫���̊
    float gamescreenx = 17.7f;
    //�o�[�̑傫��
    float karaokebarheight = 8;
    //���P�[�V����
    float karaokebarloc = 0.97f;//�{���̒l����+0.13f���Ă��������@�Ȃ񂩂���Ă�
    float noteslocy;
    float noteslocx;
    float notesize;
    float soundvalue = 30f;
    int nextnumbers;
    int nownumbers = 1;
    bool noteend = false;
    //�m�[�c�����
    //�����I�ɂ�musicxml��ǂݍ��܂��Ď��������ł���悤�ɂ��܂�
    //��ʂ�beats*�U�S�����ōl���āA���̒������̒l��notelong��
    //�̎��O�Ɠ����Ȃ��
    //�x���͉�����-1�ɉ̎���kyu��
    //�p��͖����Ȃ̂łЂ炪�ȂŁ@���̂����̎��Ή������邩������Ȃ���
    //�I�������{"�A�E�g���̎���",-2,"fin"};
    //songdata={notelong,ontei,kashi}
    //songdata[0]={�Ȗ��ABPM�Abeats}
    string[,] songdata;
    string[,] shiningstar = new string[,] {
    {"�V���C�j���O�X�^�[","158","4"},
    { "504", "-1" ,"kyu" },
    { "8", "6","��" },
    { "24","6","��" },
    {"8","6","��"},
    {"24","6","��"},
    {"8","6","��"},
    {"8","8","��"},
    {"8","10","��"},
    {"8","11","��"},
    {"24","10","��"},
    {"8","-1","kyu"},
    {"8","6","��"},
    {"8","8","��"},
    {"8","10","��"},
    {"8","11","��"},
    {"16","10","��"},
    {"16","6","��"},
    {"16","8","��"},
    {"8","10","��"},
    {"32","6","��"},
    {"8","-1","kyu"},
    {"8","6","��"},
    {"24","6","��"},
    {"8","6","��"},
    {"24","6","��"},
    {"8","6","��"},
    {"8","6","��"},
    {"8","1","��"},
    {"8","1","��"},
    {"8","8","��"},
    {"16","10",""},
    {"8","-1","kyu"},
    {"8","10","��"},
    {"16","11","��"},
    {"8","10","��"},
    {"16","8","��"},
    {"16","6","��"},
    {"16","6","��"},
    {"16","5","��"},
    {"8","6","��"},
    {"16","8","��"},
    {"16","-1","kyu" },
    {"16","10","��"},
    {"16","11","��"},
    {"16","10","��"},
    {"8","5","��"},
    {"56","6","��"},
    {"8","-1","kyu" },
    {"8","6","��"},
    {"16","10","��"},
    {"16","13","��"},
    {"16","10","��"},
    {"8","5","��"},
    {"16","5","��"},
    {"8","5","��"},
    {"16","8","��"},
    {"16","6","��"},
    {"8","-1","kyu"},
    {"8","6","��"},
    {"12","15","��"},
    {"12","13","��"},
    {"8","11","��"},
    {"12","11","��"},
    {"12","10","��"},
    {"8","11","��"},
    {"12","17","��"},
    {"12","15","��"},
    {"8","13","��"},
    {"16","13","��"},
    {"8","-1","kyu"},
    {"8","8","��"},
    {"12","14","��"},
    {"12","13","��"},
    {"8","11","��"},
    {"12","11","��"},
    {"12","13","��"},
    {"8","15","��"},
    {"64","16","��"},
    {"48","-1","kyu"},
    {"8","10","�V��"},
    {"8","11","�C"},
    {"16","13","�j���O"},
    {"8","13","�X"},
    {"16","13","�^�["},
    {"16","13","��"},
    {"16","13","��"},
    {"8","6","��"},
    {"16","6","��"},
    {"8","-1","kyu"},
    {"8","6","��"},
    {"8","13","��"},
    {"8","11","��"},
    {"16","10","��"},
    {"8","11","��"},
    {"16","8","��"},
    {"16","17","��"},
    {"16","17","��"},
    {"8","15","��"},
    {"8","17","��"},
    {"16","18","��"},
    {"8","-1","kyu"},
    {"8","15","��"},
    {"8","17","��"},
    {"16","18","��"},
    {"8","17","��"},
    {"16","15","��"},
    {"16","17","��"},
    {"16","17","��"},
    {"8","17","��"},
    {"16","20","��"},
    {"16","18","��"},
    {"8","-1","kyu"},
    {"8","11","��"},
    {"8","16","��"},
    {"16","16","��"},
    {"16","15","��"},
    {"16","11","��"},
    {"16","13","��"},
    {"8","15","��"},
    {"16","13","��"},
    {"16","-1","kyu"},
    {"8","10","��"},
    {"8","11","����"},
    {"16","13","�т�["},
    {"8","13","��"},
    {"16","13","����"},
    {"16","13","�܂�"},
    {"16","13","����"},
    {"8","6","��"},
    {"16","6","�����"},
    {"8","-1","kyu"},
    {"8","6","��"},
    {"8","13","��"},
    {"8","11","��"},
    {"16","10","�Ȃ�"},
    {"8","11","��"},
    {"16","8","��"},
    {"16","17","��"},
    {"16","17","��"},
    {"8","15","��"},
    {"8","17","��"},
    {"16","18","��"},
    {"8","-1","kyu"},
    {"8","15","��"},
    {"8","17","��"},
    {"16","18","��"},
    {"8","17","��"},
    {"16","15","��"},
    {"16","17","��"},
    {"16","17","��"},
    {"8","17","��"},
    {"16","20","��"},
    {"16","18","��"},
    {"8","-1","kyu"},
    {"8","10","��"},
    {"8","11","��"},
    {"16","11","��"},
    {"16","13","��"},
    {"16","15","��"},
    {"16","14","��"},
    {"8","14","��"},
    {"16","11","��"},
    {"16","10","��"},
    {"8","-1","kyu"},
    {"8","8","��"},
    {"16","10","��"},
    {"16","13","��"},
    {"16","18","��"},
    {"16","15","��"},
    {"32","18","��"},
    {"16","17","��"},
    {"8","17","��"},
    {"72","18","��"},
    {"72","-1","kyu"},
    {"8","16","��"},
    {"8","16","��"},
    {"8","16","��"},
    {"8","16","��"},
    {"8","9","��"},
    {"8","8","��"},
    {"64","9","��"},
    {"16","-1","kyu"},
    {"8","16","��"},
    {"8","16","��"},
    {"8","16","��"},
    {"8","16","��"},
    {"8","9","��"},
    {"8","8","��"},
    {"40","9","��"},
    {"32","11","��"},
    {"64","13","��"},
    {"1000","-2","fin"},
    };
    // Start is called before the first frame update
    void Start()
    {
        songdata = shiningstar;
        notemake();
    }
    
    void notemake(){
        int i = 0;
        int song4syo = 256;
        while(song4syo != 0){
            int notelong = int.Parse(songdata[notenumbers,0]);
            Debug.Log(notelong);
            Debug.Log(song4syo);
        //4���߂ɓ���c��̗e�ʂ�莟��songdata�̒����������ꍇ
            if(song4syo<notelong){
                Debug.Log("song4syo<notelong");
                if (songdata[notenumbers, 1] != "-1")
                {
                    noteslocy = karaokebarloc - (karaokebarheight / 2) + (karaokebarheight * float.Parse(songdata[notenumbers, 1]) / soundvalue);
                    notesize = onteiba / 256 * (float)song4syo;
                    noteslocx = gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2 + notesize / 2;
                    nownotes.Add(Instantiate(notes, new Vector3(noteslocx, noteslocy, -3f), Quaternion.identity));
                    nownotes[i].transform.localScale = new Vector3(notesize, 0.26666666f, 1f);
                }
                if (songdata[notenumbers, 1] == "-2")
                {
                    noteend = true;
                    break;
                }
                notelong -= song4syo;
                songdata[notenumbers, 0] = notelong.ToString();
                break;
            }
            if (songdata[notenumbers, 1] == "-2")
            {
                noteend = true;
                break;
            }
            if(songdata[notenumbers,1]!="-1"){
                noteslocy = karaokebarloc-(karaokebarheight/2) +  (karaokebarheight*float.Parse(songdata[notenumbers,1])/soundvalue);
                notesize = onteiba / 256 * (float)notelong;
                noteslocx = gamescreenx/256*(float)(256-song4syo)-gamescreenx/2 + notesize/2;
                nownotes.Add(Instantiate(notes, new Vector3(noteslocx,noteslocy, -3f), Quaternion.identity));
                nownotes[i].transform.localScale = new Vector3(notesize, 0.26666666f, 1f);
                nownumbers = notenumbers;
                notenumbers++;
                i++;
                song4syo -= notelong;
            }
            else
            {
                nownumbers = notenumbers;
                notenumbers++;
                song4syo -= notelong;
            }
            Debug.Log(songdata[nownumbers,2]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //���ʂł�����S���󂵂đS�����
        //4���ߕ��������Ă����\��ł����c
        if(GameMaker.instance.destroybar){
            if (nownotes != null)
            {
                foreach (GameObject obj in nownotes)
                {
                    Destroy(obj);
                }
                nownotes.Clear();
            }
            
            if(noteend==false)notemake();
        }
    }
}

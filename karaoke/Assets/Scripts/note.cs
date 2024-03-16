using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Burst.Intrinsics;

public class note : MonoBehaviour
{
    //���y�p
    AudioSource songsound;
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
    public bool noteend = false;
    bool songnow = false;
    bool notelongover = false;
    public bool musicend = false;

    int i;

    //����Ɏg�p�A�m�[�c�̎n�܂��posx�ƁA�����肩�Ȃ�����ǉ�
    public List<List<float>> notes_timingposx_list = new List<List<float>>();
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
    void Awake()
    {
        songsound = GetComponent<AudioSource>();

    }
    void Start()
    {
        songdata = shiningstar;
        notemake();
        songsound.volume = (float)(GameMaker.instance.volume_music)/10;
        Debug.Log($"music = {songsound.volume}");

    }
    
    void notemake(){
        notes_timingposx_list.Clear();
        int i = 0;
        int song4syo = 256;
        while(song4syo != 0){
            int notelong = int.Parse(songdata[notenumbers,0]);
            if(noteend){
                if(notelong<song4syo){
                    Debug.Log("music end!!!");
                    musicend = true;
                    break;
                }
            }
            Debug.Log($"notelong = {notelong}, song4syo = {song4syo}");
        //4���߂ɓ���c��̗e�ʂ�莟��songdata�̒����������ꍇ
            if(song4syo<notelong){
                Debug.Log("song4syo<notelong");
                if (songdata[notenumbers, 1] == "-2")
                {
                    //�^�C�~���O�v�Z�p
                    List<float> data = new List<float>();
                    data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                    //-2�͋x��
                    data.Add(-2);
                    notes_timingposx_list.Add(data);
                    noteend = true;
                }else if (songdata[notenumbers, 1] == "-1"){
                    //�^�C�~���O�v�Z�p
                    List<float> data = new List<float>();
                    data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                    //-1�͋x��
                    data.Add(1);
                    notes_timingposx_list.Add(data);
                }else{
                    //note�𐶐��@�����A�����A�傫�����ȃf�[�^�ɍ��킹��
                    noteslocy = karaokebarloc - (karaokebarheight / 2) + (karaokebarheight * float.Parse(songdata[notenumbers, 1]) / soundvalue);
                    notesize = onteiba / 256 * (float)song4syo;
                    noteslocx = gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2 + notesize / 2;
                    //�X�R�A�v�Z�p
                    //�O�̃m�[�c�ƍ���������m�[�c�����ꉹ�ł���ꍇ�Ƃ����łȂ��ꍇ�ŕ�����
                    if(notelongover){
                        notelongover = false;
                    }else{
                        List<float> data = new List<float>();
                        data.Add(noteslocx - notesize / 2);
                        //������add
                        data.Add(float.Parse(songdata[notenumbers,1]));
                        notes_timingposx_list.Add(data);
                    }
                    
                    //nownotes�ɒǉ�
                    nownotes.Add(Instantiate(notes, new Vector3(noteslocx, noteslocy, -3f), Quaternion.identity));
                    nownotes[i].transform.localScale = new Vector3(notesize, 0.26666666f, 1f);
                }
                //���܂���notelong������songdata�ɖ߂�
                //���̏ꍇ�^�C�~���O�v�Z�p�ɔ����ǉ�����Ƃ܂����̂ł���p�̕ϐ���p�ӂ���
                notelongover = true;
                notelong -= song4syo;
                songdata[notenumbers, 0] = notelong.ToString();
                break;
            }
            if (songdata[notenumbers, 1] == "-2")
            {
                //�^�C�~���O�v�Z�p
                List<float> data = new List<float>();
                data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                //-2�͋x��
                data.Add(-2);
                notes_timingposx_list.Add(data);
                noteend = true;
                break;
            }
            if(songdata[notenumbers,1]!="-1"){
                noteslocy = karaokebarloc-(karaokebarheight/2) +  (karaokebarheight*float.Parse(songdata[notenumbers,1])/soundvalue);
                notesize = onteiba / 256 * (float)notelong;
                noteslocx = gamescreenx/256*(float)(256-song4syo)-gamescreenx/2 + notesize/2;
                //�^�C�~���O�v�Z�p
                if(notelongover){
                    notelongover = false;
                }else{
                    List<float> data = new List<float>();
                    data.Add(noteslocx - notesize / 2);
                    //������add
                    data.Add(float.Parse(songdata[notenumbers,1]));
                    notes_timingposx_list.Add(data);
                }
                //nownotes�Ƀm�[�c����������
                nownotes.Add(Instantiate(notes, new Vector3(noteslocx,noteslocy, -3f), Quaternion.identity));
                nownotes[i].transform.localScale = new Vector3(notesize, 0.26666666f, 1f);
                nownumbers = notenumbers;
                notenumbers++;
                i++;
                song4syo -= notelong;
            }
            else
            {
                //�^�C�~���O�v�Z�p
                if(notelongover){
                    notelongover = false;
                }else{
                    List<float> data = new List<float>();
                    data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                    //-1��add
                    data.Add(-1);
                    notes_timingposx_list.Add(data);
                }
                nownumbers = notenumbers;
                notenumbers++;
                song4syo -= notelong;
            }
            Debug.Log($"songdata[{nownumbers},2] = {songdata[nownumbers,2]}");
        }
        for(i=0;i<notes_timingposx_list.Count;++i){
            Debug.Log($"note:note_timingpos_list[{i}][0] = {notes_timingposx_list[i][0]}");
            Debug.Log($"note:note_timingpos_list[{i}][1] = {notes_timingposx_list[i][1]}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMaker.instance.musicstart&&!songnow){
            Debug.Log("note:start music");
            songsound.Play();
            songnow = true;
        }
        songsound.pitch = GameMaker.instance.gamespeed;
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
            
            notemake();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Burst.Intrinsics;

public class note : MonoBehaviour
{
    //���y�p
    AudioSource songsound;
    public AudioClip shiningster_AC, tutorial_AC, kirakiraboshi_AC;
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
    //realkaraoke���[�h�ȊO�Ŏg�����
    public bool notemake_over = false;
    public float noteend_loc;

    int i;

    //����Ɏg�p�A�m�[�c�̎n�܂��posx�ƁA�����肩�Ȃ�����ǉ�
    public List<List<float>> notes_timingposx_list = new List<List<float>>();

    //�`���[�g���A���ϐ�
    int tutorial_phase = 0;
    bool tutorialmode = false;

    //pause�p
    bool pausecheck = false;

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

    //�����o���`���[�g���A���p
    string[,] tutorial1 = new string[,]{
        {"�`���[�g���A��1","120","4"},
        {"384","-1","kyu"},
        {"48","14","��"},
        {"16","-1","kyu"},
        {"16","14","��"},
        {"16","-1","kyu"},
        {"16","14","��"},
        {"1000","-2","fin"},
    };
    //�����𑀍삷��`���[�g���A���p
    string[,] tutorial2 = new string[,]{
        {"�`���[�g���A��2","120","4"},
        {"384","-1","kyu"},
        {"32","5","�h"},
        {"32","7","��"},
        {"32","9","�~"},
        {"32","10","�t�@"},
        {"32","9","�~"},
        {"32","7","��"},
        {"32","5","�h"},
        {"1000","-2","fin"},
    };

    //���炫��ڂ��@���쌠�t���[�ł�
    string[,] kirakiraboshi = new string[,]{
        {"���炫�琯","120","4"},
        {"384","-1","kyu"},
        {"16","5","�h"},
        {"16","5","�h"},
        {"16","12","�\"},
        {"16","12","�\"},
        {"16","14","��"},
        {"16","14","��"},
        {"16","12","�\"},
        {"16","-1","kyu"},
        {"16","10","�t�@"},
        {"16","10","�t�@"},
        {"16","9","�~"},
        {"16","9","�~"},
        {"16","7","��"},
        {"16","7","��"},
        {"16","5","�h"},
        {"1000","-2","fin"},
    };

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
        Select_song();
        songsound.volume = (float)(GameMaker.instance.volume_music)/10;
        Debug.Log($"music = {songsound.volume}");

    }

    void Initial_Values(){
        //nownotes��������
        Reset_nownotes();
        notenumbers = 1;
        noteend = false;
        songnow = false;
    }

    //nownotes����ɂ���
    void Reset_nownotes(){
        if (nownotes != null){
            foreach (GameObject obj in nownotes){
                Destroy(obj);
            }
            nownotes.Clear();
        }
    }

    void Select_song(){
        //������
        Initial_Values();
        songsound.Stop();
        songnow = false;
        //�`���[�g���A�����[�h�̏ꍇ
        if(GameMaker.instance.tutorialmode){
            tutorialmode = true;
            tutorial_phase = GameMaker.instance.tutorial_phase;
            if(tutorial_phase==0){
                songdata = tutorial1;
                songsound.clip = tutorial_AC;
                songsound.loop = true;
            }else if(tutorial_phase==1){
                songdata = tutorial2;
                songsound.clip = tutorial_AC;
                songsound.loop = true;
            }else if(tutorial_phase==2){
                songdata = kirakiraboshi;
                songsound.clip = kirakiraboshi_AC;
                songsound.loop = true;
            }
        }else{
            songdata = shiningstar;
            songsound.clip = shiningster_AC;
            songsound.loop = false;
        }
        //�ŏ��̃m�[�c����
        if(!GameMaker.realkaraoke){
            notemake_notemove();
            notemake_over = true;
        }else{
            notemake();
            notemake_over = true;
        }
    }
    
    //realkaraoke���[�h�������łȂ����Ŏd�l���傫���ς��
    //�O�҂Ȃ�4���߂��ƂɌ��܂����ʒu��4���ߐ���
    //��҂Ȃ��C�ɑS������
    void notemake_notemove(){
        notes_timingposx_list.Clear();
        int song_long = 0;

        Debug.Log($"songdata.GetLength(0) = {songdata.GetLength(0)}");
        for(int i=1;i<songdata.GetLength(0);++i){
            int notelong = int.Parse(songdata[i,0]);
            if(songdata[i, 1] == "-2"){
                //�^�C�~���O�v�Z�p
                List<float> data = new List<float>();
                noteend_loc = gamescreenx / 256 * (float)(song_long) - gamescreenx / 2;
                data.Add(noteend_loc);
                //-2�͋x��
                data.Add(-2);
                notes_timingposx_list.Add(data);
            }else if(songdata[i,1]!="-1"){
                noteslocy = karaokebarloc-(karaokebarheight/2) +  (karaokebarheight*float.Parse(songdata[i,1])/soundvalue);
                notesize = onteiba / 256 * (float)notelong;
                noteslocx = gamescreenx/256*(float)(song_long)-gamescreenx/2 + notesize/2;
                //�^�C�~���O�v�Z�p
                List<float> data = new List<float>();
                data.Add(noteslocx - notesize / 2);
                //������add
                data.Add(float.Parse(songdata[i,1]));
                notes_timingposx_list.Add(data);
                //nownotes�Ƀm�[�c����������
                nownotes.Add(Instantiate(notes, new Vector3(noteslocx,noteslocy, -3f), Quaternion.identity));
                nownotes[nownotes.Count-1].transform.localScale = new Vector3(notesize, 0.26666666f, 1f);
            }else{
                //�^�C�~���O�v�Z�p
                List<float> data = new List<float>();
                data.Add(gamescreenx / 256 * (float)(song_long) - gamescreenx / 2);
                //-1��add
                data.Add(-1);
                notes_timingposx_list.Add(data);
            }
            song_long += notelong;
            Debug.Log($"songdata[{i},2] = {songdata[i,2]}");
        }
        //�f�o�b�O�p
        for(int i=0;i<notes_timingposx_list.Count;++i){
            Debug.Log($"note:note_timingpos_list[{i}][0] = {notes_timingposx_list[i][0]}");
            Debug.Log($"note:note_timingpos_list[{i}][1] = {notes_timingposx_list[i][1]}");
        }
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
                    data.Add(-1);
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
            if (songdata[notenumbers, 1] == "-2"){
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
            }else{
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
    }

    // Update is called once per frame
    void Update()
    {
        //pause�@�\
        if(pausecheck==false&&GameMaker.instance.ispause==true){
            songsound.Pause();
            pausecheck = true;
        }else if(pausecheck==true&&GameMaker.instance.ispause==false){
            songsound.UnPause();
            pausecheck = false;
        }
        //�`���[�g���A�����[�h�̋L�q
        if(tutorialmode){
            if(GameMaker.tutorialstart){
                Select_song();
                Debug.Log($"tutorial_phase = {GameMaker.instance.tutorial_phase}");
                GameMaker.tutorialstart = false;
            }
        }

        //�`���[�g���A�����[�h�I�����̏���
        if(tutorialmode&&!GameMaker.instance.tutorialmode){
            tutorialmode = false;
            Select_song();
        }
        if(GameMaker.instance.musicstart&&!songnow){
            Debug.Log("note:start music");
            songsound.Play();
            songnow = true;
        }
        //songsound.pitch = GameMaker.instance.gamespeed;
        //norealkaraoke���[�h�̎��̏C������
        if(GameMaker.realkaraoke==false){
            if(noteend_loc<GameMaker.instance.notemoving){
                noteend = true;
            }
        }
        //���ʂł�����S���󂵂đS�����
        //4���ߕ��������Ă����\��ł����c
        if(GameMaker.instance.destroybar){
            Reset_nownotes();
            notemake();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Burst.Intrinsics;

public class note : MonoBehaviour
{
    //音楽用
    AudioSource songsound;
    //生成するノーツ
    public GameObject notes;
    List<GameObject> nownotes = new List<GameObject>();
    int listsize = 0;
    //画面に表示されているノーツ番号
    int notenumbers = 1;
    int song4syo = 256;
    float onteiba = 18.1f;//ノーツの大きさの基準
    float gamescreenx = 17.7f;
    //バーの大きさ
    float karaokebarheight = 8;
    //ロケーション
    float karaokebarloc = 0.97f;//本当の値から+0.13fしてください　なんかずれてる
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

    //判定に使用、ノーツの始まりのposxと、音ありかなしかを追加
    public List<List<float>> notes_timingposx_list = new List<List<float>>();
    //ノーツを作る
    //将来的にはmusicxmlを読み込ませて自動生成できるようにします
    //画面をbeats*６４分割で考えて、その長さ文の値をnotelongに
    //歌詞前と同じなら空白
    //休符は音程を-1に歌詞をkyuに
    //英語は無理なのでひらがなで　そのうち歌詞対応させるかもしれないね
    //終わったら{"アウトロの時間",-2,"fin"};
    //songdata={notelong,ontei,kashi}
    //songdata[0]={曲名、BPM、beats}
    string[,] songdata;
    string[,] shiningstar = new string[,] {
    {"シャイニングスター","158","4"},
    { "504", "-1" ,"kyu" },
    { "8", "6","た" },
    { "24","6","だ" },
    {"8","6","か"},
    {"24","6","ぜ"},
    {"8","6","に"},
    {"8","8","ゆ"},
    {"8","10","ら"},
    {"8","11","れ"},
    {"24","10","て"},
    {"8","-1","kyu"},
    {"8","6","な"},
    {"8","8","に"},
    {"8","10","も"},
    {"8","11","か"},
    {"16","10","ん"},
    {"16","6","が"},
    {"16","8","え"},
    {"8","10","ず"},
    {"32","6","に"},
    {"8","-1","kyu"},
    {"8","6","た"},
    {"24","6","だ"},
    {"8","6","く"},
    {"24","6","も"},
    {"8","6","を"},
    {"8","6","な"},
    {"8","1","が"},
    {"8","1","め"},
    {"8","8","て"},
    {"16","10",""},
    {"8","-1","kyu"},
    {"8","10","す"},
    {"16","11","ご"},
    {"8","10","す"},
    {"16","8","の"},
    {"16","6","も"},
    {"16","6","い"},
    {"16","5","い"},
    {"8","6","よ"},
    {"16","8","ね"},
    {"16","-1","kyu" },
    {"16","10","さ"},
    {"16","11","ざ"},
    {"16","10","な"},
    {"8","5","み"},
    {"56","6","の"},
    {"8","-1","kyu" },
    {"8","6","お"},
    {"16","10","と"},
    {"16","13","に"},
    {"16","10","い"},
    {"8","5","や"},
    {"16","5","さ"},
    {"8","5","れ"},
    {"16","8","て"},
    {"16","6","く"},
    {"8","-1","kyu"},
    {"8","6","き"},
    {"12","15","せ"},
    {"12","13","き"},
    {"8","11","を"},
    {"12","11","は"},
    {"12","10","こ"},
    {"8","11","ぶ"},
    {"12","17","か"},
    {"12","15","ぜ"},
    {"8","13","の"},
    {"16","13","ね"},
    {"8","-1","kyu"},
    {"8","8","と"},
    {"12","14","き"},
    {"12","13","を"},
    {"8","11","と"},
    {"12","11","じ"},
    {"12","13","こ"},
    {"8","15","め"},
    {"64","16","て"},
    {"48","-1","kyu"},
    {"8","10","シャ"},
    {"8","11","イ"},
    {"16","13","ニング"},
    {"8","13","ス"},
    {"16","13","ター"},
    {"16","13","つ"},
    {"16","13","づ"},
    {"8","6","れ"},
    {"16","6","ば"},
    {"8","-1","kyu"},
    {"8","6","む"},
    {"8","13","ね"},
    {"8","11","に"},
    {"16","10","ね"},
    {"8","11","む"},
    {"16","8","る"},
    {"16","17","ま"},
    {"16","17","ぼ"},
    {"8","15","ろ"},
    {"8","17","し"},
    {"16","18","が"},
    {"8","-1","kyu"},
    {"8","15","て"},
    {"8","17","の"},
    {"16","18","ひ"},
    {"8","17","ら"},
    {"16","15","に"},
    {"16","17","ふ"},
    {"16","17","り"},
    {"8","17","そ"},
    {"16","20","そ"},
    {"16","18","ぐ"},
    {"8","-1","kyu"},
    {"8","11","あ"},
    {"8","16","ら"},
    {"16","16","た"},
    {"16","15","な"},
    {"16","11","せ"},
    {"16","13","か"},
    {"8","15","い"},
    {"16","13","へ"},
    {"16","-1","kyu"},
    {"8","10","あ"},
    {"8","11","いる"},
    {"16","13","びりー"},
    {"8","13","ぶ"},
    {"16","13","おぶ"},
    {"16","13","まい"},
    {"16","13","せん"},
    {"8","6","せ"},
    {"16","6","しょん"},
    {"8","-1","kyu"},
    {"8","6","は"},
    {"8","13","て"},
    {"8","11","し"},
    {"16","10","ない"},
    {"8","11","み"},
    {"16","8","ち"},
    {"16","17","の"},
    {"16","17","む"},
    {"8","15","こ"},
    {"8","17","う"},
    {"16","18","で"},
    {"8","-1","kyu"},
    {"8","15","ま"},
    {"8","17","ぶ"},
    {"16","18","た"},
    {"8","17","の"},
    {"16","15","う"},
    {"16","17","ら"},
    {"16","17","に"},
    {"8","17","う"},
    {"16","20","つ"},
    {"16","18","る"},
    {"8","-1","kyu"},
    {"8","10","ひ"},
    {"8","11","と"},
    {"16","11","し"},
    {"16","13","ず"},
    {"16","15","く"},
    {"16","14","の"},
    {"8","14","ひ"},
    {"16","11","か"},
    {"16","10","り"},
    {"8","-1","kyu"},
    {"8","8","と"},
    {"16","10","き"},
    {"16","13","め"},
    {"16","18","き"},
    {"16","15","を"},
    {"32","18","か"},
    {"16","17","ん"},
    {"8","17","じ"},
    {"72","18","て"},
    {"72","-1","kyu"},
    {"8","16","ら"},
    {"8","16","ら"},
    {"8","16","ら"},
    {"8","16","ら"},
    {"8","9","ら"},
    {"8","8","ら"},
    {"64","9","ら"},
    {"16","-1","kyu"},
    {"8","16","ら"},
    {"8","16","ら"},
    {"8","16","ら"},
    {"8","16","ら"},
    {"8","9","ら"},
    {"8","8","ら"},
    {"40","9","ら"},
    {"32","11","ら"},
    {"64","13","ら"},
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
        //4小節に入る残りの容量より次のsongdataの長さが長い場合
            if(song4syo<notelong){
                Debug.Log("song4syo<notelong");
                if (songdata[notenumbers, 1] == "-2")
                {
                    //タイミング計算用
                    List<float> data = new List<float>();
                    data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                    //-2は休符
                    data.Add(-2);
                    notes_timingposx_list.Add(data);
                    noteend = true;
                }else if (songdata[notenumbers, 1] == "-1"){
                    //タイミング計算用
                    List<float> data = new List<float>();
                    data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                    //-1は休符
                    data.Add(1);
                    notes_timingposx_list.Add(data);
                }else{
                    //noteを生成　高さ、長さ、大きさを曲データに合わせる
                    noteslocy = karaokebarloc - (karaokebarheight / 2) + (karaokebarheight * float.Parse(songdata[notenumbers, 1]) / soundvalue);
                    notesize = onteiba / 256 * (float)song4syo;
                    noteslocx = gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2 + notesize / 2;
                    //スコア計算用
                    //前のノーツと今生成するノーツが同一音である場合とそうでない場合で分ける
                    if(notelongover){
                        notelongover = false;
                    }else{
                        List<float> data = new List<float>();
                        data.Add(noteslocx - notesize / 2);
                        //音程をadd
                        data.Add(float.Parse(songdata[notenumbers,1]));
                        notes_timingposx_list.Add(data);
                    }
                    
                    //nownotesに追加
                    nownotes.Add(Instantiate(notes, new Vector3(noteslocx, noteslocy, -3f), Quaternion.identity));
                    nownotes[i].transform.localScale = new Vector3(notesize, 0.26666666f, 1f);
                }
                //あまったnotelongを今のsongdataに戻す
                //この場合タイミング計算用に判定を追加するとまずいのでそれ用の変数を用意する
                notelongover = true;
                notelong -= song4syo;
                songdata[notenumbers, 0] = notelong.ToString();
                break;
            }
            if (songdata[notenumbers, 1] == "-2")
            {
                //タイミング計算用
                List<float> data = new List<float>();
                data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                //-2は休符
                data.Add(-2);
                notes_timingposx_list.Add(data);
                noteend = true;
                break;
            }
            if(songdata[notenumbers,1]!="-1"){
                noteslocy = karaokebarloc-(karaokebarheight/2) +  (karaokebarheight*float.Parse(songdata[notenumbers,1])/soundvalue);
                notesize = onteiba / 256 * (float)notelong;
                noteslocx = gamescreenx/256*(float)(256-song4syo)-gamescreenx/2 + notesize/2;
                //タイミング計算用
                if(notelongover){
                    notelongover = false;
                }else{
                    List<float> data = new List<float>();
                    data.Add(noteslocx - notesize / 2);
                    //音程をadd
                    data.Add(float.Parse(songdata[notenumbers,1]));
                    notes_timingposx_list.Add(data);
                }
                //nownotesにノーツ情報を加える
                nownotes.Add(Instantiate(notes, new Vector3(noteslocx,noteslocy, -3f), Quaternion.identity));
                nownotes[i].transform.localScale = new Vector3(notesize, 0.26666666f, 1f);
                nownumbers = notenumbers;
                notenumbers++;
                i++;
                song4syo -= notelong;
            }
            else
            {
                //タイミング計算用
                if(notelongover){
                    notelongover = false;
                }else{
                    List<float> data = new List<float>();
                    data.Add(gamescreenx / 256 * (float)(256 - song4syo) - gamescreenx / 2);
                    //-1をadd
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
        //譜面できたら全部壊して全部作る
        //4小節分生成していく予定ですが…
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

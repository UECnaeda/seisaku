using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameMaker : MonoBehaviour
{
    //�V���O���g���ɂ��܂��@�킯�킩��񂩂�����https://umistudioblog.com/singletonhowto/
    public static GameMaker instance;
    /*
        ��ʃT�C�Y�cx��-8.85�`8.85
    */
    float gamescreenx = 17.7f;
    public float bpm = 158;
    public float beats = 4;
    float time4sec;
    
    //�J���I�P�o�[
    public GameObject karaokeloc;
    public GameObject karaokebar;
    //�o�[�̑傫��
    float karaokebarheight = 8;
    //���P�[�V����
    float karaokebarloc = 0.97f;//�{���̒l����+0.13f���Ă��������@�Ȃ񂩂���Ă�
    float karaokelocy;
    //�o�[
    Vector3 karaokebarpos;
    public Slider soundslider;
    public Sprite speak, mute;
    public Image images;
    //���ݒ�
    public AudioClip a3e, a3si,a4so, a4u, a5a, a5se, b3u, b4a, b5i, c4o, c4sa, c5e, c5si,c6u, d4e, d4si, d5so, d5u, e4u, e5a, f4o, f4sa, f5e, f5si, g3o, g3sa, g4e, g4si, g5so, g5u;
    AudioSource audioSource;
    //���l�֘A
    float sing1,sing2;
    float jump;
    float soundvalue = 30;
    int selectsound;
    int beforesound = 30;
    float soundtime = 0;
    float onteiba = 18.1f; //�����̓z�̍ő剡��
    float onteitani; //�o�[�̈ړ��Ɖ����̓z�̑傫�����C���^���N�g�����邽�߂̒P��
    Gamecontrols gamecontrols;
    //��������z
    public GameObject greatbar;
    GameObject nowbar;
    //���������ꏊ
    float geneposx;
    //�����̂�����ׂĂ��킷
    public bool destroybar = false;
    // Start is called before the first frame update
    void Awake()
    {
        //�V���O���g���̎����@�����ȊO��instance����������f�X�g���C����
        if (instance == null)
        {
            // ���g���C���X�^���X�Ƃ���
            instance = this;
        }
        else
        {
            //
            //�C���X�^���X���������݂��Ȃ��悤�ɁA���ɑ��݂��Ă����玩�g����������
            Destroy(gameObject);
        }
        //FPS�̏����ݒ�@�ς������Ȃ�instance�g��
        SetFps(60);
        //karaokeloc = GameObject.Find("karaokeloc").GetComponent<GameObject>();
        soundslider = GameObject.Find("Slider").GetComponent<Slider>();
        audioSource = GetComponent<AudioSource>();
        //public�Ȃ̂łƂ肠���������Őݒ肷��@�֐��O����unity�̒l���L�������
        bpm = 158;
        beats = 4;
        time4sec = 240*beats/bpm; //4���ߏI���܂ł̎���
        onteitani = onteiba/gamescreenx; //����Ƀ^�C���f���^�����炿�傤�ǂ悭�Ȃ�͂�     
    }
    public void SetFps(int fps){
        Application.targetFrameRate = fps;
        Time.fixedDeltaTime = 1.0f/(float)fps;
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
    private void OnDestroy()
    {
        // ���g�ŃC���X�^���X������Action�N���X��IDisposable���������Ă���̂ŁA
        // �K��Dispose����K�v������
        gamecontrols?.Dispose();
    }
    public void OnJumppress(InputAction.CallbackContext context)
    {   
        if (!context.performed) return;
        Debug.Log("press");
        Sing();
        jump = context.ReadValue<float>();
        images.sprite = speak;
    }

    // �����ꂽ�u�Ԃ̃R�[���o�b�N
    public void OnJumprelease(InputAction.CallbackContext context)
    {

        if (!context.performed) return;
        Debug.Log("release");
        audioSource.Stop();
        jump = context.ReadValue<float>();
        images.sprite = mute;
        nowbar = null;
    }

    void OnSing1(InputValue context)
    {
        sing1 = context.Get<float>();
    }
    void OnSing2(InputValue context)
    {
        sing2 = context.Get<float>();
    }
    void OnJump(InputValue context)
    {
        jump = context.Get<float>();
    }
    void Sing()
    {   
        audioSource.Stop();//���X�Ȃ��Ă鉹���~�߂�;
        Debug.Log("Singcall");
        switch(selectsound){
            case 0: 
                audioSource.PlayOneShot(g3o);
                break;
            case 1: 
                audioSource.PlayOneShot(g3sa);
                break;
            case 2: 
                audioSource.PlayOneShot(a3e);
                break;
            case 3: 
                audioSource.PlayOneShot(a3si);
                break;
            case 4: 
                audioSource.PlayOneShot(b3u);
                break;
            case 5: 
                audioSource.PlayOneShot(c4o);
                break;
            case 6: 
                audioSource.PlayOneShot(c4sa);
                break;
            case 7: 
                audioSource.PlayOneShot(d4e);
                break;
            case 8: 
                audioSource.PlayOneShot(d4si);
                break;
            case 9: 
                audioSource.PlayOneShot(e4u);
                break;
            case 10: 
                audioSource.PlayOneShot(f4o);
                break;
            case 11: 
                audioSource.PlayOneShot(f4sa);
                break;
            case 12: 
                audioSource.PlayOneShot(g4e);
                break;
            case 13: 
                audioSource.PlayOneShot(g4si);
                break;
            case 14: 
                audioSource.PlayOneShot(a4u);
                break;
            case 15: 
                audioSource.PlayOneShot(a4so);
                break;
            case 16: 
                audioSource.PlayOneShot(b4a);
                break;
            case 17: 
                audioSource.PlayOneShot(c5e);
                break;
            case 18: 
                audioSource.PlayOneShot(c5si);
                break;
            case 19: 
                audioSource.PlayOneShot(d5u);
                break;
            case 20: 
                audioSource.PlayOneShot(d5so);
                break;
            case 21: 
                audioSource.PlayOneShot(e5a);
                break;
            case 22: 
                audioSource.PlayOneShot(f5e);
                break;
            case 23: 
                audioSource.PlayOneShot(f5si);
                break;
            case 24: 
                audioSource.PlayOneShot(g5u);
                break;
            case 25: 
                audioSource.PlayOneShot(g5so);
                break;
            case 26: 
                audioSource.PlayOneShot(a5a);
                break;
            case 27: 
                audioSource.PlayOneShot(a5se);
                break;
            case 28: 
                audioSource.PlayOneShot(b5i);
                break;
            case 29: 
                audioSource.PlayOneShot(c6u);
                break;
            default:
                Debug.Log("Default??");
                break;
        }
        Debug.Log(selectsound);
        karaokelocy = karaokebarloc-(karaokebarheight/2) +  (karaokebarheight*(float)selectsound/soundvalue);
        nowbar = Instantiate(greatbar, new Vector3(karaokebarpos.x, karaokelocy, -5), Quaternion.identity);
        nowbar.GetComponent<SpriteRenderer>().color = Color.red;
        geneposx = karaokebarpos.x;
     }
    // Update is called once per frame
    void Update()
    {
        //nowbar�̑���
        if(nowbar!=null){
            nowbar.transform.localPosition += new Vector3(onteiba/time4sec*Time.deltaTime/2f,0f,0f);
            nowbar.transform.localScale += new Vector3(onteiba/time4sec*Time.deltaTime,0f,0f);
        }
        soundslider.value = selectsound; //�o�[�𓮂���
        //soundtime += Time.deltaTime;
        //selectsound = (int)((soundvalue-1)*(sing1+sing2+2)/4);//�X�e�B�b�N�̌X������0�`soundvalue-1�̒l���o��
        selectsound = (int)((soundvalue-1)*(sing1+1)/2);�@///sing1�����ł��Ȃ炱����
        //karaokebar�𓮂����@�������
        karaokebarpos = karaokebar.transform.position;
        if(karaokebarpos.x >= gamescreenx/2){
            destroybar = true;
            karaokebar.transform.position += new Vector3((gamescreenx/time4sec*Time.deltaTime)-gamescreenx,0f,0f);
        }else {
            destroybar = false;
            karaokebar.transform.position += new Vector3(gamescreenx/time4sec*Time.deltaTime,0f,0f);
        }
        //karaokeloc�𓮂����@�Ԃ��z
        karaokebarpos = karaokebar.transform.position;
        karaokeloc.transform.position = new Vector3(karaokebarpos.x,karaokelocy,karaokebarpos.z-3f);
        if(jump==1&&(beforesound!=selectsound||soundtime>=5f)){
            Sing();//�����ς�����̂ŌĂ�
            soundtime = 0;
            beforesound = selectsound;
        }
    }
}

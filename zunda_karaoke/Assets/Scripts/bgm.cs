using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm : MonoBehaviour
{
    private AudioSource title_bgm;
    // Start is called before the first frame update
    void Start()
    {
        title_bgm = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        title_bgm.volume = ((float)titleBehaviour.volume_music)/12.5f;
        Debug.Log(titleBehaviour.volume_music);
    }
}

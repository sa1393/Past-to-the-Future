using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] audio; 

    private AudioSource BGM;

    private static BGMManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static BGMManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    void Start()
    {
        BGM = gameObject.GetComponent<AudioSource>();
        BGM.loop = true;
    }

    void MainBGM() {
        BGM.clip = audio[0];
    }

    void InGameBGM() {
        BGM.clip = audio[1];
    }


    void ClearBGM() {
        BGM.clip = audio[2];
    }

    void GameOverBGM() {
        BGM.clip = audio[3];
    }
}

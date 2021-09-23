using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        //player = GameObject.Find("player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        SceneManager.LoadScene("test");
        BGMManager.Instance.InGameBGN();
    }

    public void MainSceneMove()
    {

        SceneManager.LoadScene("main");
        BGMManager.Instance.MainBGM();
    }
}

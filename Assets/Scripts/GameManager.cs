using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player player;

    //积己等 葛电 利 持绢具 凳
    ArrayList enemies = new ArrayList();

    private GameManager() { }
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = new GameManager();
            return _instance;
        }
    }


    void Awake()
    {

    }

    void Start()
    {
               
    }

    void Update()
    {
        
    }
}

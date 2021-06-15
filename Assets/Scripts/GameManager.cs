using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;

    public bool timeStop = true;
    public bool timeSlow = false;

    //积己等 葛电 利 持绢具 凳
    public ArrayList enemies = new ArrayList();

    private static GameManager instance = null;

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

    public static GameManager Instance
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

    public void TimeSlow()
    {
        timeSlow = !timeSlow;
        foreach(Enemy enemy in enemies)
        {
            enemy.TimeSlow();
        }
    }

    public void TimeStop()
    {
        timeStop = !timeStop;
        foreach (Enemy enemy in enemies)
        {
            enemy.TimeStop();
        }
    }

    public void TimeFast()
    {

    }

    void Start()
    {
               
    }

    void Update()
    {
        
    }
}

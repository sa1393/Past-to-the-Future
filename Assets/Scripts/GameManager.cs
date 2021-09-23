using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int gameFloor = 1;

    public Player player;

    public bool timeStop = true;
    public bool timeSlow = false;

    //������ ��� �� �־�� ��
    public ArrayList enemies;

    private static GameManager instance = null;

    private bool gameover = false;

    void Awake()
    {
        enemies = new ArrayList();
        Screen.SetResolution(Screen.width, Screen.width * 1920 / 1080, true);

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

    private void Update()
    {
        if (gameover) return;

        if(player.isDead)
        {
            SceneManager.LoadScene("gameover");
            BGMManager.Instance.GameOverBGM();
            gameover = true;
        }
    }
}

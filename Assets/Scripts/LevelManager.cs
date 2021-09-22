using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject tempPlayer;

    //1층
    public GameObject[] mapPrefebs1;
    //시작방
    public GameObject startMap;
    //상점
    public GameObject shopMap;

    //맵
    public GameObject[,] map;
    //
    public int maxMapNum;

    public GameObject[] mapPrefebs2;

    private static LevelManager instance = null;

    public GameObject dartBat;

    public int enemyCount = 0;

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

    public static LevelManager Instance
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

    void Start() {
       
       maxMapNum = 11;
        //1 2 3 4 5 6
        CreateLevel();
        for(int i=0; i<11; i++){
            for(int j=0; j<11; j++)
            {
                if(map[i,j] == null) continue;
                GameObject temp = Instantiate(map[i, j], new Vector2((i+1) * 6000 - 36000, (j+1) * 6000 - 36000), Quaternion.identity);
                temp.GetComponent<Map>().mapLocX = i;
                temp.GetComponent<Map>().mapLocY = j;
                int tempRan = Random.Range(0, 100);
                if(tempRan < 90)
                {
                    if(tempRan < 10)
                    {
                        Instantiate(dartBat, new Vector2((i + 1) * 6000 - 35900, (j + 1) * 6000 - 36100), Quaternion.identity);
                        Instantiate(dartBat, new Vector2((i + 1) * 6000 - 36100, (j + 1) * 6000 - 36100), Quaternion.identity);
                        enemyCount++;
                        enemyCount++;
                    }
                    else
                    {
                        Instantiate(dartBat, new Vector2((i + 1) * 6000 - 36000, (j + 1) * 6000 - 36100), Quaternion.identity);
                        enemyCount++;
                    }

                }
            } 
            
        }

    }



    void CreateLevel() {
        map = new GameObject[maxMapNum, maxMapNum];
        map[maxMapNum / 2, maxMapNum / 2] = startMap;
        Map tempMap = map[maxMapNum / 2, maxMapNum / 2].GetComponent<Map>();
        tempMap.mapLocX = maxMapNum / 2;
        tempMap.mapLocY = maxMapNum / 2;
        
        CreateMap(maxMapNum / 2, maxMapNum / 2, 80);
    }

    void CreateMap(int locX, int locY, int createPer){
        if(createPer < 5) return;
        for(int x = -1; x < 2; x++){
            for(int y = -1; y < 2; y++) {
                if((x != 0 && y != 0) || (x == 0 && y == 0)) continue;

                int tempX = locX + x;
                int tempY = locY + y;

                if(map[tempX, tempY] != null) continue;

                if(x == 0 || y == 0) {
                    int tempPer = Random.Range(1, 100);
                    if(tempPer < createPer) {
                        int randomMap = Random.Range(0, mapPrefebs1.Length);
                        map[tempX , tempY] = mapPrefebs1[randomMap];
                        Map tempMap = map[tempX , tempY].GetComponent<Map>();

                        // Debug.Log("locX: " + locX + "locY: " + locY);
                        // Debug.Log("x:" + x + ", y: " + y);
                        // Debug.Log("tempX: " + tempX + "tempY: " + tempY);
                        
                        CreateMap(tempX, tempY, createPer / 2);
                    }
                }

                
            }
        }
    }




        //옛날꺼
    // //?? ???
    // float mapX = 0;
    // float mapY = 0;

    // float minRoomWidth = 0;
    // float minRoomHeight = 0;

    // [SerializeField]
    // private GameObject tilePrefeb;

    // private BoxCollider2D test;
    // private PolygonCollider2D test2;

    // void Start()
    // {
    //     test = tilePrefeb.GetComponent<BoxCollider2D>();

    //     minRoomWidth = 150f;
    //     minRoomHeight = 150f;

    //     SlideSpace(new Vector2(0f, 0f), new Vector2(2000f, 2000f), true);
    // }


    // // ???? ?????? ??? ???
    // void SlideSpace(Vector2 sliceStart, Vector2 sliceEnd, bool sliceDir)
    // {

    //     Vector2 size = sliceEnd - sliceStart;


    //     if (sliceDir)
    //     {
    //         float ranX = (Mathf.Round(Random.Range(0.4f, 0.6f) * 10) * 0.1f);

    //         float center = sliceStart.x + (size.x *  ranX);
    //         float sliceStandard = center;

    //         Vector2 newSliceStart1 = sliceStart;
    //         Vector2 newSliceEnd1 = new Vector2(sliceStandard, sliceEnd.y);

    //         Vector2 newSliceStart2 = new Vector2(sliceStandard, sliceStart.y);
    //         Vector2 newSliceEnd2 = sliceEnd;


    //         if ((newSliceEnd1.x - newSliceStart1.x) > minRoomWidth && (newSliceEnd1.y - newSliceStart1.y) > minRoomHeight)
    //         {
    //             SlideSpace(newSliceStart1, newSliceEnd1, !sliceDir);
    //             SlideSpace(newSliceStart2, newSliceEnd2, !sliceDir);
    //         }
    //         else
    //         {
    //             CreateRoom(sliceStart, sliceEnd);
    //             return;
    //         }


    //     }
    //     else
    //     {
    //         float ranY = Mathf.Round(Random.Range(0.4f, 0.6f) * 10) * 0.1f;
    //         float center = sliceStart.y + (size.y * ranY);

    //         float sliceStandard = center;

    //         Vector2 newSliceStart1 = sliceStart;
    //         Vector2 newSliceEnd1 = new Vector2(sliceEnd.x, sliceStandard);

    //         Vector2 newSliceStart2 = new Vector2(sliceStart.x, sliceStandard);
    //         Vector2 newSliceEnd2 = sliceEnd;


    //         if (newSliceEnd1.x - newSliceStart1.x > minRoomWidth && newSliceEnd1.y - newSliceStart1.y > minRoomHeight)
    //         {
    //             SlideSpace(newSliceStart1, newSliceEnd1, !sliceDir);
    //             SlideSpace(newSliceStart2, newSliceEnd2, !sliceDir);
    //         }
    //         else
    //         {
    //             CreateRoom(sliceStart, sliceEnd);
    //             return;
    //         }

    //     }
    // }           

    // //?? ?????
    // void CreateRoom(Vector2 locS, Vector2 locE)
    // {
    //     Vector2 size = new Vector2(test.bounds.size.x, test.bounds.size.y);
    //     float locX = locS.x + (Mathf.Abs(locE.x - locS.x)) / 2;
    //     float locY = locS.y + (Mathf.Abs(locE.y - locS.y)) / 2;

    //     Instantiate(tilePrefeb, new Vector2(locX, locY), Quaternion.identity);
    // } 

    // void CreateWay()
    // {

    // }
}

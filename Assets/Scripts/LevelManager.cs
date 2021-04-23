using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //맵 크기
    float mapX = 0;
    float mapY = 0;

    float minRoomWidth = 0;
    float minRoomHeight = 0;

    [SerializeField]
    private GameObject tilePrefeb;

    private BoxCollider2D test;
    private PolygonCollider2D test2;


    void Awake()
    {

    }

    void Start()
    {
        test = tilePrefeb.GetComponent<BoxCollider2D>();

        minRoomWidth = 150f;
        minRoomHeight = 150f;

        SlideSpace(new Vector2(0f, 0f), new Vector2(2000f, 2000f), true);
    }


    // 공간 가르는 재귀 함수
    void SlideSpace(Vector2 sliceStart, Vector2 sliceEnd, bool sliceDir)
    {

        Vector2 size = sliceEnd - sliceStart;


        if (sliceDir)
        {
            float ranX = (Mathf.Round(Random.Range(0.4f, 0.6f) * 10) * 0.1f);

            float center = sliceStart.x + (size.x *  ranX);
            float sliceStandard = center;

            Vector2 newSliceStart1 = sliceStart;
            Vector2 newSliceEnd1 = new Vector2(sliceStandard, sliceEnd.y);

            Vector2 newSliceStart2 = new Vector2(sliceStandard, sliceStart.y);
            Vector2 newSliceEnd2 = sliceEnd;


            if ((newSliceEnd1.x - newSliceStart1.x) > minRoomWidth && (newSliceEnd1.y - newSliceStart1.y) > minRoomHeight)
            {
                SlideSpace(newSliceStart1, newSliceEnd1, !sliceDir);
                SlideSpace(newSliceStart2, newSliceEnd2, !sliceDir);
            }
            else
            {
                CreateRoom(sliceStart, sliceEnd);
                return;
            }


        }
        else
        {
            float ranY = Mathf.Round(Random.Range(0.4f, 0.6f) * 10) * 0.1f;
            float center = sliceStart.y + (size.y * ranY);

            float sliceStandard = center;

            Vector2 newSliceStart1 = sliceStart;
            Vector2 newSliceEnd1 = new Vector2(sliceEnd.x, sliceStandard);

            Vector2 newSliceStart2 = new Vector2(sliceStart.x, sliceStandard);
            Vector2 newSliceEnd2 = sliceEnd;


            if (newSliceEnd1.x - newSliceStart1.x > minRoomWidth && newSliceEnd1.y - newSliceStart1.y > minRoomHeight)
            {
                SlideSpace(newSliceStart1, newSliceEnd1, !sliceDir);
                SlideSpace(newSliceStart2, newSliceEnd2, !sliceDir);
            }
            else
            {
                CreateRoom(sliceStart, sliceEnd);
                return;
            }

        }



    }           

    //방 만들기
    void CreateRoom(Vector2 locS, Vector2 locE)
    {
        Vector2 size = new Vector2(test.bounds.size.x, test.bounds.size.y);
        float locX = locS.x + (Mathf.Abs(locE.x - locS.x)) / 2;
        float locY = locS.y + (Mathf.Abs(locE.y - locS.y)) / 2;

        Instantiate(tilePrefeb, new Vector2(locX, locY), Quaternion.identity);
    } 

    void CreateWay()
    {

    }
}

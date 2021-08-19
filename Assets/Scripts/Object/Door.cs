using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorDir {
    left,
    right,
    up,
    down
}

public class Door : MonoBehaviour
{
    int mapLocX;
    int mapLocY;
    public GameObject mapConfiner;

    public DoorDir dir;

    public  Map map;
    void Start()
    {
        map = transform.parent.transform.parent.GetComponent<Map>();
        mapConfiner = GameObject.Find("MapConfiner");
    }

    public void Move(){
        GameObject player = GameManager.Instance.player.gameObject;
        switch(dir) {
            case DoorDir.left:
                if(LevelManager.Instance.map[map.mapLocX-1 , map.mapLocY] != null){
                    GameManager.Instance.player.transform.position = new Vector2(((map.mapLocX+1) - 1) * 6000f - 36000f, (map.mapLocY+1) * 6000f - 36000);
                    mapConfiner.transform.position = new Vector2(((map.mapLocX + 1) - 1) * 6000f - 36000f, (map.mapLocY + 1) * 6000f - 36000);
                }
                else {
                    Debug.Log("방이 없음");
                }
                break;
            case DoorDir.right:
                if(LevelManager.Instance.map[map.mapLocX+1 , map.mapLocY] != null){
                    GameManager.Instance.player.transform.position = new Vector2(((map.mapLocX+1) + 1) * 6000f - 36000f, (map.mapLocY+1) * 6000f - 36000);
                    mapConfiner.transform.position = new Vector2(((map.mapLocX + 1) + 1) * 6000f - 36000f, (map.mapLocY + 1) * 6000f - 36000);
                }
                else {
                    Debug.Log("방이 없음");
                }
            break;
            case DoorDir.up:
                if(LevelManager.Instance.map[map.mapLocX , map.mapLocY+1] != null){
                    GameManager.Instance.player.transform.position = new Vector2((map.mapLocX+1) * 6000f - 36000f, ((map.mapLocY+1)+1) * 6000f - 36000);
                   mapConfiner.transform.position = new Vector2((map.mapLocX + 1) * 6000f - 36000f, ((map.mapLocY + 1) + 1) * 6000f - 36000);
                }
                else {
                    Debug.Log("방이 없음");
                }
            break;
            case DoorDir.down:
                if(LevelManager.Instance.map[map.mapLocX , map.mapLocY-1] != null){
                    GameManager.Instance.player.transform.position = new Vector2((map.mapLocX+1) * 6000f - 36000f, ((map.mapLocY+1)-1) * 6000f - 36000);
                    mapConfiner.transform.position = new Vector2((map.mapLocX + 1) * 6000f - 36000f, ((map.mapLocY + 1) - 1) * 6000f - 36000);
                }
                else {
                    Debug.Log("방이 없음");
                }
            break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSetting : MonoBehaviour
{

    public GameObject tPlayer;
    public Transform tFollowTarget;

     CinemachineVirtualCamera vcam = null;

    void Start(){ 
        vcam = GetComponent<CinemachineVirtualCamera>(); 
        tPlayer = null;
    }


    void Update(){ 
        if (tPlayer == null){ 
            tPlayer = GameObject.Find("player");
        }

        if (tPlayer != null){
            tFollowTarget = tPlayer.transform; 
            vcam.Follow = tFollowTarget; 
        } 
    } 


}

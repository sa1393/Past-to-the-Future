using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance = null;

    public Sprite[] nomalFloorTile;
    public Sprite[] SkillIconResources = new Sprite[3];

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

        SkillIconResources = Resources.LoadAll<Sprite>("UI/skill-icon-sprite");
        if(SkillIconResources[1] == null)
        {
            Debug.Log("¸ø°¡Á®¿È");
        }

        
    }

    public static ResourceManager Instance
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


}

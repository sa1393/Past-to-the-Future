using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public SpriteRenderer render;
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.color = new Color(1, 1, 1, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.Instance.enemyCount <=0)
        {
            render.color = new Color(1, 1, 1, 1);

            BoxCollider2D colider = gameObject.AddComponent<BoxCollider2D>();
            colider.isTrigger = true;
        }
    }
}

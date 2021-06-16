using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{
    Player player;
    public int damage = 0;
    public Animator animator;

    void EffectToggle()
    {
        Destroy(gameObject.GetComponent<Collider2D>());
        PolygonCollider2D col = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
        col.enabled = false;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        
    }
}

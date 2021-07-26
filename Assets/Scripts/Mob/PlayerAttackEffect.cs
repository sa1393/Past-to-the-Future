using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{
    Player player;
    public int damage = 0;
    public Animator animator;

    
    public PolygonCollider2D[] colliders;
    private int currentColliderIndex = 0;

    void EffectSetColider(int spriteNum)
    {
        colliders[currentColliderIndex].enabled = false;
        currentColliderIndex = spriteNum;
        colliders[currentColliderIndex].enabled = true;
        colliders[currentColliderIndex].isTrigger = true;
        if (currentColliderIndex == 10)
        {
            colliders[currentColliderIndex].enabled = false;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.Find("player").GetComponent<Player>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    Enemy enemy;
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    void Start()
    {
        enemy = transform.parent.GetChild(0).GetComponent<Enemy>();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.timeStop) return;

        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetChild(0).GetComponent<Enemy>().StartCoroutine("StartBaseAttack", 0.5f);
        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Content;
using UnityEngine;

public class Enemy_Aim : MonoBehaviour
{
    public Transform player;
    [SerializeField] private bool Enemy;
    [SerializeField] private bool Drone;
    [SerializeField] Transform enemy;
    [SerializeField] private Enemy enemy_life;
    [SerializeField] private Drone_Gunner drone_life;


    private bool isFacingRight = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemy)
        {
            if (enemy_life.dead)
            {
                StartCoroutine(Die());
            }
        }
        if (Drone)
        {
            if (drone_life.dead)
            {
                StartCoroutine(Die());
            }
        }
        Vector2 direction = new Vector2(player.position.x - transform.position.x, (player.position.y - transform.position.y));
        transform.up = direction;
        Flip();
    }

    private void Flip()
    {
        if (isFacingRight && player.position.x - transform.position.x < -1 || !isFacingRight && player.position.x - transform.position.x > 0)
        {
            isFacingRight = !isFacingRight;

            Vector3 localScale = enemy.transform.localScale;
            localScale.x *= -1f;
            enemy.transform.localScale = localScale;
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}

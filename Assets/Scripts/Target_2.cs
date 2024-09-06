using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_2 : MonoBehaviour
{
    private Transform player;
    [SerializeField] public float height;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(player.position.x, player.position.y + height);
    }
}

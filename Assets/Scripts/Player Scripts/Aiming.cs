using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] private Transform player;
    public bool isFacingRight = true;
    public float xValue;
    public float yValue;
    public bool canFlip;

    // Update is called once per frame
    void Update()
    {
            transform.position = new Vector3(player.position.x, player.position.y + 0.1f, transform.position.z);

            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            xValue = mousePosition.x - transform.position.x;
            yValue = mousePosition.y - transform.position.y;

            Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            transform.up = direction;

        Flip();
    }

    private void Flip()
    {
        if (isFacingRight && xValue < -0.000001 || !isFacingRight && xValue > 0)
        {
        isFacingRight = !isFacingRight;

        Vector3 localScale = player.transform.localScale;
        localScale.x *= -1f;
        player.transform.localScale = localScale;
        }
    }
}

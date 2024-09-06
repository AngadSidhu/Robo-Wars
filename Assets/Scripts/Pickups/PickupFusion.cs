using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PickupFusion : MonoBehaviour
{
    private inventory inventory;
    public GameObject item;
    private hotbar_manager hotbar;
    private Shooting shooting;
    private Rigidbody2D rb;
    private Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();
        rb = GetComponent<Rigidbody2D>();
        shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        hotbar = GameObject.FindGameObjectWithTag("HotbarManager").GetComponent<hotbar_manager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<inventory>();
        StartCoroutine(Lifetime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false && !hotbar.holdingFusion)
                {
                    inventory.isFull[i] = true;
                    Instantiate(item, inventory.slots[i].transform, false);
                    shooting.fusionAmmo = 50f;
                    hotbar.holdingFusion = true;
                    hotbar.pickedUp = true;
                    spawner.pickupNum--;
                    Destroy(gameObject);
                    break;
                }
            }
        }
        if (collision.CompareTag("Ground"))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(60f);
        spawner.pickupNum--;
        Destroy(gameObject);
    }
}

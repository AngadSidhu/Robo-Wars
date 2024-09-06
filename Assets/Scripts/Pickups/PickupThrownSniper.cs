using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupThrownSniper : MonoBehaviour
{
    private inventory inventory;
    public GameObject item;
    private hotbar_manager hotbar;
    private Shooting shooting;
    public float sniperSave;

    // Start is called before the first frame update
    void Start()
    {
        shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        hotbar = GameObject.FindGameObjectWithTag("HotbarManager").GetComponent<hotbar_manager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<inventory>();
        sniperSave = shooting.ammo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false && !hotbar.holdingSniper)
                {
                    inventory.isFull[i] = true;
                    Instantiate(item, inventory.slots[i].transform, false);
                    shooting.ammo = sniperSave;
                    hotbar.holdingSniper = true;
                    hotbar.pickedUp = true;
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}

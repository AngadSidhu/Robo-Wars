using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupThrownShotgun : MonoBehaviour
{
    private inventory inventory;
    public GameObject item;
    private hotbar_manager hotbar;
    private Shooting shooting;
    public float shotgunSave;

    // Start is called before the first frame update
    void Start()
    {
        shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        hotbar = GameObject.FindGameObjectWithTag("HotbarManager").GetComponent<hotbar_manager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<inventory>();
        shotgunSave = shooting.ammo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false && !hotbar.holdingShotgun)
                {
                    inventory.isFull[i] = true;
                    Instantiate(item, inventory.slots[i].transform, false);
                    shooting.shotgunAmmo = shotgunSave;
                    hotbar.holdingShotgun = true;
                    hotbar.pickedUp = true;
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}

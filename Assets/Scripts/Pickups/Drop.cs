using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    private Shooting shooting;
    private inventory inventory;
    private hotbar_manager hotbar;
    public int i;

    public void Start()
    {
        hotbar = GameObject.FindGameObjectWithTag("HotbarManager").GetComponent<hotbar_manager>();
        shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<inventory>();
    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Rifle"))
            {
                shooting.EquipRifle();
            }
            if (child.CompareTag("Shotgun"))
            {
                shooting.EquipShotgun();
            }
            if (child.CompareTag("Fusion"))
            {
                shooting.EquipFusion();
            }
            if (child.CompareTag("Sniper"))
            {
                shooting.EquipSniper();
            }
            else
            {
                shooting.EquipUnnarmed();
            }
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

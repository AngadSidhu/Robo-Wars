using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drop3 : MonoBehaviour
{
    private Shooting shooting;
    private inventory inventory;
    private hotbar_manager hotbar;
    public int i;

    [SerializeField] private GameObject riflethrow;
    [SerializeField] private GameObject shotgunthrow;
    [SerializeField] private GameObject fusionthrow;
    [SerializeField] private GameObject sniperthrow;
    [SerializeField] private GameObject grenadethrow;


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
        if (hotbar.slot3Equipped)
        {
            if (transform.childCount != 0)
            {
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
                    if (child.CompareTag("Grenade"))
                    {
                        shooting.EquipGrenade();
                    }
                }
            }
            if (transform.childCount == 0)
            {
                shooting.EquipUnnarmed();
            }
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            if (shooting.rifleEquipped)
            {
                Instantiate(riflethrow, shooting.FirePoint.position, shooting.FirePoint.rotation);
                hotbar.holdingRifle = false;
            }
            else if (shooting.shotgunEquipped)
            {
                Instantiate(shotgunthrow, shooting.FirePoint.position, shooting.FirePoint.rotation);
                hotbar.holdingShotgun = false;
            }
            else if (shooting.fusionEquipped)
            {
                Instantiate(fusionthrow, shooting.FirePoint.position, shooting.FirePoint.rotation);
                hotbar.holdingFusion = false;
            }
            else if (shooting.sniperEquipped)
            {
                Instantiate(sniperthrow, shooting.FirePoint.position, shooting.FirePoint.rotation);
                hotbar.holdingSniper = false;
            }
            else if (shooting.grenadeEquipped)
            {
                Instantiate(grenadethrow, shooting.FirePoint.position, shooting.FirePoint.rotation);
                hotbar.holdingGrenade = false;
            }
            GameObject.Destroy(child.gameObject);
        }
    }
}

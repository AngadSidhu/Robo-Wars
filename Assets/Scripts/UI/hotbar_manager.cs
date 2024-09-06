using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hotbar_manager : MonoBehaviour
{
    [SerializeField] private Shooting shooting;

    [SerializeField] private GameObject image1;

    [SerializeField] private Drop1 drop1;

    [SerializeField] private GameObject image2;

    [SerializeField] private Drop2 drop2;

    [SerializeField] private GameObject image3;

    [SerializeField] private Drop3 drop3;

    [SerializeField] private GameObject select1;

    [SerializeField] private GameObject select2;

    [SerializeField] private GameObject select3;

    public bool slot1Equipped;
    public bool slot2Equipped;
    public bool slot3Equipped;
    public bool switched;
    public bool pickedUp = false;
    public bool dropped;
    public bool holdingRifle = false;
    public bool holdingShotgun = false;
    public bool holdingFusion = false;
    public bool holdingSniper = false;
    public bool holdingGrenade = false;
    public bool grenadeThrown = false;

    private void Start()
    {
        select1.SetActive(true);
        select2.SetActive(false);
        select3.SetActive(false);
        slot1Equipped = true;
        slot2Equipped = false;
        slot3Equipped = false;
    }

    private void Update()
    {
        switched = false;
        dropped = false;
        if (Input.GetKeyDown("1") && shooting.canSwitch)
        {
            select3.SetActive(false);
            select2.SetActive(false);
            select1.SetActive(true);
            slot1Equipped = true;
            slot2Equipped = false;
            slot3Equipped = false;
            switched = true;
        }
        if (Input.GetKeyDown("2") && shooting.canSwitch)
        {
            select3.SetActive(false);
            select2.SetActive(true);
            select1.SetActive(false);
            slot1Equipped = false;
            slot2Equipped = true;
            slot3Equipped = false;
            switched = true;
        }
        if (Input.GetKeyDown("3") && shooting.canSwitch)
        {
            select3.SetActive(true);
            select2.SetActive(false);
            select1.SetActive(false);
            slot1Equipped = false;
            slot2Equipped = false;
            slot3Equipped = true;
            switched = true;
        }
        if (Input.GetKeyDown(KeyCode.Q) && shooting.canSwitch)
        {
            if (slot1Equipped)
            {
                drop1.DropItem();
            }
            else if (slot2Equipped)
            {
                drop2.DropItem();
            }
            else if (slot3Equipped)
            {
                drop3.DropItem();
            }
            dropped = true;
        }
        pickedUp = false;
    }
}

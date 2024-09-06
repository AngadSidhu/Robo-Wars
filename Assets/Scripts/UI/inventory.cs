using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;
    [SerializeField] private Text points;
    public int scraps = 0;
    public static int totalScraps = 0;

    private void Start()
    {
        points.text = scraps.ToString();
    }

    private void Update()
    {
        points.text = scraps.ToString();
        totalScraps = scraps;
    }
}

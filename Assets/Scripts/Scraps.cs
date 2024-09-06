using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scraps : MonoBehaviour
{
    [SerializeField] public bool smallScrap, midScrap, bigScrap;
    inventory inv;
    private int value;

    private void Start()
    {
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<inventory>();
        value = Random.Range(1,15);
        StartCoroutine(Lifetime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inv.scraps += value;
            Destroy(gameObject);
        }
    }
    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }
}

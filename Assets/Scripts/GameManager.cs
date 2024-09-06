using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform SpawnPoint1;
    [SerializeField] private Transform SpawnPoint2;
    public GameObject dronePrefab;
    public GameObject RiflePrefab;

    private bool canStartWave = true;

    private int waveNum = 1;

    // Update is called once per frame
    void Update()
    {
        if (waveNum < 10 && canStartWave)
        {
            canStartWave = false;
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        canStartWave = false;
        for (int i = 0; i < waveNum; i++)
        {

        }
        for (int i = 0; i < waveNum; i++)
        {

        }
        Instantiate(RiflePrefab, SpawnPoint1.position, SpawnPoint1.rotation);
        Instantiate(RiflePrefab, SpawnPoint2.position, SpawnPoint2.rotation);
        yield return new WaitForSeconds(10f);
        waveNum++;
        canStartWave = true;
    }
}

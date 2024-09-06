using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnObjects;

    [SerializeField] Transform player;

    public GameObject[] pickupObjects;
    private float pickupInterval = 5;
    public int pickupNum = 1;
    private int pickupMax = 15;
    private bool delay = true;

    [SerializeField] private GameObject drone;
    private float droneInterval = 10;
    [SerializeField] private GameObject droneGunner;
    private float droneGunnerInterval = 30;
    [SerializeField] private GameObject droneSpawner;
    private float droneSpawnerInterval = 120;
    [SerializeField] private GameObject droneGunnerSpawner;
    private float droneGunnerSpawnerInterval = 240;
    [SerializeField] private GameObject rifleman;
    private float riflemanInterval = 60;
    [SerializeField] private GameObject shotgunner;
    private float shotgunInterval = 90;
    [SerializeField] private GameObject fusioneer;
    private float fusioneerInterval = 90;
    [SerializeField] private GameObject sniper;
    private float sniperInterval = 90;

    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        Reposition();
        StartCoroutine(Wave1());
    }

    private void Update()
    {
        Reposition();
        if ((pickupNum < pickupMax) && delay == true)
        {
            Instantiate(pickupObjects[Random.Range(0, pickupObjects.Length)], position, Quaternion.identity);
            pickupNum++;
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Wave1 ()
    {
        StartCoroutine(spawnEnemy(droneInterval, drone));
        Reposition();
        StartCoroutine(spawnEnemy(droneInterval, drone));
        yield return new WaitForSeconds(60);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave2());
    }

    private IEnumerator Wave2()
    {
        StartCoroutine(spawnEnemy(droneGunnerInterval, droneGunner));
        StartCoroutine(spawnEnemy(droneGunnerInterval, droneGunner));
        StartCoroutine(spawnEnemy(droneInterval, drone));
        yield return new WaitForSeconds(60);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave3());
    }
    private IEnumerator Wave3()
    {
        StartCoroutine(spawnEnemy(droneSpawnerInterval, droneSpawner));
        StartCoroutine(spawnEnemy(droneSpawnerInterval, droneSpawner));
        StartCoroutine(spawnEnemy(droneGunnerInterval, droneGunner));
        yield return new WaitForSeconds(100);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave4());
    }

    private IEnumerator Wave4()
    {
        StartCoroutine(spawnEnemy(droneSpawnerInterval, droneSpawner));
        StartCoroutine(spawnEnemy(droneSpawnerInterval, droneSpawner));
        StartCoroutine(spawnEnemy(droneGunnerInterval, droneGunner));
        yield return new WaitForSeconds(140);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave5());
    }

    private IEnumerator Wave5()
    {
        StartCoroutine(spawnEnemy(droneGunnerSpawnerInterval, droneGunnerSpawner));
        StartCoroutine(spawnEnemy(droneGunnerSpawnerInterval, droneGunnerSpawner));
        StartCoroutine(spawnEnemy(droneInterval, drone));
        StartCoroutine(spawnEnemy(droneInterval, drone));
        yield return new WaitForSeconds(180);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave6());
    }

    private IEnumerator Wave6()
    {
        StartCoroutine(spawnEnemy(riflemanInterval, rifleman));
        yield return new WaitForSeconds(100);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave7());
    }

    private IEnumerator Wave7()
    {
        StartCoroutine(spawnEnemy(shotgunInterval, shotgunner));
        yield return new WaitForSeconds(100);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave8());
    }

    private IEnumerator Wave8()
    {
        StartCoroutine(spawnEnemy(fusioneerInterval, fusioneer));
        yield return new WaitForSeconds(100);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave9());
    }
    
    private IEnumerator Wave9()
    {
        StartCoroutine(spawnEnemy(sniperInterval, sniper));
        yield return new WaitForSeconds(100);
        StopAllCoroutines();
        delay = true;
        StartCoroutine(Wave10());
    }

    private IEnumerator Wave10()
    {
        StartCoroutine(spawnEnemy(droneInterval, drone));
        StartCoroutine(spawnEnemy(droneInterval, drone));
        StartCoroutine(spawnEnemy(droneGunnerInterval, droneGunner));
        StartCoroutine(spawnEnemy(droneSpawnerInterval, droneSpawner));
        StartCoroutine(spawnEnemy(droneGunnerSpawnerInterval, droneGunnerSpawner));
        StartCoroutine(spawnEnemy(riflemanInterval, rifleman));
        StartCoroutine(spawnEnemy(shotgunInterval, shotgunner));
        StartCoroutine(spawnEnemy(fusioneerInterval, fusioneer));
        StartCoroutine(spawnEnemy(sniperInterval, sniper));
        yield break;
    }



    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        Instantiate(enemy, position, Quaternion.identity);
        Reposition();
        yield return new WaitForSeconds(interval);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

    public void spawnScrap(int quantity, Vector3 position, Quaternion q)
    {
        Vector3 temp;
        for (int i = 0; i < quantity; i++)
        {
            temp = new Vector3(position.x + Random.Range(-1f, 1f), position.y + Random.Range(-1f, 1f), position.z);
            Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)], temp, q);
        }
    }

    private void Reposition()
    {
        position = new Vector3(player.position.x + (Random.Range(-50.0f, 50.0f)), 10, 0);
        while (Vector2.Distance(player.position, position) < 20)
        {
            position = new Vector3(player.position.x + (Random.Range(-50.0f, 50.0f)), 10, 0);
        }
        while (position.x > 90f)
        {
            position = new Vector3(player.position.x + (Random.Range(-50.0f, 50.0f)), 10, 0);
        }
        while (position.x < -30f)
        {
            position = new Vector3(player.position.x + (Random.Range(-50.0f, 50.0f)), 10, 0);
        }
    }

    private IEnumerator Delay()
    {
        delay = false;
        yield return new WaitForSeconds(pickupInterval);
        delay = true;
    }
}

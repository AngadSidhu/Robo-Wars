using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shotgun : MonoBehaviour
{
    [SerializeField] private Enemy shooting;
    private bool reloading = false;
    private float ammo = 25f;
    public bool canShoot = true;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform FirePoint;

    [SerializeField] private AudioSource Shotgun_shot;

    [SerializeField] private Animator Shotgun_anim;

    // Update is called once per frame
    void Update()
    {
        if(shooting.inVision && !reloading && ammo > 0 && canShoot)
        {
            canShoot = false;
            StartCoroutine(Shoot());
        }
        if (ammo <= 0 && !reloading)
        {
            StartCoroutine(Reload());
        }

    }

    private IEnumerator Shoot()
    {
        Quaternion savedFirePoint = Quaternion.identity;
        savedFirePoint = FirePoint.rotation;
        for (int i = 0; i < 5; i++)
        {
            Quaternion quaternion = Quaternion.identity;
            quaternion.eulerAngles = new Vector3(0f, 0f, Mathf.Sqrt(Mathf.Sqrt(i)));
            FirePoint.rotation = FirePoint.rotation * quaternion;
            Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            Shotgun_shot.Play();
            ammo--;
        }
        FirePoint.rotation = savedFirePoint;
        yield return new WaitForSeconds(0.75f);
        yield break;
    }

    private IEnumerator Reload()
    {
        reloading = true;
        Shotgun_anim.SetTrigger("reload");
        yield return new WaitForSeconds(6f);
        ammo = 25;
        reloading = false;
    }
}

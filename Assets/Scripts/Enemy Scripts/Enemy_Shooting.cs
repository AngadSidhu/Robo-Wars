using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooting : MonoBehaviour
{
    [SerializeField] private Enemy shooting;
    public bool reloading = false;
    private float ammo;
    public bool canShoot = true;

    [SerializeField] private bool rifleman;
    [SerializeField] private bool shotgunner;
    [SerializeField] private bool fusioneer;
    [SerializeField] private bool sniper;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform FirePoint;

    [SerializeField] private AudioSource Rifle_shot;

    [SerializeField] private Animator Rifle_anim;

    [SerializeField] private AudioSource Shotgun_shot;

    [SerializeField] private Animator Shotgun_anim;

    [SerializeField] private AudioSource Fusion_shot;

    [SerializeField] private Animator Fusion_anim;

    [SerializeField] private AudioSource Sniper_shot;

    [SerializeField] private Animator Sniper_anim;

    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        if (rifleman)
        {
            ammo = 30;
        }
        if (shotgunner)
        {
            ammo = 25;
        }
        if (fusioneer)
        {
            ammo = 50;
        }
        if (sniper)
        {
            ammo = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(shooting.inVision && !reloading && ammo > 0 && canShoot && FirePoint != null)
        {
            canShoot = false;
            StartCoroutine(Shoot());
        }
        if (ammo <= 0 && !reloading)
        {
            canShoot = false;
            StartCoroutine(Reload());
        }
        if (shooting.dead)
        {
            if (rifleman)
            {
                Rifle_anim.SetTrigger("Gun_Death");
            }
            else if (shotgunner)
            {
                Shotgun_anim.SetTrigger("Gun_Death");
            }
            else if (fusioneer)
            {
                Fusion_anim.SetTrigger("Gun_Death");
            }
            else if (sniper)
            {
                Sniper_anim.SetTrigger("Gun_Death");
            }
        }
    }

    private IEnumerator Shoot()
    {
        if (rifleman && FirePoint != null)
        {
            Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            Rifle_shot.Play();
            ammo--;
            yield return new WaitForSeconds(0.75f);
            canShoot = true;
        }
        else if(shotgunner && FirePoint != null)
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
            yield return new WaitForSeconds(1.5f);
            canShoot = true;
            yield break;
        }
        else if (fusioneer && FirePoint != null)
        {
            canShoot = false;
            for (int i = 0; i < 10; i++)
            {
                if (FirePoint != null)
                {
                    Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
                    Fusion_shot.Play();
                    ammo--;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(1.5f);
            canShoot = true;
            yield break;
        }
        else if (sniper && FirePoint != null)
        {
            canShoot = false;
            Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            Sniper_shot.Play();
            ammo--;
            yield return new WaitForSeconds(3f);
            canShoot = true;
            yield break;
        }
    }

    private IEnumerator Reload()
    {
        if (rifleman && FirePoint != null)
        {
            reloading = true;
            Rifle_anim.SetTrigger("reload");
            yield return new WaitForSeconds(7f);
            ammo = 30;
            reloading = false;
        }
        else if (shotgunner && FirePoint != null)
        {
            reloading = true;
            Shotgun_anim.SetTrigger("reload");
            yield return new WaitForSeconds(7f);
            ammo = 25;
            reloading = false;
        }
        else if (fusioneer && FirePoint != null)
        {
            reloading = true;
            Fusion_anim.SetTrigger("reload");
            yield return new WaitForSeconds(7f);
            ammo = 50;
            reloading = false;
        }
        else if (sniper && FirePoint != null)
        {
            reloading = true;
            Sniper_anim.SetTrigger("reload");
            yield return new WaitForSeconds(7f);
            tr.emitting = false;
            ammo = 2;
            reloading = false;
        }
    }
}

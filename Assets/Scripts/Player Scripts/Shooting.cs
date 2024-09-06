using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    private Camera cam1;
    [SerializeField] private Camera cam2;
    [SerializeField] private Camera cam3;
    private float zoom;

    [SerializeField] private AudioSource Rifle_shot;
    [SerializeField] private Animator Rifle_anim;

    [SerializeField] private AudioSource Shotgun_shot;
    [SerializeField] private Animator Shotgun_anim;

    [SerializeField] private AudioSource Fusion_shot;
    [SerializeField] private Animator Fusion_anim;

    [SerializeField] private AudioSource Sniper_shot;
    [SerializeField] private Animator Sniper_anim;

    [SerializeField] private AudioSource click;

    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Text ammocount;

    public Transform FirePoint;
    public GameObject bulletPrefab;
    public GameObject sniperBulletPrefab;
    public GameObject grenadePrefab;
    private hotbar_manager hotbar;
    public float ammo;
    public bool canFire = true;
    public bool canReload = false;
    private bool reloading = false;
    public bool rifleEquipped = false;
    public bool shotgunEquipped = false;
    public bool fusionEquipped = false;
    public bool sniperEquipped = false;
    public bool unnarmedEquipped = true;
    public bool grenadeEquipped = false;
    public float rifleAmmo = 30f;
    public float shotgunAmmo = 25f;
    public float fusionAmmo = 50f;
    public float sniperAmmo = 2f;
    public float grenadeAmmo = 1f;
    public bool canSwitch = true;
    public bool stop = false;

    [SerializeField] private GameObject unnarmed;

    [SerializeField] private GameObject grenade;

    [SerializeField] private GameObject rifle;

    [SerializeField] private GameObject shotgun;

    [SerializeField] private GameObject fusion;

    [SerializeField] private GameObject sniper;

    [SerializeField] private Drop1 drop1;

    [SerializeField] private Drop2 drop2;

    [SerializeField] private Drop3 drop3;

    private void Start()
    {
        cam1 = Camera.main;
        hotbar = GameObject.FindGameObjectWithTag("HotbarManager").GetComponent<hotbar_manager>();
        ammo = 0;
        unnarmedEquipped = true;
        grenadeEquipped = false;
        rifleEquipped = false;
        shotgunEquipped = false;
        fusionEquipped = false;
        sniperEquipped = false;
        rifle.SetActive(false);
        shotgun.SetActive(false);
        fusion.SetActive(false);
        sniper.SetActive(false);
        grenade.SetActive(false);
        ammocount.text = ammo.ToString();
    }
    // Update is called once per frame
    private void Update()
    {
        if (!stop)
        {
            if (ammo < 30 && rifleEquipped && !reloading)
            {
                canReload = true;
            }
            if (ammo < 25 && shotgunEquipped && !reloading)
            {
                canReload = true;
            }
            if (ammo < 50 && fusionEquipped && !reloading)
            {
                canReload = true;
            }
            if (ammo < 2 && sniperEquipped && !reloading)
            {
                canReload = true;
            }
            if (playerMovement.bubbled)
            {
                canFire = false;
            }
            if (Input.GetButtonDown("Fire1") && ammo > 0 && canFire && !reloading)
            {
                StartCoroutine(Shoot());
            }

            if (Input.GetButtonDown("Fire1") && ammo <= 0 && canFire && !reloading)
            {
                if (!unnarmedEquipped)
                {
                    click.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && canReload && !unnarmedEquipped && !grenadeEquipped)
            {
                playerMovement.canClimb = false;
                reloading = true;
                StartCoroutine(Reload());
            }
        }

        if (ammo == 0f)
        {
            ammocount.color = Color.red;
        }
        else if (ammo > 0f)
        {
            ammocount.color = new Vector4(238,255,0,255);
        }
    }

    private IEnumerator Shoot()
    {
        canSwitch = false;
        if (rifleEquipped)
        {
            canFire = false;
            Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            Rifle_shot.Play();
            ammo--;
            ammocount.text = ammo.ToString();
            canSwitch = true;
            yield return new WaitForSeconds(0.25f);
            canFire = true;
            yield break;
        }
        if (shotgunEquipped)
        {
            canFire = false;
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
                ammocount.text = ammo.ToString();
            }
            FirePoint.rotation = savedFirePoint;
            canSwitch = true;
            yield return new WaitForSeconds(0.75f);
            canFire = true;
            yield break;
        }
        if(fusionEquipped)
        {
            canFire = false;
            for (int i = 0; i < 10; i++)
            {
                if (!stop)
                {
                    Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
                    Fusion_shot.Play();
                    ammo--;
                    ammocount.text = ammo.ToString();
                    yield return new WaitForSeconds(0.05f);
                }
                if (stop)
                {
                    yield break;
                }
            }
            canSwitch = true;
            yield return new WaitForSeconds(0.5f);
            canFire = true;
            yield break;
        }
        if (sniperEquipped)
        {
            canFire = false;
            Instantiate(sniperBulletPrefab, FirePoint.position, FirePoint.rotation);
            Sniper_shot.Play();
            ammo--;
            ammocount.text = ammo.ToString();
            canSwitch = true;
            yield return new WaitForSeconds(1.5f);
            canFire = true;
            yield break;
        }
        if (grenadeEquipped)
        {
            if (hotbar.slot1Equipped)
            {
                drop1.DropItem();
            }
            else if (hotbar.slot2Equipped)
            {
                drop2.DropItem();
            }
            else if (hotbar.slot3Equipped)
            {
                drop3.DropItem();
            }
            ammo--;
            ammocount.text = ammo.ToString();
            canSwitch = true;
            EquipUnnarmed();
            yield break;
        }
    }

    private IEnumerator Reload()
    {
        canSwitch = false;
        if (rifleEquipped)
        {
            canReload = false;
            canFire = false;
            Rifle_anim.SetTrigger("reload");
            yield return new WaitForSeconds(1.5f);
            ammo = 30;
            canFire = true;
            reloading = false;
        }
        if (shotgunEquipped)
        {
            canReload = false;
            canFire = false;
            Shotgun_anim.SetTrigger("reload");
            yield return new WaitForSeconds(2f);
            ammo = 25;
            canFire = true;
            reloading = false;
        }
        if (fusionEquipped)
        {
            canReload = false;
            canFire = false;
            Fusion_anim.SetTrigger("reload");
            yield return new WaitForSeconds(2.5f);
            ammo = 50;
            canFire = true;
            reloading = false;
        }
        if (sniperEquipped)
        {
            canReload = false;
            canFire = false;
            Sniper_anim.SetTrigger("reload");
            yield return new WaitForSeconds(3f);
            ammo = 2;
            canFire = true;
            reloading = false;
        }
        canSwitch = true;
        playerMovement.canClimb = true;
        ammocount.text = ammo.ToString();
    }

    public void EquipUnnarmed()
    {
        if (!unnarmedEquipped)
        {
            if (shotgunEquipped)
            {
                shotgun.SetActive(false);
                shotgunAmmo = ammo;
            }
            if (fusionEquipped)
            {
                fusion.SetActive(false);
                fusionAmmo = ammo;
            }
            if (rifleEquipped)
            {
                rifle.SetActive(false);
                rifleAmmo = ammo;
            }
            if (sniperEquipped)
            {
                sniper.SetActive(false);
                sniperAmmo = ammo;
            }
            if (grenadeEquipped)
            {
                grenade.SetActive(false);
                grenadeAmmo = ammo;
            }
            ammo = 0;
            unnarmed.SetActive(true);
            ammocount.text = ammo.ToString();
            rifleEquipped = false;
            shotgunEquipped = false;
            fusionEquipped = false;
            sniperEquipped = false;
            grenadeEquipped = false;
            unnarmedEquipped = true;
        }
    }

    public void EquipRifle()
    {
        if (!rifleEquipped)
        {
            if (shotgunEquipped)
            {
                shotgun.SetActive(false);
                shotgunAmmo = ammo;
            }
            if (fusionEquipped)
            {
                fusion.SetActive(false);
                fusionAmmo = ammo;
            }
            if (unnarmedEquipped)
            {
                unnarmed.SetActive(false);
            }
            if (sniperEquipped)
            {
                sniper.SetActive(false);
                sniperAmmo = ammo;
            }
            if (grenadeEquipped)
            {
                grenade.SetActive(false);
                grenadeAmmo = ammo;
            }
            rifle.SetActive(true);
            unnarmedEquipped = false;
            rifleEquipped = true;
            shotgunEquipped = false;
            fusionEquipped = false;
            sniperEquipped = false;
            grenadeEquipped = false;
            ammo = rifleAmmo;
            ammocount.text = ammo.ToString();
        }
    }
    public void EquipShotgun()
    {
        if (!shotgunEquipped)
        {
            if (rifleEquipped)
            {
                rifle.SetActive(false);
                rifleAmmo = ammo;
            }
            if (fusionEquipped)
            {
                fusion.SetActive(false);
                fusionAmmo = ammo;
            }
            if (unnarmedEquipped)
            {
                unnarmed.SetActive(false);
            }
            if (sniperEquipped)
            {
                sniper.SetActive(false);
                sniperAmmo = ammo;
            }
            if (grenadeEquipped)
            {
                grenade.SetActive(false);
                grenadeAmmo = ammo;
            }
            shotgun.SetActive(true);
            unnarmedEquipped = false;
            rifleEquipped = false;
            shotgunEquipped = true;
            fusionEquipped = false;
            sniperEquipped = false;
            grenadeEquipped = false;
            ammo = shotgunAmmo;
            ammocount.text = ammo.ToString();
        }
    }
    public void EquipFusion()
    {
        if (!fusionEquipped)
        {
            if (rifleEquipped)
            {
                rifle.SetActive(false);
                rifleAmmo = ammo;
            }
            if (shotgunEquipped)
            {
                shotgun.SetActive(false);
                shotgunAmmo = ammo;
            }
            if (unnarmedEquipped)
            {
                unnarmed.SetActive(false);
            }
            if (sniperEquipped)
            {
                sniper.SetActive(false);
                sniperAmmo = ammo;
            }
            if (grenadeEquipped)
            {
                grenade.SetActive(false);
                grenadeAmmo = ammo;
            }
            fusion.SetActive(true);
            unnarmedEquipped = false;
            rifleEquipped = false;
            shotgunEquipped = false;
            fusionEquipped = true;
            sniperEquipped = false;
            grenadeEquipped = false;
            ammo = fusionAmmo;
            ammocount.text = ammo.ToString();
        }
    }
    public void EquipSniper()
    {
        if(!sniperEquipped)
        {
            if (unnarmedEquipped)
            {
                unnarmed.SetActive(false);
            }
            if (shotgunEquipped)
            {
                shotgun.SetActive(false);
                shotgunAmmo = ammo;
            }
            if (fusionEquipped)
            {
                fusion.SetActive(false);
                fusionAmmo = ammo;
            }
            if (rifleEquipped)
            {
                rifle.SetActive(false);
                rifleAmmo = ammo;
            }
            sniper.SetActive(true);
            sniperEquipped = true;
            unnarmedEquipped = false;
            rifleEquipped = false;
            shotgunEquipped = false;
            fusionEquipped = false;
            grenadeEquipped = false;
            ammo = sniperAmmo;
            ammocount.text = ammo.ToString();
        }
    }

    public void EquipGrenade()
    {
        if (!grenadeEquipped)
        {
            if (unnarmedEquipped)
            {
                unnarmed.SetActive(false);
            }
            if (shotgunEquipped)
            {
                shotgun.SetActive(false);
                shotgunAmmo = ammo;
            }
            if (fusionEquipped)
            {
                fusion.SetActive(false);
                fusionAmmo = ammo;
            }
            if (rifleEquipped)
            {
                rifle.SetActive(false);
                rifleAmmo = ammo;
            }
            if (sniperEquipped)
            {
                sniper.SetActive(false);
                sniperAmmo = ammo;
            }
            grenade.SetActive(true);
            grenadeEquipped = true;
            sniperEquipped = false;
            unnarmedEquipped = false;
            rifleEquipped = false;
            shotgunEquipped = false;
            fusionEquipped = false;
            ammo = grenadeAmmo;
            ammocount.text = ammo.ToString();
        }
    }
}

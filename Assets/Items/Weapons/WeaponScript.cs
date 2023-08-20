using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("References")]
    public GameObject shootPoint;
    public GameObject weaponSpriteHolder;
    public bool multiplesProjectilesTypes;
    public GameObject[] projectilePrefab;
    public Sprite weaponSprite;
    private Animator animator;

    [Header("Parameters")]
    public bool infiniteAmmo;
    public bool infiniteMagazine;
    public int currentAmmoInMagazine;
    public int ammoPerMagazine;
    public int currentAmmo;
    public int maxAmmo;
    public int ammoPerShot;
    public float reloadTime;
    [Space(10)]
    public bool isReloading;
    public bool canShoot;
    public float timeBetweenShot;
    [Space(10)]
    public float shootAngle;
    public int bulletsPerShot;
    public enum ShootingMode { automatic, semiAutomatic }
    public ShootingMode shootingMode;
    [Space(10)]
    public bool hasRecoil;
    public float recoilForce;
    [Space(10)]
    public Vector3 noFlipCoord;
    public Vector3 flipCoord;

    private float reloadTimer;

    void Awake()
    {
        Initialisation();
    }

    void Update()
    {
        HandleInputs();
        HandleReload();
    }

    private void OnDisable()
    {
        if (isReloading)
        {
            reloadTimer = reloadTime;
            ResetIsReloading();
        }
    }

    void Initialisation()
    {
        shootPoint = GameObject.Find("ShootPoint");
        if (shootPoint == null) { Debug.LogError("Couldn't find the ShootPoint GameObject inside the scene on this item :" + this.gameObject.name); }

        weaponSpriteHolder = GameObject.Find("WeaponSprite");
        if (weaponSpriteHolder == null) { Debug.LogError("Couldn't find the WeaponSprite GameObject inside the scene on this item :" + this.gameObject.name); }

        weaponSpriteHolder.GetComponent<SpriteRenderer>().sprite = weaponSprite;

        GameManager.instance.players.GetComponent<PlayerBehaviour>().currentWeapon = this.gameObject;
        transform.parent = GameManager.instance.players.GetComponent<PlayerBehaviour>().weaponSpot.transform;
        if (GameManager.instance.players.GetComponent<PlayerBehaviour>().weaponSpot == null) { Debug.LogError("Couldn't find the player WeaponSpot GameObject : " + this.gameObject.name); }

        transform.localPosition = Vector3.zero;

        animator = GetComponent<Animator>();

        canShoot = true;
    }

    private void HandleInputs()
    {
        if (shootingMode == ShootingMode.automatic)
        {
            if (Input.GetButton("Fire1"))
            {
                if (canShoot && currentAmmoInMagazine != 0 && !isReloading)
                {
                    Shoot();
                }
                else if (canShoot && currentAmmo != 0 && !isReloading)
                {
                    Reload();
                }
                else if (canShoot && currentAmmo == 0 && !isReloading)
                {
                    Debug.Log("Out of ammo :v !");
                }
            }
        }
        else if (shootingMode == ShootingMode.semiAutomatic)
        {
            if (Input.GetKeyDown("Fire1"))
            {
                if (canShoot && currentAmmoInMagazine != 0 && !isReloading)
                {
                    Shoot();
                }
                else if (canShoot && currentAmmo != 0 && !isReloading)
                {
                    Reload();
                }
                else if (canShoot && currentAmmo == 0 && !isReloading)
                {
                    Debug.Log("Out of ammo :v !");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && currentAmmo > 0)
            {
                Reload();
            }
            else if (currentAmmo == 0 && !isReloading)
            {
                Debug.Log("Out of ammo :v !");
            }
        }

    }

    private void HandleReload()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                ResetIsReloading();
                if (!infiniteMagazine)
                {
                    currentAmmo -= ammoPerMagazine - currentAmmoInMagazine;
                }
                currentAmmoInMagazine = ammoPerMagazine;
            }
        }
    }

    private void Reload()
    {
        reloadTimer = reloadTime;
        isReloading = true;
        canShoot = false;
        Debug.Log("Reloading !");
    }

    private void Shoot()
    {
        animator.SetBool("Shoot", true);
        canShoot = false;
        if (!infiniteAmmo)
        {
            currentAmmoInMagazine -= ammoPerShot;
        }
        Debug.Log("Pew !");
        if (ammoPerShot > 1)
        {
            for (int i = 0; i < ammoPerShot; i++)
            {
                Instantiate(projectilePrefab[0], shootPoint.transform.position, Quaternion.Euler(new Vector3(shootPoint.transform.eulerAngles.x, shootPoint.transform.eulerAngles.y, shootPoint.transform.eulerAngles.z + Random.Range(-shootAngle / 2, shootAngle / 2))));
            }
        }
        else if (shootAngle == 0)
        {
            Instantiate(projectilePrefab[0], shootPoint.transform.position, shootPoint.transform.rotation);
        }
        else if (shootAngle > 0)
        {
            Instantiate(projectilePrefab[0], shootPoint.transform.position, Quaternion.Euler(new Vector3(shootPoint.transform.eulerAngles.x, shootPoint.transform.eulerAngles.y, shootPoint.transform.eulerAngles.z + Random.Range(-shootAngle / 2, shootAngle / 2))));
        }
        if (hasRecoil)
        {
            Vector2 dir = GameManager.instance.players.GetComponent<PlayerBehaviour>().mousePos - shootPoint.transform.position;
            Debug.DrawLine(shootPoint.transform.position, dir);
            GameManager.instance.players.GetComponent<Rigidbody2D>().AddForce(-dir.normalized * recoilForce, ForceMode2D.Impulse);
        }
        Invoke("ResetCanShoot", timeBetweenShot);
    }

    private void ResetCanShoot()
    {
        canShoot = true;
    }

    private void ResetIsReloading()
    {
        isReloading = false;
        canShoot = true;
    }

    public void SetBoolFalse(string name)
    {
        animator.SetBool(name, false);
    }
}
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponSO weaponScriptableObject;
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private GameObject weaponSpriteHolder;

    [Header("Parameters")]
    public int currentAmmo;
    public int currentAmmoInMagazine;

    [Space(10)]
    public bool isReloading;
    public bool canShoot;
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
            reloadTimer = weaponScriptableObject.reloadTime;
            ResetIsReloading();
        }
    }

    void Initialisation()
    {
        //shootPoint = GameObject.Find("ShootPoint");
        if (shootPoint == null) { Debug.LogError("Couldn't find the ShootPoint GameObject inside the scene on this item :" + this.gameObject.name); }

        //weaponSpriteHolder = GetComponentInChildren<SpriteRenderer>().gameObject;
        if (weaponSpriteHolder == null) { Debug.LogError("Couldn't find the WeaponSprite GameObject inside the scene on this item :" + this.gameObject.name); }

        weaponSpriteHolder.GetComponent<SpriteRenderer>().sprite = weaponScriptableObject.weaponSprite;

        GameManager.instance.players.GetComponent<PlayerBehaviour>().currentWeapon = this.gameObject;
        transform.parent = GameManager.instance.players.GetComponent<PlayerBehaviour>().weaponSpot.transform;
        if (GameManager.instance.players.GetComponent<PlayerBehaviour>().weaponSpot == null) { Debug.LogError("Couldn't find the player WeaponSpot GameObject : " + this.gameObject.name); }

        transform.localPosition = Vector3.zero;

        canShoot = true;
    }

    private void HandleInputs()
    {
        if (weaponScriptableObject.shootingMode == WeaponSO.ShootingMode.automatic)
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
        else if (weaponScriptableObject.shootingMode == WeaponSO.ShootingMode.semiAutomatic)
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
                if (!weaponScriptableObject.infiniteMagazine)
                {
                    currentAmmo -= weaponScriptableObject.ammoPerMagazine - currentAmmoInMagazine;
                }
                currentAmmoInMagazine = weaponScriptableObject.ammoPerMagazine;
            }
        }
    }

    private void Reload()
    {
        reloadTimer = weaponScriptableObject.reloadTime;
        isReloading = true;
        canShoot = false;
        Debug.Log("Reloading !");
    }

    private void Shoot()
    {
        canShoot = false;
        if (!weaponScriptableObject.infiniteAmmo)
        {
            currentAmmoInMagazine -= weaponScriptableObject.ammoCostPerShot;
        }
        Debug.Log("Pew !");
        if (weaponScriptableObject.ammoCostPerShot > 1)
        {
            for (int i = 0; i < weaponScriptableObject.ammoCostPerShot; i++)
            {
                Instantiate(weaponScriptableObject.projectilePrefab[0], shootPoint.transform.position, Quaternion.Euler(new Vector3(shootPoint.transform.eulerAngles.x, shootPoint.transform.eulerAngles.y, shootPoint.transform.eulerAngles.z + Random.Range(-weaponScriptableObject.shootAngle / 2, weaponScriptableObject.shootAngle / 2))));
            }
        }
        else if (weaponScriptableObject.shootAngle == 0)
        {
            Instantiate(weaponScriptableObject.projectilePrefab[0], shootPoint.transform.position, shootPoint.transform.rotation);
        }
        else if (weaponScriptableObject.shootAngle > 0)
        {
            Instantiate(weaponScriptableObject.projectilePrefab[0], shootPoint.transform.position, Quaternion.Euler(new Vector3(shootPoint.transform.eulerAngles.x, shootPoint.transform.eulerAngles.y, shootPoint.transform.eulerAngles.z + Random.Range(-weaponScriptableObject.shootAngle / 2, weaponScriptableObject.shootAngle / 2))));
        }
        if (weaponScriptableObject.hasRecoil)
        {
            Vector2 dir = GameManager.instance.players.GetComponent<PlayerBehaviour>().mousePos - shootPoint.transform.position;
            Debug.DrawLine(shootPoint.transform.position, dir);
            GameManager.instance.players.GetComponent<Rigidbody2D>().AddForce(-dir.normalized * weaponScriptableObject.recoilForce, ForceMode2D.Impulse);
        }
        Invoke("ResetCanShoot", weaponScriptableObject.timeBetweenShot);
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

    public GameObject GetWeaponShootPoint()
    {
        return this.shootPoint;
    }

    public GameObject GetWeaponSpriteHolder()
    {
        return this.weaponSpriteHolder;
    }

    public WeaponSO GetWeaponSO()
    {
        return this.weaponScriptableObject;
    }
}
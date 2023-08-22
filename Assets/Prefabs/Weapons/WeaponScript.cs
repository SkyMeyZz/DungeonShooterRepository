using UnityEngine;

public class WeaponScript : MonoBehaviour, IInteractable
{
    #region Variables
    [Header("References")]
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private GameObject weaponSpriteHolder;

    [Header("Parameters")]
    private bool isReloading;
    private bool canShoot;

    private int currentAmmo;
    private int currentAmmoInMagazine;

    private float reloadTimer;

    [Header("Weapon Parameter")]
    [SerializeField] private ShootingMode shootingMode;
    private enum ShootingMode { automatic, semiAutomatic }
    [Space(20)]
    [SerializeField] private bool infiniteAmmo;
    [SerializeField] private bool infiniteMagazine;
    [Space(20)]
    [SerializeField] private int ammoPerMagazine;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int ammoCostPerShot;
    [SerializeField] private float reloadTime;
    [SerializeField] private float shootAngle;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float timeBetweenShot;
    [Space(20)]
    [SerializeField] private bool multiplesProjectilesTypes;
    [SerializeField] private GameObject[] projectilePrefab;
    [Space(20)]
    [SerializeField] private bool hasRecoil;
    [SerializeField] private float recoilForce;

    [Header("Visuals")]
    [SerializeField] private Sprite weaponSprite;
    [Space(10)]
    [SerializeField] private Vector3 noFlipCoord;
    [SerializeField] private Vector3 flipCoord;
    #endregion

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
        //shootPoint = GameObject.Find("ShootPoint");
        if (shootPoint == null) { Debug.LogError("Couldn't find the ShootPoint GameObject inside the scene on this item :" + this.gameObject.name); }

        //weaponSpriteHolder = GetComponentInChildren<SpriteRenderer>().gameObject;
        if (weaponSpriteHolder == null) { Debug.LogError("Couldn't find the WeaponSprite GameObject inside the scene on this item :" + this.gameObject.name); }

        weaponSpriteHolder.GetComponent<SpriteRenderer>().sprite = weaponSprite;

        GameManager.instance.players.GetComponent<PlayerBehaviour>().currentWeapon = this.gameObject;
        transform.parent = GameManager.instance.players.GetComponent<PlayerBehaviour>().weaponSpot.transform;
        if (GameManager.instance.players.GetComponent<PlayerBehaviour>().weaponSpot == null) { Debug.LogError("Couldn't find the player WeaponSpot GameObject : " + this.gameObject.name); }

        transform.localPosition = Vector3.zero;

        if(infiniteAmmo)
        {
            currentAmmoInMagazine = 1;
        }
        else currentAmmoInMagazine = ammoPerMagazine;

        currentAmmo = maxAmmo;

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
        reloadTimer =  reloadTime;
        isReloading = true;
        canShoot = false;
    }

    private void Shoot()
    {
        canShoot = false;
        if (!infiniteAmmo)
        {
            currentAmmoInMagazine -= ammoCostPerShot;
        }
        if (ammoCostPerShot > 1)
        {
            for (int i = 0; i < ammoCostPerShot; i++)
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

    public GameObject GetWeaponShootPoint()
    {
        return this.shootPoint;
    }

    public GameObject GetWeaponSpriteHolder()
    {
        return this.weaponSpriteHolder;
    }

    public Vector3 GetFlipCoords()
    {
        if (weaponSpriteHolder.GetComponent<SpriteRenderer>().flipY)
        {
            return this.flipCoord;
        }
        else if (!weaponSpriteHolder.GetComponent<SpriteRenderer>().flipY)
        {
            return this.noFlipCoord;
        }
        else return Vector3.zero;
    }

    public Sprite GetWeaponSprite()
    {
        return this.weaponSprite;
    }

    public void Interact(GameObject interactor)
    {
        //interact
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
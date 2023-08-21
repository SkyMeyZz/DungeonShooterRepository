using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movements")]
    public float moveSpeed;
    Vector2 movement;

    [Header("Camera")]
    public Camera cam;
    public GameObject cameraPoint;
    public Vector3 mousePos;

    [Header("Weapon Handling")]
    [SerializeField]
    private List<GameObject> weapons;
    public GameObject weaponSpot;
    public GameObject currentWeapon;
    private WeaponScript weaponScript;
    [SerializeField]
    private int weaponListIndex;

    private Rigidbody2D rb;
    private Rigidbody2D weaponRb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        weaponRb = weaponSpot.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInputs();
        HandleWeapon();
        SelectWeapon();

        cameraPoint.transform.position = Vector3.Lerp(rb.position, mousePos, 0.25f); //calcule et assigne la position de l'objet cameraPoint utilisé pour la position de la camera
    }

    private void FixedUpdate()
    {
        rb.AddForce(movement * moveSpeed * 100 * Time.fixedDeltaTime, ForceMode2D.Force);

    }

    private void HandleWeapon()
    {
        if (weapons.Count != 0)
        {
            weaponSpot.SetActive(true);
            weaponScript = currentWeapon.GetComponent<WeaponScript>();
            if (weaponScript == null) { Debug.LogError("Couldn't find the WeaponScript script inside the current helded weapon : " + currentWeapon.name); }

            //Handle WeaponSpot rotation
            Vector2 lookDir = mousePos - weaponScript.GetWeaponShootPoint().transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            if (Mathf.Sqrt(Mathf.Pow(lookDir.x, 2) + Mathf.Pow(lookDir.y, 2)) > 1.5f)
            {
                weaponRb.rotation = angle;
            }
            Debug.DrawLine(weaponScript.GetWeaponShootPoint().transform.position, mousePos);


            //Handle gun flip and arm rotation
            if (weaponScript.GetWeaponSpriteHolder().GetComponent<SpriteRenderer>().flipY)
            {
                weaponSpot.transform.localPosition = new Vector2(-0.65f, -0.25f);
                weaponScript.GetWeaponSpriteHolder().transform.localPosition = weaponScript.flipCoord;
                if (weaponRb.rotation >= -45 && weaponRb.rotation < 45)
                {
                    weaponScript.GetWeaponSpriteHolder().GetComponent<SpriteRenderer>().flipY = false;
                }
            }
            else if (!weaponScript.GetWeaponSpriteHolder().GetComponent<SpriteRenderer>().flipY)
            {
                weaponSpot.transform.localPosition = new Vector2(0.65f, -0.25f);
                weaponScript.GetWeaponSpriteHolder().transform.localPosition = weaponScript.noFlipCoord;
                if (weaponRb.rotation < -135 || weaponRb.rotation >= 135)
                {
                    weaponScript.GetWeaponSpriteHolder().GetComponent<SpriteRenderer>().flipY = true;
                }
            }
        }
        else
        {
            weaponSpot.SetActive(false);
        }
    }

    private void HandleInputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ScollUp();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ScollDown();
        }
    }

    private void ScollDown()
    {
        if (weaponListIndex <= 0)
            weaponListIndex = weapons.Count - 1;
        else
            weaponListIndex--;
    }

    private void ScollUp()
    {
        if (weaponListIndex >= weapons.Count - 1)
            weaponListIndex = 0;
        else
            weaponListIndex++;
    }

    public void AddWeapon(GameObject weapon)
    {
        weapon = Instantiate(weapon, transform.position, transform.rotation);
        weapons.Add(weapon);
        weaponListIndex = weapons.Count - 1;
        currentWeapon = weapons[weapons.Count - 1];
    }

    public void DropWeapon()
    {
        ScollDown();
        weapons.Remove(currentWeapon);
    }

    private void SelectWeapon()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            if (i == weaponListIndex)
            {
                weapon.SetActive(true);
                currentWeapon = weapons[i];
            }
            else
                weapon.SetActive(false);
            i++;
        }
    }
}

using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject heldWeapon;
    private WeaponScript heldWeaponScript;
    private SpriteRenderer spriteRenderer;
    public bool canBePickedUp;

    private void Awake()
    {
        if (heldWeapon == null) { Debug.LogError("no heldItem GameObject in " + this.gameObject.name); }
        heldWeaponScript = heldWeapon.GetComponent<WeaponScript>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = heldWeaponScript.weaponSprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canBePickedUp)
        {
            GameManager.instance.players.GetComponent<PlayerBehaviour>().AddWeapon(heldWeapon);
            Pickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePickedUp = true;
            Debug.Log("Can pickup a weapon : " + heldWeapon.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePickedUp = false;
            Debug.Log("Can no longer pickup : " + heldWeapon.name);
        }
    }
    public void Pickup()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }
}

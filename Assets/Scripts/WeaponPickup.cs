using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject heldWeapon;
    private WeaponScript heldWeaponScript;

    private void Awake()
    {
        if (heldWeapon == null) { Debug.LogError("no heldItem GameObject in " + this.gameObject.name); }
        heldWeaponScript = heldWeapon.GetComponent<WeaponScript>();
    }

    public void Interact(GameObject interactor)
    {
        heldWeapon = Instantiate(heldWeapon, transform.position, transform.rotation);
        interactor.GetComponent<PlayerBehaviour>().AddWeapon(heldWeapon);
        heldWeapon.transform.parent = interactor.transform;
        heldWeapon.transform.localPosition = Vector3.zero;

        GetComponent<CircleCollider2D>().enabled = false;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public GameObject GetheldWeaponPrefab()
    {
        return heldWeapon;
    }


}

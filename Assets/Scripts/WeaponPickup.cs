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
        interactor.GetComponent<PlayerBehaviour>().AddWeapon(heldWeapon);
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(this.gameObject);
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

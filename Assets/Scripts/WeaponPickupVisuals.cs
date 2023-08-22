using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupVisuals : MonoBehaviour
{
    [SerializeField] private WeaponPickup weaponPickup;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponPickup.GetheldWeaponPrefab().GetComponent<WeaponScript>().GetWeaponSprite();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("Visuals")]
    public Sprite weaponSprite;

    [Header("Weapon Parameter")]
    public ShootingMode shootingMode;
    public enum ShootingMode { automatic, semiAutomatic }
    [Space(20)]
    public bool infiniteAmmo;
    public bool infiniteMagazine;
    [Space(20)]
    public int ammoPerMagazine;
    public int maxAmmo;
    public int ammoCostPerShot;
    public float reloadTime;
    public float shootAngle;
    public int bulletsPerShot;
    public float timeBetweenShot;
    [Space(20)]
    public bool multiplesProjectilesTypes;
    public GameObject[] projectilePrefab;
    [Space(20)]
    public bool hasRecoil;
    public float recoilForce;

}

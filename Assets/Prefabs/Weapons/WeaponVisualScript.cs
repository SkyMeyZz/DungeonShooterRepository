using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualScript : MonoBehaviour
{
    private SpriteRenderer spriteRender;
    [SerializeField] private GameObject shootPoint;
    private bool flipState;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        flipState = spriteRender.flipY;
    }
    private void Update()
    {
        if(spriteRender.flipY != flipState)
        {
            flipState = !flipState;
            transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y, transform.localPosition.z);
            shootPoint.transform.localPosition = new Vector3(shootPoint.transform.localPosition.x, -shootPoint.transform.localPosition.y, shootPoint.transform.localPosition.z);
        }
    }
}

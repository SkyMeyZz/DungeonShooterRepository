using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakinGunProjectileScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed;

    private void Awake()
    {
        transform.position += transform.up * Random.Range(-0.2f, 0.2f);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}

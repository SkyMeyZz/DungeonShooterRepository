using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileScript : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * /*Random.Range(10f, 20f)*/15f, ForceMode2D.Impulse);
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}

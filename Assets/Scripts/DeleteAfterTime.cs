using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterTime : MonoBehaviour
{
    public float time;

    private void Awake()
    {
        Destroy(this.gameObject, time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetDrop : MonoBehaviour
{
    private void Start()
    {
        Invoke("SelfDestruct", 30f);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.tag.Equals("Player")) return;
        var playerController = collider.GetComponent<PlayerController>();
        playerController.resistCharges = 2;
        Destroy(gameObject);
    }
}
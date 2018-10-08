using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadSpriteController : MonoBehaviour
{
    public GameObject normalSprite;
    public GameObject eatedSprite;
    public GameObject dryedSprite;
    public GameObject eatedDryedSprite;
    public GameObject infectedSprite;
    public GameObject eatedInfectedSprite;
    private List<GameObject> list = new List<GameObject>();

    void Start()
    {
        list.Add(normalSprite);
        list.Add(eatedSprite);
        list.Add(dryedSprite);
        list.Add(eatedDryedSprite);
        list.Add(infectedSprite);
        list.Add(eatedInfectedSprite);
    }

    void Update()
    {
        PlayerController playerController = GetComponentInParent<PlayerController>();
        foreach (GameObject gameObject in list)
        {
            gameObject.SetActive(false);
        }

        switch (playerController.breadType)
        {
            case PlayerController.EnumBreadType.NORMAL:
                normalSprite.SetActive(true);
                break;
            case PlayerController.EnumBreadType.DRYED:
                dryedSprite.SetActive(true);
                break;
            case PlayerController.EnumBreadType.DRYED_EATED:
                eatedDryedSprite.SetActive(true);
                break;
            case PlayerController.EnumBreadType.EATED:
                eatedSprite.SetActive(true);
                break;
            case PlayerController.EnumBreadType.EATED_INFECTED:
                eatedInfectedSprite.SetActive(true);
                break;
            case PlayerController.EnumBreadType.INFECTED:
                infectedSprite.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
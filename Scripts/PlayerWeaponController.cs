using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        player.controlls.Character.Fire.performed += ctx => Shoot();
    }

    private void Shoot()
    {
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
}

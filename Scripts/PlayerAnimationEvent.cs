
using System;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver()
    {
        visualController.ReturnRighWeightToOne();
    }
}

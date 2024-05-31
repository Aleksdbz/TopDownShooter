
using System;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

    private PlayerWeaponVisuals _visuals;

    private void Start()
    {
        _visuals = GetComponentInParent<PlayerWeaponVisuals>();
    }

    public void ReloadIsOver()
    {
        _visuals.MaximizeRighWeight();
    }

    public void ReturnRig()
    {
        _visuals.MaximizeRighWeight();
        _visuals.MaximizeLeftHandWeight();
    }

    public void WeaponGrabIsOver()
    {
        _visuals.SetBusyGrabbingWeapon(false);
    }
}

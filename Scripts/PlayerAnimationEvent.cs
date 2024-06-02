
using System;
using UnityEngine;
/*
    This script defines animation events triggered during player actions.
    Features:
    - Handles animation events related to reloading, returning rig, and weapon grabbing.
    - Communicates with the PlayerWeaponVisuals component to update visuals accordingly.
    Dependencies:
    - Requires the PlayerWeaponVisuals component to be present in the parent GameObject.
*/
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

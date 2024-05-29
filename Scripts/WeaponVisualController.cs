using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{


  private Animator anim;

  [SerializeField] private Transform[] gunTransforms;
  [SerializeField] private Transform pistol;
  [SerializeField] private Transform revolver;
  [SerializeField] private Transform autoRifle;
  [SerializeField] private Transform shotgun;
  [SerializeField] private Transform rifle;

  private Transform currentGun;

  [Header("Left Hand Ik")] [SerializeField]
  private Transform leftHand;


  private void Start()
  {
    SwitchOn(pistol);
    anim = GetComponentInParent<Animator>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1)) //TEMPORARY PLACEHOLDER FOR TESTING AND DEBUGGING PURPOSES. IT’S GOING TO BE REPLACED LATER WITH A NEW INPUT SYSTEM AND MORE OPTIMISED CODE ITS BAD CODE PRACTICE AND ITS NOT CLEAN BUT WILL FIX IT LATER 
    //FOR NOW JUST NEED IT TO WORK
    {
      SwitchOn(pistol);
      SwitchAnimationLayer(1);
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      SwitchOn(revolver); 
      SwitchAnimationLayer(1);
    }

    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      SwitchOn(autoRifle);
      SwitchAnimationLayer(1);
    }

    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      SwitchOn(shotgun);
      SwitchAnimationLayer(2);
    }

    if (Input.GetKeyDown(KeyCode.Alpha5))
    {
      SwitchOn(rifle);
      SwitchAnimationLayer(3);
    }
    
    
  }

  private void SwitchOn(Transform gunTransforms)
  {
    SwitchOffGuns();
    gunTransforms.gameObject.SetActive(true);
    currentGun = gunTransforms;

    AttachLeftHand();
  }

  private void SwitchOffGuns()
  {
    for (int i = 0; i < gunTransforms.Length; i++)
    {
      gunTransforms[i].gameObject.SetActive(false);
    }
  }

  private void AttachLeftHand()
  {
    Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

    leftHand.localPosition = targetTransform.localPosition;
    leftHand.localRotation = targetTransform.localRotation;
  }

  private void SwitchAnimationLayer(int layerIndex)
  {
    for (int i = 1; i < anim.layerCount; i++)
    {
      anim.SetLayerWeight(i,0);
    }
    anim.SetLayerWeight(layerIndex,1);
  }
  
}


using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class PlayerWeaponVisuals : MonoBehaviour
{
  //TEMPORARY PLACEHOLDER FOR TESTING AND DEBUGGING PURPOSES. IT’S GOING TO BE REPLACED LATER WITH A NEW INPUT SYSTEM AND MORE OPTIMISED CODE ITS BAD CODE PRACTICE AND ITS NOT CLEAN BUT WILL FIX IT LATER 
  //FOR NOW JUST NEED IT TO WORK


  private Animator anim;

  [SerializeField] private Transform[] gunTransforms;
  [SerializeField] private Transform pistol;
  [SerializeField] private Transform revolver;
  [SerializeField] private Transform autoRifle;
  [SerializeField] private Transform shotgun;
  [SerializeField] private Transform rifle;

  private Transform currentGun;

  [Header("Rig")] [SerializeField] private float righWeightIncreaseRate;
  private bool shouldIncrease_RigWeight;

  [Header("Left Hand Ik")] 
  [SerializeField] private TwoBoneIKConstraint leftHandIK;
  [SerializeField] private Transform leftHandIk_Target;
  [SerializeField] private float lafthandIKWeightIncreaseRate;
  private bool shouldIncrease_LeftHandIKweight;
  private bool isGrabbingWeapon;
  private Rig rig;
  


  private void Start()
  {
    
    anim = GetComponentInChildren<Animator>();
    rig = GetComponentInChildren<Rig>();
    SwitchOn(pistol);
    
  }

  private void Update()
  {
    CheckWeaponSwitch();
    if (Input.GetKeyDown(KeyCode.R) && isGrabbingWeapon == false)
    {
      anim.SetTrigger("Reload");
      rig.weight = 0.15f;
    }

    UpdatRigWight();

    UpdateLeftHandIKWeight();
  }

  private void UpdateLeftHandIKWeight()
  {
    if (shouldIncrease_LeftHandIKweight)
    {
      leftHandIK.weight += lafthandIKWeightIncreaseRate * Time.deltaTime;
      if (leftHandIK.weight >= 1)
      {
        shouldIncrease_LeftHandIKweight = false;
      }
    }
  }

  private void UpdatRigWight()
  {
    if (shouldIncrease_RigWeight)
    {
      rig.weight += righWeightIncreaseRate * Time.deltaTime;
      if (rig.weight >= 1)
        shouldIncrease_RigWeight = false;
    }
  }
  private void PlayWeaponGrabAnimation(GrabType grabType)
  {
      leftHandIK.weight = 0;
      anim.SetFloat("WeaponGrabType",(float)grabType);
      anim.SetTrigger("WeaponGrab");
      SetBusyGrabbingWeapon(true);
  }
  public void SetBusyGrabbingWeapon(bool busy)
  {
    isGrabbingWeapon = busy;
    anim.SetBool("BusyGrabbingWeapon",isGrabbingWeapon);
  }
  public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKweight = true;
  public void MaximizeRighWeight() => shouldIncrease_RigWeight = true;
  private void CheckWeaponSwitch()
  {
    if (Input.GetKeyDown(KeyCode
          .Alpha1)) //TEMPORARY PLACEHOLDER FOR TESTING AND DEBUGGING PURPOSES. IT’S GOING TO BE REPLACED LATER WITH A NEW INPUT SYSTEM AND MORE OPTIMISED CODE ITS BAD CODE PRACTICE AND ITS NOT CLEAN BUT WILL FIX IT LATER 
      //FOR NOW JUST NEED IT TO WORK
    {
      SwitchOn(pistol);
      SwitchAnimationLayer(1);
      PlayWeaponGrabAnimation(GrabType.SideGrab);
    }

    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      SwitchOn(revolver);
      SwitchAnimationLayer(1);
      PlayWeaponGrabAnimation(GrabType.SideGrab);
    }

    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      SwitchOn(autoRifle);
      SwitchAnimationLayer(1);
      PlayWeaponGrabAnimation(GrabType.BackGrab);
    }

    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      SwitchOn(shotgun);
      SwitchAnimationLayer(2);
      PlayWeaponGrabAnimation(GrabType.BackGrab);
    }

    if (Input.GetKeyDown(KeyCode.Alpha5))
    {
      SwitchOn(rifle);
      SwitchAnimationLayer(3);
      PlayWeaponGrabAnimation(GrabType.BackGrab);
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

    leftHandIk_Target.localPosition = targetTransform.localPosition;
    leftHandIk_Target.localRotation = targetTransform.localRotation;
  }
  private void SwitchAnimationLayer(int layerIndex)
  {
    for (int i = 1; i < anim.layerCount; i++)
    {
      anim.SetLayerWeight(i, 0);
    }

    anim.SetLayerWeight(layerIndex, 1);
  }
  public enum GrabType{ SideGrab,BackGrab };
  
}

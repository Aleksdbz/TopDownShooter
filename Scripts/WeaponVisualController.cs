
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
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

  [Header("Rig")] [SerializeField] private float rigIncreaseStep;
  private bool rigShouldBeIncreased;

  [Header("Left Hand Ik")] 
  [SerializeField] private TwoBoneIKConstraint leftHandIK;
  [SerializeField] private Transform leftHandIk_Target;
  [SerializeField] private float lafthandIKIncreaseStep;
  private bool shouldIncreaseLeftHandIKweight;
  private Rig rig;

  private bool busyGrabbingWeapon;
  


  private void Start()
  {
    SwitchOn(pistol);
    anim = GetComponentInChildren<Animator>();
    rig = GetComponentInChildren<Rig>();
    Debug.Log((float)GrabType.SideGrab);
  }

  private void Update()
  {
    CheckWeaponSwitch();
    if (Input.GetKeyDown(KeyCode.R) && busyGrabbingWeapon == false)
    {
      anim.SetTrigger("Reload");
      rig.weight = 0;
    }

    UpdatRigWight();

    UpdateLeftHandIKWeight();
  }

  private void UpdateLeftHandIKWeight()
  {
    if (shouldIncreaseLeftHandIKweight)
    {
      leftHandIK.weight += lafthandIKIncreaseStep * Time.deltaTime;
      if (leftHandIK.weight >= 1)
      {
        shouldIncreaseLeftHandIKweight = false;
      }
    }
  }

  private void UpdatRigWight()
  {
    if (rigShouldBeIncreased)
    {
      rig.weight += rigIncreaseStep * Time.deltaTime;
      if (rig.weight >= 1)
        rigShouldBeIncreased = false;
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
    busyGrabbingWeapon = busy;
    anim.SetBool("BusyGrabbingWeapon",busyGrabbingWeapon);
  }
  public void ReturnWeightToLeftHandIk() => shouldIncreaseLeftHandIKweight = true;
  public void ReturnRighWeightToOne() => rigShouldBeIncreased = true;
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

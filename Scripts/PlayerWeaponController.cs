
using UnityEngine;

/*
 Features:
    - Allows the player to shoot bullets from a designated gun point towards a target.
    - Manages bullet instantiation, velocity, and destruction.
    - Handles weapon aiming towards a specified target.
 */
public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Transform weaponHolder;

    private const float REFRENCE_BULLET_SPEED = 20f; 
    //This is default speed from which my mass formula is derived.
  

    private void Start()
    {
        
        player = GetComponent<Player>();
        player.controlls.Character.Fire.performed += ctx => Shoot();
    }

    private void Shoot()
    {
      
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        newBullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;  // Calculate the velocity of the bullet
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = REFRENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet,10f);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    // Method to calculate the direction of the bullet
    public Vector3 BulletDirection()
    {

        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized; // Calculate the direction from the gun point to the aim position

        if (player.aim.CanAimPrecisley() == false && player.aim.Target() == null)
        {
            direction.y = 0;  // Ensure the bullet direction stays parallel to the ground
        }
        
        // Rotate the weapon holder and gun point to aim at the target
        //weaponHolder.LookAt(aim); TODO FIND A BETTER PLACE FOR IT
        //gunPoint.LookAt(aim);
        return direction;
    }

    public Transform GunPoint() => gunPoint;
}

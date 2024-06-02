
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
    [SerializeField] private Transform aim;

    private void Start()
    {
        
        player = GetComponent<Player>();
        player.controlls.Character.Fire.performed += ctx => Shoot();
    }

    private void Shoot()
    {
      
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        newBullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;  // Calculate the velocity of the bullet

        Destroy(newBullet,10f);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    // Method to calculate the direction of the bullet
    private Vector3 BulletDirection() 
    {
        Vector3 direction = (aim.position - gunPoint.position).normalized; // Calculate the direction from the gun point to the aim position

        if (player.aim.CanAimPrecisley() == false && player.aim.Target() == null)
        {
            direction.y = 0;  // Ensure the bullet direction stays parallel to the ground
        }
        
        // Rotate the weapon holder and gun point to aim at the target
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);
        return direction;
    }
}

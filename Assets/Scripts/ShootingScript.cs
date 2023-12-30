using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    private enum GunType
    {
        Gun1,
        Gun2,
        Melee
    }

    private GunType currentGun = GunType.Gun1;

    public GameObject bullet; // Reference to bullet prefab for Gun1
    public GameObject placeholderblaster; // Reference to placeholderblaster prefab for Gun2
    public Transform bulletTransform; // The transform where bullets spawn
    public float gun1TimeBetweenFiring = 0.2f; // Time between shots for Gun1
    public float gun2TimeBetweenFiring = 1.0f; // Time between shots for Gun2
    public float meleeTimeBetweenHits = 1.0f; // Time between melee hits
    public float bulletDistance = 2.0f; // Distance from the player to the bulletTransform
    private bool canFire = true; // Whether the player can fire
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main; // Assuming your main camera is tagged as "MainCamera"
    }

    private void Update()
    {
        // Get the cursor's position in world space
        Vector3 cursorPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Calculate a position for the bulletTransform around the player
        Vector3 playerToCursor = cursorPos - transform.position;
        Vector3 desiredBulletPos = transform.position + playerToCursor.normalized * bulletDistance;

        // Aim and cursor follow logic
        Vector3 rotation = desiredBulletPos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        bulletTransform.rotation = Quaternion.Euler(0, 0, rotZ);

        // Set the position of the bulletTransform
        bulletTransform.position = desiredBulletPos;

        // Shooting logic for both guns
        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;

            switch (currentGun)
            {
                case GunType.Gun1:
                    Instantiate(bullet, bulletTransform.position, Quaternion.identity);
                    StartCoroutine(ShootCooldown(gun1TimeBetweenFiring));
                    break;
                case GunType.Gun2:
                    Instantiate(placeholderblaster, bulletTransform.position, Quaternion.identity);
                    StartCoroutine(ShootCooldown(gun2TimeBetweenFiring));
                    break;
                case GunType.Melee:
                    UnityEngine.Debug.Log("Melee hit"); // Change 'Debug' to 'UnityEngine.Debug'
                    StartCoroutine(ShootCooldown(meleeTimeBetweenHits));
                    break;
                default:
                    break;
            }
        }

        // Gun switching
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun = GunType.Gun1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentGun = GunType.Gun2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentGun = GunType.Melee;
        }
    }

    private IEnumerator ShootCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
    }
}

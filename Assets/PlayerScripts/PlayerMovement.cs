using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dodgeForce = 500f;
    public float dodgeDuration = 0.5f;
    public float dodgeCooldown = 1f;
    public Animator animator;

    public Rigidbody2D rb;

    private Vector2 movement;
    private bool isDodging = false;
    private Vector2 dodgeDirection;
    private float dodgeCooldownTimer = 0f;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Cooldown update
        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }

        // Check for dodge input
        if (Input.GetKeyDown(KeyCode.Space) && dodgeCooldownTimer <= 0)
        {
            StartDodge();
        }
    }

    private void FixedUpdate()
    {
        if (!isDodging)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void StartDodge()
    {
        isDodging = true;
        dodgeCooldownTimer = dodgeCooldown;

        // Calculate dodge direction based on mouse cursor position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z));
        dodgeDirection = (mousePosition - (Vector3)transform.position).normalized;

        // Disable movement during dodge
        movement = Vector2.zero;

        // Apply dodge force
        rb.velocity = dodgeDirection * dodgeForce * Time.fixedDeltaTime;

        StartCoroutine(EndDodge());
    }

    private IEnumerator EndDodge()
    {
        yield return new WaitForSeconds(dodgeDuration);

        // Restore movement
        isDodging = false;
    }
}

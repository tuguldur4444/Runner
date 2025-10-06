using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    public float speed = 5;
    [SerializeField] Rigidbody rb;
    float horizontalInput;
    [SerializeField] float horizontalMultiplier = 2;

    public float speedIncrementPerPoint = 0.1f;

    [SerializeField] float jumpForce = 400f;

    [SerializeField] LayerMask groundMask;
    
    bool isGrounded = false;
    
    // Add rotation speed
    public float spinSpeed = 10f;
    
    private void FixedUpdate()
    {
        if (!alive) return;
        
        // Store forward direction before rotation
        Vector3 worldForward = Vector3.forward;
        Vector3 worldRight = Vector3.right;
        
        Vector3 forwardMove = worldForward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = worldRight * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
        
        // Just spin the player (visual only - doesn't affect movement)
        transform.Rotate(spinSpeed, 0, 0 );
        
        CheckGrounded();
    }

    void Start()
    {

    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (transform.position.y < -5)
        {
            Die();
        }
    }
    
    void CheckGrounded()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);
    }

    public void Die()
    {
        alive = false;
        Invoke("Restart", 2);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
        isGrounded = false;
    }
}
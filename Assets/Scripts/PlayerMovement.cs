using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;

    private void Awake()
    {
        Activate();
    }

    public void Activate()
    {
        enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponentInChildren<MouseLook>().enabled = true;
        controller.GetComponent<CharacterController>().enabled = true;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", z);

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

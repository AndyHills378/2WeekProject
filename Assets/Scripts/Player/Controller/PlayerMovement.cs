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

    private CharacterAiming characterAiming;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;
    public bool isWalking;
    public bool isSprinting;

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
        characterAiming = GetComponent<CharacterAiming>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        float x;
        float z;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (Input.GetButton("Sprint"))
        {
            x = Input.GetAxis("Horizontal") * 1.5f;
            z = Input.GetAxis("Vertical") * 1.5f;
        }
        if (characterAiming.playerAiming)
        {
            x = Input.GetAxis("Horizontal") * .5f;
            z = Input.GetAxis("Vertical") * .5f;
        }

        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", z);

        Vector3 move = transform.right * x + transform.forward * z;

        isWalking = !Mathf.Approximately(move.x, move.z);

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

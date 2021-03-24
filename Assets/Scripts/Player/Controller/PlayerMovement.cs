using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private GameObject[] playerBodyParts = null;
    public Camera mainCamera;
    [SerializeField] private Canvas FPSCanvas;

    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float groundDistance;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private CharacterAiming characterAiming = null;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;
    public bool isWalking;
    public bool isSprinting;

    [Client]
    public void CmdMovement()
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

        if(animator != null)
        {
            animator.SetFloat("Horizontal", x);
            animator.SetFloat("Vertical", z);
        }


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

    [ClientCallback]
    public void Start()
    {
        mainCamera.GetComponent<MouseLook>().playerBody = controller.gameObject.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponentInChildren<MouseLook>().enabled = true;

        controller.GetComponent<CharacterController>().enabled = true;
        characterAiming = GetComponent<CharacterAiming>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnStartAuthority()
    {
        enabled = true;
        foreach (GameObject playerbodypart in playerBodyParts)
        {
            playerbodypart.layer = 10;
        }
        FPSCanvas.gameObject.SetActive(true);
    }

    [ClientCallback]
    void Update() => CmdMovement();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour {

	[SerializeField] private float speed;
	[SerializeField] private float sprintSpeed;
	[SerializeField] private float sensitivity = 30.0f;
	[SerializeField] private float WaterHeight = 15.5f;
	CharacterController character;
	Animator anim;
	[SerializeField] private GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	public bool webGLRightClickRotation = true;
	float gravity = -9.8f;


	void Start(){
		//LockCursor ();
		character = GetComponent<CharacterController> ();
		anim = GetComponentInChildren<Animator>();
		if (Application.isEditor) {
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 1.5f;
		}
	}


	void CheckForWaterHeight(){
		if (transform.position.y < WaterHeight) {
			gravity = 0f;			
		} else {
			gravity = -9.8f;
		}
	}



	void Update(){

		if (Input.GetButton("Sprint"))
        {
			moveFB = Input.GetAxis("Horizontal") * sprintSpeed;
			moveLR = Input.GetAxis("Vertical") * sprintSpeed;
			anim.SetFloat("Horizontal", Input.GetAxis("Horizontal") * sprintSpeed);
			anim.SetFloat("Vertical", Input.GetAxis("Vertical") * sprintSpeed);
		}
        else
        {
			moveFB = Input.GetAxis("Horizontal") * speed;
			moveLR = Input.GetAxis("Vertical") * speed;
			anim.SetFloat("Horizontal", Input.GetAxis("Horizontal") * speed);
			anim.SetFloat("Vertical", Input.GetAxis("Vertical") * speed);
		}

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		//rotX = Input.GetKey (KeyCode.Joystick1Button4);
		//rotY = Input.GetKey (KeyCode.Joystick1Button5);

		CheckForWaterHeight ();

		Vector3 movement = new Vector3 (moveFB, gravity, moveLR);

		if (webGLRightClickRotation) {
			if (Input.GetKey (KeyCode.Mouse0)) {
				CameraRotation (cam, rotX, rotY);
			}
		} else if (!webGLRightClickRotation) {
			CameraRotation (cam, rotX, rotY);
		}

		movement = transform.rotation * movement;
		character.Move (movement * Time.deltaTime);
	}


	void CameraRotation(GameObject cam, float rotX, float rotY){		
		transform.Rotate (0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
	}




}

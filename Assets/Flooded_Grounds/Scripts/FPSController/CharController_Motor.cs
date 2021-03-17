using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour {

	[SerializeField] private float speed;
	[SerializeField] private float sensitivity = 30.0f;
	[SerializeField] private float WaterHeight = 15.5f;
	CharacterController character;
	Animator anim;
	[SerializeField] private GameObject cam;
	Vector2 input;
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

		input.x = Input.GetAxis("Horizontal") * speed;
		input.y = Input.GetAxis("Vertical") * speed;
		anim.SetFloat("Horizontal", input.x);
		anim.SetFloat("Vertical", input.y);

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		//rotX = Input.GetKey (KeyCode.Joystick1Button4);
		//rotY = Input.GetKey (KeyCode.Joystick1Button5);

		CheckForWaterHeight ();

		Vector3 movement = new Vector3 (input.x, gravity, input.y);

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

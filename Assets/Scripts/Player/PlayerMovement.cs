using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;		// Speed of player
	public float rotateSpeed = 6f; 	// Speed of rotation of player

	Vector3 movement;			// Vector to store direction of player's movement
	Quaternion rotation;		// Quaternion to store direction of player's rotation

	Animator anim;				// Reference to animator component
	Rigidbody playerRigidbody;	// Reference to player's rigidbody
	int floorMask;				// Layer mask so that a ray can be cast just at gameobjects on the floor layer
	float camRayLength = 100f;	// Length of ray from camera into the scene

	void Awake()
	{
		// Create layer mask for the floor layer
		floorMask = LayerMask.GetMask("Floor");

		// Set up references
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		KeyboardControl();
		//KeyboardAndMouseControl ();
	}

#region KeyboardOnly

	void KeyboardControl()
	{
		// Get input as 1 or 0 (does not gradually increase)
		float horizontal_input = Input.GetAxisRaw ("Horizontal");
		float vertical_input = Input.GetAxisRaw ("Vertical");
		float strafe_input = Input.GetAxisRaw ("Strafe");

		MovementLocalAxes_Keyboard (vertical_input, strafe_input);
		Turn_Keyboard (horizontal_input);
		Animating (horizontal_input, vertical_input, strafe_input);	// Animate player
	}

	void MovementLocalAxes_Keyboard(float vertical_input, float strafe_input)
	{
		movement = transform.forward * vertical_input + transform.right * strafe_input;
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turn_Keyboard(float h)
	{
		rotation = Quaternion.AngleAxis(h * rotateSpeed, Vector3.up);
		playerRigidbody.MoveRotation(transform.rotation * rotation);
	}

#endregion
	
#region KeyboardAndMouse
	void KeyboardAndMouseControl()
	{
		// Get input as 1 or 0 (does not gradually increase)
		float horizontal_input = Input.GetAxisRaw ("Horizontal");
		float vertical_input = Input.GetAxisRaw ("Vertical");

		MovementWorldAxes_Keyboard(horizontal_input,vertical_input);
		Turn_Mouse ();
		Animating (horizontal_input, vertical_input, 0);	// Animate player

	}

	void MovementWorldAxes_Keyboard(float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turn_Mouse()
	{
		// Create a ray from the mouse cursor on screen in direction of the camera
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// RaycastHit variable stores info on what was hit by the ray.
		RaycastHit floorHit;
		
		// Perform raycast and check when it hits something on the floor layer
		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) 
		{
			// Create vector from player to the floor point
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			
			// Convert vector3 to quarternion
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}
#endregion

	void Animating(float horizontal_input, float vertical_input, float strafe_input)
	{
		bool walking = horizontal_input != 0f || vertical_input != 0f || strafe_input != 0f;
		anim.SetBool ("IsWalking", walking);
	}
}
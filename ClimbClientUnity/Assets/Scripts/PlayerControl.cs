﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

		[HideInInspector]
		public bool
				facingRight = true;
		[HideInInspector]
		public bool
				jump = false;				// Condition for whether the player should jump.

		private Animator anim;
		public float moveForce = 40f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 3f;				// The fastest the player can travel in the x axis.
		public float jumpForce = 30000f;			// Amount of force added when the player jumps.
		public float climbForce = 100000f;

	private Transform groundCheckLeft;			// A position marking where to check if the player is grounded.
	private Transform groundCheckRight;
		private bool grounded = false;			// Whether or not the player is grounded.
	private bool isClimbing = false;
	private float lockedX = float.NaN;

	public int numGrapple = 6;


	public int controller = 1;
	
		// Use this for initialization
		void Awake ()
		{
				groundCheckLeft = transform.Find("groundCheckLeft");
		groundCheckRight = transform.Find ("groundCheckRight");
				anim = GetComponent<Animator> ();
		print ("test");
		}
	
		// Update is called once per frame
		void Update ()
		{
			grounded = (Physics2D.Linecast (transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Platform")) || Physics2D.Linecast (transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Platform")));
			
			if (Input.GetButtonDown("Player"+controller+"_Jump") && grounded)
						jump = true;
		
		}

		void FixedUpdate ()
		{
				// Cache the horizontal input.
		float h = Input.GetAxis ("Player"+controller+"_Move_Horizontal");
		
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				anim.SetFloat ("Speed", Mathf.Abs (h));

				// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
				if (h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
						rigidbody2D.AddForce (Vector2.right * h * moveForce);
		
				// If the player's horizontal velocity is greater than the maxSpeed...
				if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
						rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
				
				if (h < 0 && facingRight)
						Flip ();
				else if (h > 0 && !facingRight)
						Flip ();

				if (jump) {
						rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
						jump = false;
				}

			float v = Input.GetAxis ("Player"+controller+"_Climb");

			if (isClimbing) {
				print(v);
				rigidbody2D.AddForce(Vector3.up * climbForce * v);
			}
			if (!float.IsNaN(lockedX)) {
				transform.position = new Vector2(lockedX, transform.position.y);
			}

		}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Grapple")) {
			isClimbing = true;
			Physics2D.IgnoreLayerCollision(this.gameObject.layer, 8, true);
			lockedX = other.transform.position.x;
			rigidbody2D.gravityScale = 0f;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag.Equals ("Grapple")) {
			isClimbing = false;
			Physics2D.IgnoreLayerCollision(this.gameObject.layer, 8, false);
			//rigidbody2D.AddForce(Vector3.up * 5f);
			transform.Translate(Vector2.up * 0.2f);
			lockedX = float.NaN;
			rigidbody2D.gravityScale = 1f;
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
		}
	}

}

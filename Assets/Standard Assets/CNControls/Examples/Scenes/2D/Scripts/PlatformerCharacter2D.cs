using UnityEngine;

// Just in case so no "duplicate definition" stuff shows up
namespace UnityStandardAssets.Copy._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField]
        private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField]
        private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField]
        private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

		public AudioClip jump;
		AudioSource audioSource;

        private Transform m_GroundCheck;                    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f;                 // Radius of the overlap circle to determine if grounded
        static bool m_Grounded;                            // Whether or not the player is grounded.
        private Animator m_Anim;                            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        public static bool m_FacingRight = true;                  // For determining which way the player is currently facing.

		Transform playerGraphics;

        private void Awake()
        {
			audioSource = GetComponent<AudioSource>();
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
			playerGraphics = transform.Find ("Graphics");
			if (playerGraphics == null) 
			{
				Debug.LogError ("No 'Graphics'!");
			}
        }

        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

        }


        public void Move(float move, bool jump)
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));
                //m_Anim.SetBool("Speed", true);

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight) {
                    
					// ... flip the player.
					Flip ();

				}
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight) {
                    
					// ... flip the player.
					Flip ();

				}
					
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

            }
        }

		void SoundJump()
		{
			audioSource.PlayOneShot(jump, 0.7F);
		}

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
			Vector3 theScale = playerGraphics.localScale;
            theScale.x *= -1;
			playerGraphics.localScale = theScale;
			Debug.Log (playerGraphics.localScale);
        }
    }
}

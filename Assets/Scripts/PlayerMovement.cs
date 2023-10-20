using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum AnimationStates { idle, running, jumping, falling }
    private float horizontal;
    private float speed = 8f;
    private float jumpSpeed = 14f;
    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D collider;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // 1, 0, -1

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        UpdateAnimationState();
    }

    void UpdateAnimationState()
    {
        AnimationStates state;

        if (horizontal != 0)
        {
            state = AnimationStates.running;
            sprite.flipX = horizontal == -1;
        }
        else
        {
            state = AnimationStates.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = AnimationStates.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = AnimationStates.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    bool isGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}

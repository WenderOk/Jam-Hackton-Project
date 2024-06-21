using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private float movementSpeed;

    private Rigidbody2D _rigidbody;
    private float _horizontal;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        _horizontal = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate() {
        _rigidbody.velocity = new Vector2(_horizontal * this.movementSpeed, _rigidbody.velocity.y);
    }

    // void FixedUpdate()
    // {
    //     walk();
    //     Run();
    //     jump();
    //     Stamina();
    // }

    // public Vector2 moveVector;
    // public float speed = 5f;
    // public float fastSpeed = 15f;
    // private float realSpeed;

    // void walk()
    // {
    //     moveVector.x = Input.GetAxisRaw("Horizontal");
    //     anim.SetFloat("MoveX", Mathf.Abs(moveVector.x));
    //     rb.velocity = new Vector2(moveVector.x * realSpeed, rb.velocity.y);
    // }

    // public int jumpforce = 7;
    // private bool jumpControl;
    // private int jumpIteration = 0;
    // public int jumpValueIteration = 60;

    // void Flip()
    // {
    //     sr.flipX = moveVector.x < 0;
    // }

    // void jump()
    // {
    //   if (Input.GetKey(KeyCode.W))
    //     {
    //         //if (onGround) { jumpControl = true; }
    //         jumpControl = true;
    //     }
    //   else { jumpControl = false; }

    //   if (jumpControl)
    //     {
    //         if (jumpIteration++ < jumpValueIteration)
    //         {
    //             rb.AddForce(Vector2.up * jumpforce / jumpIteration);
    //         }
    //     }
    //   else { jumpIteration = 0; }
    // }
    // public bool onGround;
    // public Transform GroundCheck;
    // public float checkRadius = 0.5f;
    // public LayerMask Ground;
    
    // void CheckingGround()
    // {
    //     onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
    //     anim.SetBool("onGround", onGround);
    // }

    // public float stamina = 0f;
    // void Run()
    // {
    //     if (Input.GetKey(KeyCode.LeftShift) && onGround && stamina < 10f)
    //     {
    //         anim.SetBool("Run", true);
    //         if(fastSpeed>5f) { realSpeed = fastSpeed - stamina; }
    //         stamina += 0.15f;
    //     }
    //     else
    //     {
    //         anim.SetBool("Run", false);
    //         realSpeed = speed;
    //     }
    // }
    
    // void Stamina()
    // {
    //     if (stamina > 0f && !Input.GetKey(KeyCode.LeftShift))
    //     {
    //         stamina -= 0.1f;
    //     }
    // }
}
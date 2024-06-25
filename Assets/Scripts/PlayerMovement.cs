using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Transform bottom;

    [SerializeField] private float baseGroundAcceleration;
    [SerializeField] private float baseAirAcceleration;
    [SerializeField] private float maxAccelerationModifier;
    [SerializeField] private float maxSpeed;

    private Rigidbody2D _rigidbody;
    private Animator _anim;
    private SpriteRenderer _sr;
    private float _horizontal;
    private LayerMask _groundLayer;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update() {
        _horizontal = Input.GetAxisRaw("Horizontal");
        
        _anim.SetFloat("MoveX", Mathf.Abs(_rigidbody.velocity.x));

        if (_horizontal < 0) _sr.flipX = true;
        else if (_horizontal > 0) _sr.flipX = false;
    }

    private void FixedUpdate() {
        _rigidbody.velocity = new Vector2(
            _rigidbody.velocity.x
                + Mathf.Clamp(_horizontal * this.maxSpeed - _rigidbody.velocity.x, -this.maxAccelerationModifier, this.maxAccelerationModifier)
                    * (IsGrounded() ? this.baseGroundAcceleration : this.baseAirAcceleration)
                    * Time.fixedDeltaTime,
            _rigidbody.velocity.y
        );
    }

    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, _groundLayer);
}
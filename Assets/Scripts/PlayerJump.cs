using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private float jumpForce;
    
    [SerializeField] private float jumpStaminaCost;

    [SerializeField] private float coyoteTime; // время после отрыва от земли, в течение котого игрок сможет прыгнуть
    private float _coyoteTimeLeft;

    [SerializeField] private float jumpBufferTime; // время до падения на землю, в течение которого игрок сможет прыгнуть(сам прыжок произойдёт уже при приземлении)
    private float _jumpBufferTimeLeft;

    [SerializeField] private float fallGravityScale;
    private float _normalGravityScale;

    [SerializeField] private float maxFallSpeed;

    [SerializeField] private float maxApexSpeed; // максимальная скорость в прыжке при которой уменьшается гравитация
    [SerializeField] private float apexGravityScaleModifier;
    private bool _jumping;

    private Rigidbody2D _rigidbody;
    private Animator _anim;
    private LayerMask _groundLayer;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundLayer = LayerMask.GetMask("Ground");
        _normalGravityScale = _rigidbody.gravityScale;
        _anim = GetComponent<Animator>();
    }

    private void Update() {
        if (IsGrounded()) {
            _coyoteTimeLeft = this.coyoteTime;
            _rigidbody.gravityScale = _normalGravityScale;
            _jumping = false;
        }
        else _coyoteTimeLeft -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W)) _jumpBufferTimeLeft = this.jumpBufferTime;
        else _jumpBufferTimeLeft -= Time.deltaTime;

        if (_jumpBufferTimeLeft > 0f && _coyoteTimeLeft > 0f && this.playerStats.stamina >= this.jumpStaminaCost) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, this.jumpForce);
            _jumping = true;

            _jumpBufferTimeLeft = 0f;
            this.playerStats.stamina -= this.jumpStaminaCost;
        }

        if (Input.GetKeyUp(KeyCode.W)) {
            _coyoteTimeLeft = 0f;
            _rigidbody.gravityScale = this.fallGravityScale;
        }
        
        if (_rigidbody.velocity.y < 0)
            _rigidbody.gravityScale = this.fallGravityScale;

        if (_jumping && Mathf.Abs(_rigidbody.velocity.y) <= this.maxApexSpeed) 
            _rigidbody.gravityScale = ((_rigidbody.velocity.y > 0) ? _normalGravityScale : this.fallGravityScale) * this.apexGravityScaleModifier;
                
        _anim.SetBool("IsGrounded", IsGrounded());
    }
    private void FixedUpdate() {
        Vector2 velocity = _rigidbody.velocity;
        if (velocity.y < 0)
            velocity.y = Mathf.Clamp(velocity.y, -this.maxFallSpeed, 0);
        _rigidbody.velocity = velocity;
    }

    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, _groundLayer);
}
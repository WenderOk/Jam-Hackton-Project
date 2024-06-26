using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerJump : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;

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

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _normalGravityScale = _rigidbody.gravityScale;
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
        
        if (_rigidbody.velocity.y < 0f)
            _rigidbody.gravityScale = this.fallGravityScale;

        if (_jumping && Mathf.Abs(_rigidbody.velocity.y) <= this.maxApexSpeed) {
            _anim.SetBool("Apex", true);
            _rigidbody.gravityScale = ((_rigidbody.velocity.y > 0f) ? _normalGravityScale : this.fallGravityScale) * this.apexGravityScaleModifier;
        }
        else _anim.SetBool("Apex", false);
            
                
        _anim.SetBool("IsGrounded", IsGrounded());
        _anim.SetBool("Jumping", _jumping);
    }
    private void FixedUpdate() {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -this.maxFallSpeed, float.MaxValue));
    }

    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}
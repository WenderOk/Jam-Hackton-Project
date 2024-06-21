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

    [SerializeField] private float lowJumpGravityScale;
    [SerializeField] private float lowJumpTime;
    private float _buttonPressedTime;
    private bool _jumping;
    private float _normalGravityScale;

    private Rigidbody2D _rigidbody;
    private LayerMask _groundLayer;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundLayer = LayerMask.GetMask("Ground");
        _normalGravityScale = _rigidbody.gravityScale;
    }

    private void Update() {

        if (IsGrounded()) _coyoteTimeLeft = this.coyoteTime;
        else _coyoteTimeLeft -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W)) _jumpBufferTimeLeft = this.jumpBufferTime;
        else _jumpBufferTimeLeft -= Time.deltaTime;

        if (_jumpBufferTimeLeft > 0f && _coyoteTimeLeft > 0f && this.playerStats.stamina >= this.jumpStaminaCost) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, this.jumpForce);
            _rigidbody.gravityScale = this._normalGravityScale;
            _jumping = true;
            _jumpBufferTimeLeft = 0f;
            this.playerStats.stamina -= this.jumpStaminaCost;
            _buttonPressedTime = 0f;
        }

        if (_jumping && Input.GetKey(KeyCode.W))
            _buttonPressedTime += Time.deltaTime;

        if (_jumping && Input.GetKeyUp(KeyCode.W) && _rigidbody.velocity.y > 0f) {
            _coyoteTimeLeft = 0f;
            _jumping = false;

            if (_buttonPressedTime <= lowJumpTime) _rigidbody.gravityScale = this.lowJumpGravityScale;
        }

        if (_rigidbody.velocity.y < 0f)
            _rigidbody.gravityScale = this.fallGravityScale;
    }

    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, _groundLayer);
}
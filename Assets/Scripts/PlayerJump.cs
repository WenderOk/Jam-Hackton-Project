using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerJump : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private PlayerWallJump playerWallJump;

    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;

    [SerializeField] private ParticleSystem jumpAndLanding;

    [SerializeField] private PlayerMovement playerMovement;

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
    private bool _falling;

    private Rigidbody2D _rigidbody;
    private Animator _anim;

    [SerializeField] private float noStepAfterLandingTime; // время, в течении которого после приземпления не проигрывается звук ходьбы
    private float _stepAfterLandingTimeLeft;

    private bool _grounded;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _normalGravityScale = _rigidbody.gravityScale;
    }

    private void Update() {
        _grounded = IsGrounded();
        if (_grounded) {
            _coyoteTimeLeft = this.coyoteTime;
            _rigidbody.gravityScale = _normalGravityScale;
            _jumping = false;
            if (_falling) {
                this.playerMovement.playSound = false;
                _stepAfterLandingTimeLeft = this.noStepAfterLandingTime;
                this.audioSource2.clip = this.landingSound;
                this.audioSource2.Play();
                this.jumpAndLanding.Play();
            }
            _falling = false;
            
        }
        else _coyoteTimeLeft -= Time.deltaTime;

        _stepAfterLandingTimeLeft -= Time.deltaTime;
        if (_stepAfterLandingTimeLeft < 0) this.playerMovement.playSound = true;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) _jumpBufferTimeLeft = this.jumpBufferTime;
        else _jumpBufferTimeLeft -= Time.deltaTime;

        if (_jumpBufferTimeLeft > 0f && _coyoteTimeLeft > 0f && this.playerStats.stamina >= this.jumpStaminaCost) {
            this.audioSource1.clip = this.jumpSound;
            this.audioSource1.Play();
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, this.jumpForce);
            _jumping = true;
            _anim.SetTrigger("Jumping");
            _jumpBufferTimeLeft = 0f;
            playerWallJump.jumpBufferTimeLeft = 0;
            this.playerStats.stamina -= this.jumpStaminaCost;
            jumpAndLanding.Play();
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space)) {
            _coyoteTimeLeft = 0f;
            _rigidbody.gravityScale = this.fallGravityScale;
        }
        
        if (_rigidbody.velocity.y < 0f && !_grounded) {
            _falling = true;
            _rigidbody.gravityScale = this.fallGravityScale;
        }
        else _falling = false;

        if (_jumping && Mathf.Abs(_rigidbody.velocity.y) <= this.maxApexSpeed)
            _rigidbody.gravityScale = ((_rigidbody.velocity.y > 0) ? _normalGravityScale : this.fallGravityScale) * this.apexGravityScaleModifier;
        if (_rigidbody.velocity.y <= 0.1f) _anim.SetTrigger("Apex");

        _anim.SetBool("IsGrounded", _grounded);
    }
    private void FixedUpdate() {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -this.maxFallSpeed, float.MaxValue));
    }

    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}
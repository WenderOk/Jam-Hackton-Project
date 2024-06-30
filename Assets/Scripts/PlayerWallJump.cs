using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerWallJump : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private Transform right;
    [SerializeField] private LayerMask wallLayers;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private ParticleSystem wallJumpParticles;

    [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallJumpAngle; // 0 - горизонтально
    [SerializeField] private float wallJumpStaminaCost;

    [SerializeField] private float coyoteTime; // время после отрыва от стены, в течение котого игрок сможет прыгнуть
    private float _coyoteTimeLeft;

    [SerializeField] private float jumpBufferTime;
    [NonSerialized] public float jumpBufferTimeLeft;

    [SerializeField] private float fallGravityScale;
    private float _normalGravityScale;

    [SerializeField] private float maxFallSpeed;

    [SerializeField] private float maxApexSpeed; // максимальная скорость в прыжке при которой уменьшается гравитация
    [SerializeField] private float apexGravityScaleModifier;
    private bool _jumping;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _normalGravityScale = _rigidbody.gravityScale;
    }

    private void Update() {
        if (IsWalled()) {
            _coyoteTimeLeft = this.coyoteTime;
            _rigidbody.gravityScale = _normalGravityScale;
            _jumping = false;
        }
        else _coyoteTimeLeft -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) jumpBufferTimeLeft = this.jumpBufferTime;
        else jumpBufferTimeLeft -= Time.deltaTime;

        if (!IsGrounded() && jumpBufferTimeLeft > 0 && _coyoteTimeLeft > 0f && this.playerStats.stamina >= this.wallJumpStaminaCost) {
            _rigidbody.velocity = new Vector2(
                (_spriteRenderer.flipX ? 1f : -1f) * this.wallJumpForce * Mathf.Cos(Mathf.Deg2Rad * this.wallJumpAngle),
                this.wallJumpForce * Mathf.Sin(Mathf.Deg2Rad * this.wallJumpAngle)
            );
            _jumping = true;
            _animator.SetTrigger("Jumping");
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
            this.playerStats.stamina -= this.wallJumpStaminaCost;
            this.audioSource.clip = this.jumpSound;
            this.audioSource.Play();
            wallJumpParticles.Play();
            jumpBufferTimeLeft = 0;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space)) {
            _coyoteTimeLeft = 0f;
            _rigidbody.gravityScale = this.fallGravityScale;
        }
        
        if (_rigidbody.velocity.y < 0f)
            _rigidbody.gravityScale = this.fallGravityScale;

        if (_jumping && Mathf.Abs(_rigidbody.velocity.y) <= this.maxApexSpeed)
            _rigidbody.gravityScale = ((_rigidbody.velocity.y > 0) ? _normalGravityScale : this.fallGravityScale) * this.apexGravityScaleModifier;
        if (_rigidbody.velocity.y <= 0.1f) _animator.SetTrigger("Apex");

        _animator.SetBool("IsGrounded", IsGrounded());
    }
    private void FixedUpdate() {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -this.maxFallSpeed, float.MaxValue));
    }

    private bool IsWalled() =>
        Physics2D.OverlapCircle(this.right.position, 0.1f/*маленькое число*/, this.wallLayers);
    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}

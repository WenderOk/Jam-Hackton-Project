using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerWallSlide : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private Transform right;
    [SerializeField] private LayerMask wallLayers;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private float maxWallSlidingSpeed;
    [SerializeField] private float wallSlideStaminaCost;

    private float _horizontal;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Update() {
        _horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate() {
        if (IsWalled() && this.playerStats.stamina >= this.wallSlideStaminaCost && !IsGrounded()
                && _horizontal == (_spriteRenderer.flipX ? -1f : 1f)) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -this.maxWallSlidingSpeed, float.MaxValue));
            this.playerStats.stamina -= this.wallSlideStaminaCost;
            _anim.SetBool("IsSliding", true);
        }
        else _anim.SetBool("IsSliding", false);
    }

    private bool IsWalled() =>
        Physics2D.OverlapCircle(this.right.position, 0.1f/*маленькое число*/, this.wallLayers);
    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}

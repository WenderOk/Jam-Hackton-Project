using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerSlide : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private float slideForce;
    [SerializeField] private float groundSlideStaminaCost;
    [SerializeField] private float airSlideStaminaCost;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;
    private bool _grounded;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Update() {
        _grounded = IsGrounded();

        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;

        _anim.SetTrigger("Dash");

        if (_grounded && this.playerStats.stamina >= this.groundSlideStaminaCost) {
            _rigidbody.velocity = new Vector2((_spriteRenderer.flipX ? -1f : 1f) * this.slideForce, _rigidbody.velocity.y);
            this.playerStats.stamina -= this.groundSlideStaminaCost;
        }
        if (!_grounded && this.playerStats.stamina >= this.airSlideStaminaCost) {
            _rigidbody.velocity = new Vector2((_spriteRenderer.flipX ? -1f : 1f) * this.slideForce, _rigidbody.velocity.y);
            this.playerStats.stamina -= this.airSlideStaminaCost;
        }
    }

    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}
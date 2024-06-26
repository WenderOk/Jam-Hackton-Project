using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerWallSlide : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private Transform right;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask wallLayers;

    [SerializeField] private float maxWallSlidingSpeed;

    private float _horizontal;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;
    private float _rightBaseX;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        _rightBaseX = this.right.localPosition.x;
    }

    private void Update() {
        _horizontal = Input.GetAxisRaw("Horizontal");

        this.right.localPosition = new Vector2((_spriteRenderer.flipX ? -1f : 1f) *  _rightBaseX, this.right.localPosition.y);
    }

    private void FixedUpdate() {
        if (IsWalled() && !IsGrounded() && _horizontal == (_spriteRenderer.flipX ? -1f : 1f)) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -this.maxWallSlidingSpeed, float.MaxValue));
            _anim.SetBool("IsSliding", true);
        }
       else  _anim.SetBool("IsSliding", false);
    }

    private bool IsWalled() =>
        Physics2D.OverlapCircle(this.right.position, 0.1f/*маленькое число*/, this.wallLayers);
    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}

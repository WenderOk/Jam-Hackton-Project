using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private Transform right;
    private float _rightBaseX;
    // [SerializeField] private Transform upEdge;
    // private float _upEdgeBaseX;
    // [SerializeField] private Transform downEdge;
    // private float _downEdgetBaseX;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;

    [SerializeField] private float baseGroundAcceleration;
    [SerializeField] private float baseAirAcceleration;
    [SerializeField] private float maxAccelerationModifier;
    [SerializeField] private float maxSpeed;

    [SerializeField] private ParticleSystem steps;

    private Rigidbody2D _rigidbody;
    private Animator _anim;
    private SpriteRenderer _sr;
    private float _horizontal;
    

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        _rightBaseX = this.right.localPosition.x;
        // _upEdgeBaseX = this.upEdge.localPosition.x;
        // _downEdgetBaseX = this.downEdge.localPosition.x;
    }

    public void Step() {
        this.audioSource.PlayOneShot(this.sounds[UnityEngine.Random.Range(0, this.sounds.Length)]);
    }

    private void Update() {
        _horizontal = Input.GetAxisRaw("Horizontal");
        
        _anim.SetFloat("MoveX", Mathf.Abs( _horizontal));

        if (_horizontal < 0) _sr.flipX = true;
        else if (_horizontal > 0) _sr.flipX = false;
        this.right.localPosition = new Vector2((_sr.flipX ? -1f : 1f) *  _rightBaseX, this.right.localPosition.y);
        // this.upEdge.localPosition = new Vector2((_sr.flipX ? -1f : 1f) *  _upEdgeBaseX, this.upEdge.localPosition.y);
        // this.downEdge.localPosition = new Vector2((_sr.flipX ? -1f : 1f) *  _downEdgetBaseX, this.downEdge.localPosition.y);

        if (IsGrounded() && Mathf.Abs(_rigidbody.velocity.x) > 0)
            steps.Play();
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
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerEdgeNudging : MonoBehaviour {
    [SerializeField] private Transform upEdge;
    [SerializeField] private Transform downEdge;
    [SerializeField] private LayerMask nudgingLayers;

    [SerializeField] private Transform right;
    [SerializeField] private LayerMask wallLayers;

    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;


    [SerializeField] private float nudgingForce;
    [SerializeField] private float nudgingRange;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (IsGrounded() || IsWalled()) return;

        if (IsUpEdged()) {
            if (_rigidbody.velocity.y > 0) {
                _rigidbody.velocity = new Vector2((_spriteRenderer.flipX ? 1f : -1f) * this.nudgingForce, _rigidbody.velocity.x);
                Debug.Log("Nudged side");
            }
            else {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -this.nudgingForce);
                Debug.Log("Nudged down");
            }
        }
        if (IsDownEdged()) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, this.nudgingForce);

            Debug.Log("Nudged up");
        } 
    }

    private bool IsUpEdged() =>
        Physics2D.OverlapCircle(this.upEdge.position, this.nudgingRange, this.nudgingLayers);
    private bool IsDownEdged() =>
        Physics2D.OverlapCircle(this.downEdge.position, this.nudgingRange, this.nudgingLayers);
    private bool IsWalled() =>
        Physics2D.OverlapCircle(this.right.position, 0.1f/*маленькое число*/, this.wallLayers);
    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}

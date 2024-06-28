using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSit : MonoBehaviour {
    [SerializeField] private Transform bottom;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private float sittingStaminaRegeneration;
    private float _normalStaminaRegeneration;

    [SerializeField] private float jumpBufferTime;
    private float _jumpBufferTimeLeft;

    private bool _grounded;

    private void Awake() {
        _normalStaminaRegeneration = this.playerStats.staminaRegeneration;
    }

    private void Update() {
        _grounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.S)) _jumpBufferTimeLeft = this.jumpBufferTime;
        else _jumpBufferTimeLeft -= Time.deltaTime;

        if (_grounded && _jumpBufferTimeLeft > 0) {
            this.playerStats.staminaRegeneration = this.sittingStaminaRegeneration;

            _jumpBufferTimeLeft = 0;
        }

        if (Input.GetKeyUp(KeyCode.S) || !_grounded) {
            this.playerStats.staminaRegeneration = _normalStaminaRegeneration;
        }
    }

    private bool IsGrounded() =>
        Physics2D.OverlapCircle(this.bottom.position, 0.1f/*маленькое число*/, this.groundLayers);
}

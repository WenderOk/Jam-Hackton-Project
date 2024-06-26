using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour {
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegeneration;
    [NonSerialized] public float stamina;


    private void Awake() {
        this.stamina = this.maxStamina;
    }

    public void Update() {
        this.stamina = Mathf.Clamp(this.stamina + this.staminaRegeneration, 0f, this.maxStamina);
    }

}

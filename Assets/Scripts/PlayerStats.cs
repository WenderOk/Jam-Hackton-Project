using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    [SerializeField] private float maxStamina;
    public float staminaRegeneration;
    [SerializeField] private Image staminaBar;
    //[NonSerialized] 
    public float stamina;


    private void Awake() {
        this.stamina = this.maxStamina;
    }

    public void Update() {
        this.stamina = Mathf.Clamp(this.stamina + this.staminaRegeneration*Time.deltaTime, 0f, this.maxStamina);
        staminaBar.fillAmount = stamina/maxStamina;
    }

}

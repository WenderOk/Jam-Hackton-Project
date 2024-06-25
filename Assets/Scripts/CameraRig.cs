using UnityEngine;
using System;


[RequireComponent(typeof(Camera))]
public class CameraRig : MonoBehaviour {
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float followSpeed;
    [SerializeField] private float tiltAngle;
    [SerializeField] private float angleChangeSpeed;
    [SerializeField] private float centerOffset;
   
    private float _horizontal;
    private void Awake() {
        this.transform.position = this.objectToFollow.position;
    }

    private void FixedUpdate() {
        this.transform.position = Vector2.Lerp(
            this.transform.position,
            (Vector2)(this.objectToFollow.position) + Vector2.right * _horizontal * centerOffset,
            this.followSpeed * Time.fixedDeltaTime
        );
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f);
    }
        
    private void Update() {
        _horizontal = Input.GetAxis("Horizontal");

        this.transform.rotation = Quaternion.Lerp(
            this.transform.rotation,
            Quaternion.Euler(Vector3.forward * Input.GetAxis("Horizontal") * tiltAngle),
            this.angleChangeSpeed * Time.deltaTime
        );
    }
}

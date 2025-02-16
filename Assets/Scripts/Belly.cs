using UnityEngine;

public class Belly : MonoBehaviour
{
    Collider _collider;
    Rigidbody _rigidbody;
    [SerializeField, Range(0f, 10f)] float defaultFriction;

    void Start() {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponentInParent<Rigidbody>();
        var material = new PhysicsMaterial();
        material.dynamicFriction = defaultFriction;
        material.staticFriction = defaultFriction;
        material.bounciness = 0.3f;
        material.bounceCombine = PhysicsMaterialCombine.Average;
        material.frictionCombine = PhysicsMaterialCombine.Average;
        _collider.material = material;
    }

    void FixedUpdate() {
        Vector3 local_vel = this.transform.InverseTransformDirection(_rigidbody.linearVelocity);
        local_vel.y = 0f;
        float local_speed = local_vel.magnitude;
        // If velocity is too small    => Do nothing
        //             faces forward   => Small friction
        //             faces back,side => Big friction
        if (local_speed > Mathf.Epsilon) {
            float friction;
            friction = Mathf.Max(local_vel.z, 0f) / local_speed;
            friction = 1f - friction;
            friction *= defaultFriction;
            _collider.material.dynamicFriction = friction;
            _collider.material.staticFriction = friction;
        }
    }
}

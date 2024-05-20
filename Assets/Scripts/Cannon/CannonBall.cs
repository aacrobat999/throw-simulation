using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] public Rigidbody rigidBody;

    [HideInInspector] public bool hasHitGround = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitGround && collision.collider.CompareTag("Ground"))
        {
            CameraManager.i.StartTransitionBackToMain();
            hasHitGround = true;
        }
    }
}


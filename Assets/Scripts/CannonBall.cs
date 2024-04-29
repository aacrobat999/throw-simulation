using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [HideInInspector] public bool hasHitGround = false;
    [SerializeField] public Rigidbody rigidBody;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitGround && collision.collider.CompareTag("Ground"))
        {
            CameraManager.i.StartTransitionBackToMain();
            hasHitGround = true;
        }
    }
}


using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private bool hasHitGround = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitGround && collision.collider.CompareTag("Ground"))
        {
            CameraManager.i.StartTransitionBack();
            hasHitGround = true;
        }
    }
}


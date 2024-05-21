using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] public Rigidbody rigidBody;

    [HideInInspector] public bool hasHitGround = false;

    [SerializeField] ParticleSystem[] explosions;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitGround && collision.collider.CompareTag("Ground"))
        {
            CameraManager.i.StartTransitionBackToMain();
            TriggerExplosion();
            hasHitGround = true;
        }
    }

    void TriggerExplosion()
    {
        int randomIndex = Random.Range(0, explosions.Length);
        ParticleSystem selectedExplosion = Instantiate(explosions[randomIndex], transform.position, Quaternion.identity);
        selectedExplosion.Play();
    }
}


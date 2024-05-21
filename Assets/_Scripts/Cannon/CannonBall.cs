using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] public Rigidbody rigidBody;


    [SerializeField] ParticleSystem[] explosions;

    [SerializeField] AudioClip[] explosionSounds;

    [HideInInspector] public bool hasHitGround = false;
    
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
        int randomSound = Random.Range(0, explosionSounds.Length);
        int randomIndex = Random.Range(0, explosions.Length);
        
        ParticleSystem selectedExplosion = Instantiate(explosions[randomIndex], transform.position, Quaternion.identity);
        selectedExplosion.Play();
        AudioManager.i.PlaySfx(explosionSounds[randomSound]);
    }
}


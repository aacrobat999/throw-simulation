using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    [SerializeField] MouseLook mouseLook;

    [SerializeField, Range(0.0f, 50.0f)] public float force = 5;

    [SerializeField, Range(0.0f, 50.0f)] public float _gravityScale = 9.81f;

    [SerializeField] Transform StartPosition;

    [SerializeField] CannonBall objectToThrow;

    [HideInInspector] public CannonBall thrownObject;

    TrajectoryPredictor trajectoryPredictor;
    public InputAction fire;



    void OnEnable()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();

        if (StartPosition == null)
            StartPosition = transform;

        fire.Enable();
        fire.performed += ThrowObject;
    }

    void Update()
    {
        Physics.gravity = new Vector3(0, -_gravityScale, 0);
        Predict();
    }

    void Predict()
    {
        trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();

        properties.direction = StartPosition.forward;
        properties.initialPosition = StartPosition.position;
        properties.initialSpeed = force;
        properties.mass = r.mass;
        properties.drag = r.drag;

        return properties;
    }

    void ThrowObject(InputAction.CallbackContext ctx)
    {
        if (!mouseLook.isInUI)
        {
            CameraManager.i.StartTransitionTo();

            thrownObject = Instantiate(objectToThrow, StartPosition.position, Quaternion.identity);
            thrownObject.rigidBody.AddForce(StartPosition.forward * force, ForceMode.Impulse);

            trajectoryPredictor.SpawnTrajectoryBalls(ProjectileData());
        }
    }

    #region UI

    public void ChangeGravityScale(TMP_InputField input)
    {
        if (string.IsNullOrEmpty(input.text))
            return;

        if (float.TryParse(input.text, out float newGravityScale))
        {
            if (newGravityScale <= -20)
            {
                _gravityScale = -20;
                input.text = _gravityScale.ToString();
            }
            else if (newGravityScale >= 50)
            {
                _gravityScale = 50;
                input.text = _gravityScale.ToString();
            }
            else
            {
                _gravityScale = newGravityScale;
            }
        }
    }

    public void ChangeForceValue(TMP_InputField input)
    {
        if (string.IsNullOrEmpty(input.text))
            return;

        if (float.TryParse(input.text, out float newForce))
        {
            if (newForce <= 1)
            {
                force = 1;
                input.text = force.ToString();
            }
            else if (newForce >= 30)
            {
                force = 30;
                input.text = force.ToString();
            }
            else
            {
                force = newForce;
            }
        }
    }
    #endregion
}
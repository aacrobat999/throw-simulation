using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    [SerializeField] MouseLook mouseLook;

    [SerializeField, Range(0.0f, 50.0f)] public float force = 5;

    [SerializeField, Range(0.0f, 50.0f)] public float _gravityScale = 9.81f;

    [SerializeField] Transform startPosition;

    [SerializeField] public CannonBall objectToThrow;

    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI massText;
    [SerializeField] TextMeshProUGUI dragText;


    [HideInInspector] public CannonBall thrownObject;
    [HideInInspector] public Vector3 startingPosition;

    TrajectoryPredictor trajectoryPredictor;
    public InputAction fire;

    bool hasFired = false;

    private void Awake()
    {
        startingPosition = transform.position;
    }

    void OnEnable()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();

        if (startPosition == null)
            startPosition = transform;

        fire.Enable();
        fire.performed += ThrowObject;
    }

    void Update()
    {
        Physics.gravity = new Vector3(0, -_gravityScale, 0);
        Predict();

        if (!hasFired)
            trajectoryPredictor.currentVelocityText.text = "0";

        if (thrownObject != null && thrownObject.hasHitGround)
            hasFired = false;
    }

    void Predict()
    {
        trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();

        properties.direction = startPosition.forward;
        properties.initialPosition = startPosition.position;
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

            // Throw the object

            hasFired = true;
            thrownObject = Instantiate(objectToThrow, startPosition.position, Quaternion.identity);
            thrownObject.rigidBody.AddForce(startPosition.forward * force, ForceMode.Impulse);

            trajectoryPredictor.SpawnTrajectoryBalls(ProjectileData());
        }
    }


    #region UI
    public void UpdateDistanceText(float distance) => distanceText.text = distance.ToString("F2");

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

    public void ChangeHeightValue(TMP_InputField input)
    {
        if (string.IsNullOrEmpty(input.text))
            return;

        if (float.TryParse(input.text, out float newHeight))
        {
            if (newHeight <= 1.74)
            {
                transform.position = new Vector3(0, 0, 0) + startingPosition;
                input.text = transform.position.y.ToString();
            }
            else if (newHeight >= 20)
            {
                transform.position = new Vector3(0, 20, 0) + startingPosition;
                input.text = transform.position.y.ToString();
            }
            else
            {
                transform.position = new Vector3(0, newHeight, 0) + startingPosition;
            }
        }
    }
    public void ChangeMassValue(TMP_InputField input)
    {
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();

        if (string.IsNullOrEmpty(input.text))
            return;

        if (float.TryParse(input.text, out float newMass))
        {
            if (newMass <= 0.1)
            {
                r.mass = 0.1f;
                input.text = r.mass.ToString();
            }
            else if (newMass >= 5)
            {
                r.mass = 0.1f;
                input.text = r.mass.ToString();
            }
            else
            {
                r.mass = newMass;
            }
        }
    }
    public void ChangeDragValue(TMP_InputField input)
    {
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();

        if (string.IsNullOrEmpty(input.text))
            return;

        if (float.TryParse(input.text, out float newDrag))
        {
            if (newDrag <= 0.01)
            {
                r.drag = 0.1f;
                input.text = r.mass.ToString();
            }
            else if (newDrag >= 2)
            {
                r.drag = 2f;
                input.text = r.drag.ToString();
            }
            else
            {
                r.drag = newDrag;
            }
        }
    }
    #endregion
}

public struct ProjectileProperties
{
    public Vector3 direction;
    public Vector3 initialPosition;
    public float initialSpeed;
    public float mass;
    public float drag;
}


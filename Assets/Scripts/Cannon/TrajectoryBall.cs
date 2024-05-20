using UnityEngine;

public class TrajectoryBall : MonoBehaviour
{
    [SerializeField] GameObject directionArrow;
    float velocityValue;

    [HideInInspector] public bool isMouseOver = false;

    public void Update()
    {
        if (isMouseOver)
        {
            Debug.Log("Velocity at this point: " + velocityValue);
        }

        if (!isMouseOver)
            directionArrow.SetActive(TrajectoryPredictor.i.displayAllArrows);
    }

    public void SetVelocity(float velocity)
    {
        velocityValue = velocity;
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
        directionArrow.SetActive(true);
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        directionArrow.SetActive(false);
    }
}

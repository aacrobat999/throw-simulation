using TMPro;
using UnityEngine;

public class TrajectoryBall : MonoBehaviour
{
    [SerializeField] GameObject directionArrow;
    float velocityValue;

    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI vectorText;
    [SerializeField] TextMeshProUGUI velocityText;

    [HideInInspector] public bool isMouseOver = false;

    public void Update()
    {
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

        if (CameraManager.i.currentCamera != 2)
        {
            canvas.gameObject.SetActive(true);
            vectorText.text = transform.forward.ToString();
            velocityText.text = velocityValue.ToString();
        }
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        directionArrow.SetActive(false);
        canvas.gameObject.SetActive(false);
    }
}

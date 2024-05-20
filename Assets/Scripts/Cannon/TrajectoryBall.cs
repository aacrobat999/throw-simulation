using UnityEngine;

public class TrajectoryBall : MonoBehaviour
{
    [SerializeField] GameObject directionArrow;

    [HideInInspector] public bool isMouseOver = false;

    public void Update()
    {
        /*if (isMouseOver)
            Debug.Log(transform.forward);
        */

        if (!isMouseOver)
            directionArrow.SetActive(TrajectoryPredictor.i.displayAllArrows);
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

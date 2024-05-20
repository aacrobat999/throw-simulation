using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    [SerializeField] TMP_InputField input;

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(UpdateInputField);
    }

    private void UpdateInputField(float value)
    {
        input.text = value.ToString("F2");
    }

    public void ChangeGravityValue(ProjectileThrow projectile)
    {
        input.text = slider.value.ToString("F2");
        projectile._gravityScale = slider.value;
    }

    public void ChangeForceValue(ProjectileThrow projectile)
    {
        input.text = slider.value.ToString("F2");
        projectile.force = slider.value;
    }
    public void ChangePointValue(TrajectoryPredictor predictor)
    {
        input.text = slider.value.ToString("F2");
        predictor.maxPoints = (int)slider.value;
    }
    public void ChangeHiegthValue(ProjectileThrow projectile)
    {
        input.text = slider.value.ToString("F2");
        projectile.transform.position = new Vector3(0, slider.value, 0) + projectile.startingPosition;
    }
}

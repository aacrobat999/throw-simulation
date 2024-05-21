using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] ShotViewCamera shotViewCam;
    [SerializeField] SideViewCamera sideViewCam;
    [SerializeField] GameObject controls;

    public static CameraManager i;

    [HideInInspector] public int currentCamera;

    private void Awake() => i = this;
    private void Start() => shotViewCam.gameObject.SetActive(false);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            StartTransitionToSideView();

        if (Input.GetKeyDown(KeyCode.L))
            StartTransitionBackToMain();

        if (Input.GetKeyDown(KeyCode.Escape))
            controls.SetActive(true);
        else if (Input.GetKeyUp(KeyCode.Escape))
            controls.SetActive(false);
    }

    public void StartTransitionTo()
    {
        sideViewCam.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);

        shotViewCam.gameObject.SetActive(true);
        currentCamera = 2;
    }

    void StartTransitionToSideView()
    {
        shotViewCam.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);


        sideViewCam.gameObject.SetActive(true);
        currentCamera = 3;
    }

    public void StartTransitionBackToMain()
    {
        shotViewCam.gameObject.SetActive(false);
        sideViewCam.gameObject.SetActive(false);


        mainCamera.gameObject.SetActive(true);
        currentCamera = 1;
    }
}

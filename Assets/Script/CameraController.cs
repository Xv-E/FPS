using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 100f;
    public GameObject v_rotate;
    public GameObject h_rotate;
    public Camera camera;

    void Start()
    {
        camera.enabled = this.gameObject == PlayerManager.localPlayer;
    }
    void FixedUpdate()
    {
        if (this.gameObject!=PlayerManager.localPlayer) return;

        float mouseX = Input.GetAxis("Mouse X") * cameraSpeed * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSpeed * Time.fixedDeltaTime;
        v_rotate.transform.localRotation = v_rotate.transform.localRotation * Quaternion.Euler(-mouseY, 0, 0);
        h_rotate.transform.localRotation = h_rotate.transform.localRotation * Quaternion.Euler(0, mouseX, 0);
    }
}

using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float sensitivity = 2f;
    public float minY = -30f;
    public float maxY = 60f;

    private float rotX = 0f;
    private float rotY = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotX = angles.y;
        rotY = angles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotX += mouseX;
        rotY -= mouseY;
        rotY = Mathf.Clamp(rotY, minY, maxY);

        transform.rotation = Quaternion.Euler(rotY, rotX, 0);

        transform.position = target.position;

        Camera.main.transform.position = transform.position + transform.rotation * offset;
        Camera.main.transform.LookAt(target);
    }
}
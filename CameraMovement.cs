using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    public float speed = 3;
    public float minSpeed = 0.01f;
    public float maxSpeed = 10;
    public float mouseScorllerDelta = 0.5f;
    public float mouseMoveSpeed = 0.2f;
    public float shiftMulti = 3f;
    public float controlMulti = 0.5f;

    private Vector3 oldMousePosition;
    private GameObject cameraMountGO, cameraChildGO;

    private Transform cameraMountT, cameraChildT;

    private void Start()
    {
        CreateParents();
    }

    private void CreateParents()
    {
        cameraMountGO = new GameObject("CameraMount");
        cameraChildGO = new GameObject("CameraChild");

        cameraMountT = cameraMountGO.transform; 
        cameraChildT = cameraChildGO.transform;

        cameraChildT.SetParent(cameraMountT);

        cameraMountT.position = transform.position;
        
        cameraMountT.localRotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
        cameraChildT.localRotation = Quaternion.Euler(transform.eulerAngles.x,0,0);

        transform.SetParent(cameraChildT);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        var mouseScroll = Input.mouseScrollDelta.y;
        if (0 != mouseScroll)
        {
            speed += mouseScroll * mouseScorllerDelta;
            if (speed < minSpeed) speed = minSpeed;
            else if (speed >= maxSpeed) speed = maxSpeed;
        }
        
        
        var deltaMouse = (Input.mousePosition - oldMousePosition) * (mouseMoveSpeed * (Time.deltaTime * 60));

        if (Input.GetMouseButton(1))
        {
            cameraMountT.Rotate(0, deltaMouse.x, 0, Space.Self);
            cameraChildT.Rotate(-deltaMouse.y, 0, 0, Space.Self);
        }

        oldMousePosition = Input.mousePosition;

        var move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move.z = speed;
        if (Input.GetKey(KeyCode.S)) move.z = -speed;
        if (Input.GetKey(KeyCode.A)) move.x = -speed;
        if (Input.GetKey(KeyCode.D)) move.x = speed;
        if (Input.GetKey(KeyCode.Q)) move.y = -speed;
        if (Input.GetKey(KeyCode.E)) move.y = speed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) move *= shiftMulti;
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) move *= controlMulti;

        move *= Time.deltaTime * 60;

        var rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(cameraChildT.eulerAngles.x, cameraMountT.eulerAngles.y, 0);
        move = rotation * move;

        cameraMountT.position += move;
    }
}

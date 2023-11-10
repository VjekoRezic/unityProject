using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kontroler : MonoBehaviour
{
    [SerializeField] Transform camera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float walkSpeed = 6.0f;

    float cameraRotation = 0f;
    bool lockCursor = true;
    CharacterController controller = null;
    float velocityY = 0.0f;
    float jumpHeight = 2.0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //zakljuèavanje kursora na sredini zaslona
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application
            QuitGame();
        }
    }
    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraRotation -= mouseDelta.y * mouseSensitivity;
        //ogranièavanje rotacije kamere
        cameraRotation = Mathf.Clamp(cameraRotation, -90.0f, 90.0f);
        //vertikalna rotacija kamere
        camera.localEulerAngles = Vector3.right * cameraRotation;

        //horizontalna rotacija objekta
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }
    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
            if (Input.GetButtonDown("Jump"))
            {
                // Ako je pritisnuta tipka za skakanje, primijeni silu prema gore
                velocityY = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * targetDir.y + transform.right * targetDir.x) * walkSpeed +Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

    }
    void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions m_IA;
    InputAction m_move;
    InputAction m_mousePosition;
    InputAction m_action;
    InputAction m_restart;
    InputAction m_map;
    bool m_mapOpen;

    Rigidbody m_RB;
    [SerializeField] Camera m_camera;
    public GameObject m_mapObject;

    Vector3 m_moveDirection;

    public float m_moveSpeed;

    [SerializeField] Transform m_playerOrientation;
    float m_XRotation, m_YRotation;

    private void Awake()
    {
        m_IA = new PlayerInputActions();
        m_RB = GetComponent<Rigidbody>();

        m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        m_move = m_IA.Player.Move;
        m_move.Enable();

        m_mousePosition = m_IA.Player.LookAt;
        m_mousePosition.Enable();

        m_action = m_IA.Player.Action;
        m_action.Enable();

        m_map = m_IA.Player.Map;
        m_map.Enable();
        
        m_restart = m_IA.Player.Restart;
        m_restart.Enable();
    }

    private void Update()
    {
        Movement();
        Camera();

        Debug.DrawRay(m_camera.transform.position, m_camera.transform.forward * 1, Color.red);

        if (m_action.triggered)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("hit wall");
                }
            }
        }

        if (m_map.triggered)
        {
            if (!m_mapOpen)
            {
                m_mapOpen = true;
                m_mapObject.SetActive(true);
            }
            else
            {
                m_mapOpen = false;
                m_mapObject.SetActive(false);
            }
        }

        if (m_restart.IsPressed())
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        m_RB.MovePosition(transform.position + m_moveSpeed * Time.deltaTime * m_moveDirection);
    }

    public void Movement()
    {
        Vector2 movement = m_move.ReadValue<Vector2>();

        m_moveDirection = m_playerOrientation.forward * movement.y + m_playerOrientation.right * movement.x;
    }

    public void Camera()
    {
        Vector2 mousePosition = m_mousePosition.ReadValue<Vector2>();

        m_YRotation += mousePosition.x * Time.deltaTime * 50;
        m_XRotation -= mousePosition.y * Time.deltaTime * 50;
        m_XRotation = Mathf.Clamp(m_XRotation, -90, 90);

        m_camera.transform.rotation = Quaternion.Euler(m_XRotation, m_YRotation, 0);
        transform.rotation = Quaternion.Euler(0, m_YRotation, 0);
    }

    private void OnDisable()
    {
        m_move.Disable();
        m_mousePosition.Disable();
        m_action.Disable();
        m_map.Disable();
        m_restart.Disable();
    }
}

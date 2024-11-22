using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions m_IA;
    InputAction m_move;

    Rigidbody m_RB;
    [SerializeField] Camera m_camera;

    Vector2 m_moveDirection;

    public float m_moveSpeed;

    private void Awake()
    {
        m_IA = new PlayerInputActions();
        m_RB = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_move = m_IA.Player.Move;
        m_move.Enable();
    }

    private void Update()
    {
        Vector2 moveDirection = m_move.ReadValue<Vector2>();

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, 0);

        transform.rotation = Quaternion.Euler(-mousePosition.y, mousePosition.x, 0);

        Vector3 camForward = m_camera.transform.forward;
        Vector3 camRight = m_camera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        m_moveDirection = camForward * moveDirection.x + camRight * moveDirection.y;







        //if (m_moveDirection.x != 0 || m_moveDirection.y != 0)
        //{
        //    Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, new Vector3(m_moveDirection.x, 0, m_moveDirection.y), 10, 0);
        //    transform.rotation = Quaternion.LookRotation(rotateDirection);
        //}
    }

    private void FixedUpdate()
    {
        m_RB.velocity = new Vector3(m_moveDirection.x * m_moveSpeed, 0, m_moveDirection.y * m_moveSpeed);
    }

    private void OnDisable()
    {
        m_move.Disable();
    }
}

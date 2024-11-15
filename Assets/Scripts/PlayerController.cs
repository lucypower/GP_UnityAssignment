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
    Camera m_camera;

    Vector2 m_moveDirection;

    public float m_moveSpeed;

    private void Awake()
    {
        m_IA = new PlayerInputActions();
        m_RB = GetComponent<Rigidbody>();
        m_camera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        m_move = m_IA.Player.Move;
        m_move.Enable();
    }

    private void Update()
    {
        m_moveDirection = m_move.ReadValue<Vector2>();

        if (m_moveDirection.x != 0 || m_moveDirection.y != 0)
        {
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, new Vector3(m_moveDirection.x, 0, m_moveDirection.y), 10, 0);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }
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

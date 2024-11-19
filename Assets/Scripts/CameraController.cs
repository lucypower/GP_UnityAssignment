using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform m_player;

    Vector3 m_offset = new Vector3(0, 10, 0);

    private void Update()
    {
        transform.position = m_player.position + m_offset;
    }

    public void FindPlayer()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        m_offset = new Vector3(0, 10, -5);
    }
}

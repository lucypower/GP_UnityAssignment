using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform m_player;
    [SerializeField] Transform m_playerCameraPos;

    Vector3 m_offset;

    private void Update()
    {
        if (gameObject.name == "PlayerCamera")
        {
            transform.position = m_playerCameraPos.position;
        }
        else
        {
            transform.position = m_player.position + m_offset;
        }
    }

    public void FindPlayer()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        if (gameObject.name == "PlayerCamera")
        {
            m_playerCameraPos = GameObject.FindGameObjectWithTag("PlayerCamHolder").GetComponent<Transform>();
        }
        else if (gameObject.name == "MiniMap Camera")
        {
            m_offset = new Vector3(0, 10, 0);
        }
    }
}

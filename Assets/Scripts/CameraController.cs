using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform m_player;
    [SerializeField] Transform m_playerCameraPos;

    Vector3 m_offset;

    bool m_playerSpawned;

    private void Update()
    {
        if (m_playerSpawned)
        {
            if (gameObject.name == "Main Camera")
            {
                transform.position = m_playerCameraPos.position;
            }
            else
            {
                transform.position = m_player.position + m_offset;
            }
        }
    }

    public void FindPlayer()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        m_playerSpawned = true;
        
        if (gameObject.name == "Main Camera")
        {
            m_playerCameraPos = GameObject.FindGameObjectWithTag("PlayerCamHolder").GetComponent<Transform>();
        }
        else if (gameObject.name == "MiniMap Camera")
        {
            m_offset = new Vector3(0, 10, 0);
        }
    }
}

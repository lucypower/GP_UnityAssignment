using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager m_gM;
    [SerializeField] TextMeshProUGUI m_pickupsText;

    private void Awake()
    {
        m_gM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        m_pickupsText.text = m_gM.m_pickupsCollected.ToString() + " / " + m_gM.m_pickupsToSpawn.ToString();
    }
}

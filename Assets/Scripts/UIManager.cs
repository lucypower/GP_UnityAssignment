using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    CellularAutomata m_CA;

    GameManager m_gM;
    [SerializeField] TextMeshProUGUI m_pickupsText;

    [SerializeField] TMP_InputField m_widthInput, m_heightInput, m_iterationInput, m_densityInput;
    [SerializeField] Button m_generateButton, m_playButton;

    GameObject m_manager;

    [SerializeField] GameObject m_miniMapUI, m_gridUI;

    private void Awake()
    {
        m_manager = GameObject.Find("Manager");

        m_gM = m_manager.GetComponent<GameManager>();
        m_CA = m_manager.GetComponent<CellularAutomata>();
    }

    private void Update()
    {
        m_pickupsText.text = m_gM.m_pickupsCollected.ToString() + " / " + m_gM.m_pickupsToSpawn.ToString() + " pickups collected!";
    }

    public void UpdateValues()
    {
        var ms = m_manager.GetComponent<MarchingSquares>();
        Destroy(ms.m_walls);
        Destroy(GameObject.Find("Floor(Clone)"));

        m_CA.m_width = int.Parse(m_widthInput.text);
        m_CA.m_height = int.Parse(m_heightInput.text);
        m_CA.m_iterations = int.Parse(m_iterationInput.text);

        float density = float.Parse(m_densityInput.text);

        if (density > 1)
        {
            m_CA.m_density = 1;
        }
        else if (density < 0)
        {
            m_CA.m_density = 0;
        }
        else
        {
            m_CA.m_density = density;
        }

        m_gM.GenerateGrid();
    }

    public void Play()
    {
        m_gM.SpawnPickups();
        m_gM.SpawnPlayer();

        m_miniMapUI.SetActive(true);
        m_gridUI.SetActive(false);
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}

using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public TMP_Text collectiblesNumbersText;

    private int collectiblesNumber;

    public static GameManager Instance;

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para aumentar collectibles
    public void AddCollectible()
    {
        collectiblesNumber++;

        if (collectiblesNumbersText != null)
        {
            collectiblesNumbersText.text = collectiblesNumber.ToString();
        }
    }
}
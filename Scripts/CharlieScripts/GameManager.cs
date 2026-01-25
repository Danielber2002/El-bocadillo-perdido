using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Buscamos el objeto de texto en la nueva escena (Asegúrate de ponerle el Tag o Nombre correcto)
        // Opción A: Buscar por Tag (Recomendado) -> Tienes que crear un Tag "ScoreText" en Unity
        GameObject textObj = GameObject.FindGameObjectWithTag("ScoreText");

        // Opción B: Buscar por nombre (Más fácil ahora mismo) -> Tu objeto de texto debe llamarse "ScoreText"
        // GameObject textObj = GameObject.Find("ScoreText"); 

        if (textObj != null)
        {
            collectiblesNumbersText = textObj.GetComponent<TMP_Text>();
            // 2. Actualizamos el texto visualmente para que no empiece en 0 o vacío
            collectiblesNumbersText.text = collectiblesNumber.ToString();
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
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // referencia al texto de los collectibles
    public TMP_Text collectiblesNumbersText;

    // numero de collectibles recogidos
    private int collectiblesNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // el metodo sirve para aumentar el numero de collectibles
    public void AddCollectible()
    {
        // cuando recoja un collectible le suma 1
        collectiblesNumber++;
        // actualiza el texto en pantalla
        collectiblesNumbersText.text = collectiblesNumber.ToString();
    }
}

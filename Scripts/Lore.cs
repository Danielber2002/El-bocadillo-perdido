using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Lore : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoEntreLetras = 0.05f;
    public string contenidoDelTexto;

    private TextMeshProUGUI miTextoUI;

    void Awake()
    {
        miTextoUI = GetComponent<TextMeshProUGUI>();
    }


    void Start()
    {

        if (string.IsNullOrEmpty(contenidoDelTexto))
        {
            contenidoDelTexto = miTextoUI.text;
        }

        StartCoroutine(EscribirTexto());
    }
    void Update()
    {

        MoreVelocity();
    }

    void MoreVelocity()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tiempoEntreLetras = 0.001f;
        }
    }

    IEnumerator EscribirTexto()
    {
        miTextoUI.text = "";

        foreach (char letra in contenidoDelTexto.ToCharArray())
        {
            miTextoUI.text += letra;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
    }
}

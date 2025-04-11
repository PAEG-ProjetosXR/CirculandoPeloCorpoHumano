using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controla o carregamento assíncrono entre cenas com tela de loading
/// </summary>
public class SceneLoadingMenuGameOver : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Carregamento
    //-----------------------------
    [Header("Configurações de Carregamento")]
    [SerializeField] private RawImage _telaCarregamento;    // Referência para a imagem de loading
    [SerializeField] private float _tempoCarregamento = 2f; // Tempo mínimo de exibição da tela de loading

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Start() 
    {
        IniciarCarregamento();
    }

    //-----------------------------
    // Controle de Carregamento
    //-----------------------------

    /// <summary>
    /// Inicia o processo de carregamento da próxima cena
    /// </summary>
    private void IniciarCarregamento() 
    {
        _telaCarregamento.gameObject.SetActive(true);
        StartCoroutine(CarregarCenaAssincrona(1)); // Carrega a cena 1 (gameplay)
    }

    //-----------------------------
    // Corrotinas
    //-----------------------------

    /// <summary>
    /// Carrega a cena de forma assíncrona com tempo mínimo de loading
    /// </summary>
    /// <param name="cenaDestino">Índice da cena a ser carregada</param>
    private IEnumerator CarregarCenaAssincrona(int cenaDestino) 
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaDestino);
        operacao.allowSceneActivation = false;

        float tempoRestante = _tempoCarregamento;

        while (!operacao.isDone)
        {
            // Quando o carregamento estiver em 90% e o tempo mínimo tiver passado
            if (operacao.progress >= 0.9f && tempoRestante <= 0)
            {
                operacao.allowSceneActivation = true;
            }

            tempoRestante -= Time.deltaTime;
            yield return null;
        }

        _telaCarregamento.gameObject.SetActive(false);
    }
}
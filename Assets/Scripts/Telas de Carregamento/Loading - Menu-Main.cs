using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controla a transição entre o menu principal e a cena do jogo
/// com tela de carregamento personalizada
/// </summary>
public class SceneLoadingMenu : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Carregamento
    //-----------------------------
    [Header("Configurações de Loading")]
    [SerializeField] private RawImage _telaLoading;        // Componente da tela de carregamento
    [SerializeField] private float _duracaoLoading = 2f;   // Duração mínima da tela de loading

    //-----------------------------
    // Constantes
    //-----------------------------
    private const int CENA_JOGO = 1; // Índice da cena principal do jogo

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Start() 
    {
        IniciarCarregamentoJogo();
    }

    //-----------------------------
    // Controle de Carregamento
    //-----------------------------

    /// <summary>
    /// Inicia o processo de transição para a cena do jogo
    /// </summary>
    private void IniciarCarregamentoJogo() 
    {
        _telaLoading.gameObject.SetActive(true);
        StartCoroutine(CarregarCenaJogo());
    }

    //-----------------------------
    // Corrotinas
    //-----------------------------

    /// <summary>
    /// Gerencia o carregamento assíncrono da cena do jogo
    /// </summary>
    private IEnumerator CarregarCenaJogo() 
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(CENA_JOGO);
        operacao.allowSceneActivation = false;

        float tempoRestante = _duracaoLoading;

        while (!operacao.isDone)
        {
            if (operacao.progress >= 0.9f && tempoRestante <= 0)
            {
                operacao.allowSceneActivation = true;
            }

            tempoRestante -= Time.deltaTime;
            yield return null;
        }

        _telaLoading.gameObject.SetActive(false);
    }
}
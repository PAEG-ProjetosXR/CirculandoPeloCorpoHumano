using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controla a transição entre a cena principal e a tela de Game Over
/// com tela de carregamento personalizada
/// </summary>
public class SceneLoadingGameOver : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Carregamento
    //-----------------------------
    [Header("Configurações de Carregamento")]
    [SerializeField] private RawImage _telaCarregamento;    // Referência para a RawImage de loading
    [SerializeField] private float _tempoMinimoLoading = 2f; // Tempo mínimo de exibição do loading

    //-----------------------------
    // Constantes
    //-----------------------------
    private const int CENA_GAMEOVER = 3; // Índice da cena de Game Over

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Start() 
    {
        IniciarProcessoCarregamento();
    }

    //-----------------------------
    // Controle de Carregamento
    //-----------------------------

    /// <summary>
    /// Inicia a sequência de carregamento para a cena de Game Over
    /// </summary>
    private void IniciarProcessoCarregamento() 
    {
        _telaCarregamento.gameObject.SetActive(true);
        StartCoroutine(CarregarCenaGameOver());
    }

    //-----------------------------
    // Corrotinas
    //-----------------------------

    /// <summary>
    /// Gerencia o carregamento assíncrono com tempo mínimo garantido
    /// </summary>
    private IEnumerator CarregarCenaGameOver() 
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(CENA_GAMEOVER);
        operacao.allowSceneActivation = false;

        float tempoRestante = _tempoMinimoLoading;

        while (!operacao.isDone)
        {
            // Quando o carregamento estiver em 90% + tempo mínimo atingido
            if (operacao.progress >= 0.9f && tempoRestante <= 0)
            {
                operacao.allowSceneActivation = true;
            }

            tempoRestante -= Time.deltaTime;
            yield return null;
        }

        // Opcional: Adicionar fade out antes de desativar
        _telaCarregamento.gameObject.SetActive(false);
    }
}
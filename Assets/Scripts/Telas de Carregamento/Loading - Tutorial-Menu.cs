using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controla a transição entre a cena de tutorial e o menu principal
/// com tela de carregamento personalizada
/// </summary>
public class LoadingTutorialMenu : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Carregamento
    //-----------------------------
    [Header("Configurações de Loading")]
    [SerializeField] private RawImage _telaCarregamento;   // Referência para a imagem de loading
    [SerializeField] private float _tempoMinimo = 2f;      // Tempo mínimo de exibição do loading

    //-----------------------------
    // Constantes
    //-----------------------------
    private const int CENA_MENU = 1; // Índice da cena do menu principal

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Start() 
    {
        IniciarCarregamentoMenu();
    }

    //-----------------------------
    // Controle de Carregamento
    //-----------------------------

    /// <summary>
    /// Inicia o processo de transição para o menu principal
    /// </summary>
    private void IniciarCarregamentoMenu() 
    {
        _telaCarregamento.gameObject.SetActive(true);
        StartCoroutine(CarregarCenaMenu());
    }

    //-----------------------------
    // Corrotinas
    //-----------------------------

    /// <summary>
    /// Gerencia o carregamento assíncrono para o menu principal
    /// </summary>
    private IEnumerator CarregarCenaMenu() 
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(CENA_MENU);
        operacao.allowSceneActivation = false;

        float tempoRestante = _tempoMinimo;

        while (!operacao.isDone)
        {
            if (operacao.progress >= 0.9f && tempoRestante <= 0)
            {
                operacao.allowSceneActivation = true;
            }

            tempoRestante -= Time.deltaTime;
            yield return null;
        }

        _telaCarregamento.gameObject.SetActive(false);
    }

    //-----------------------------
    // Elementos Opcionais (Região para implementação futura)
    //-----------------------------
    /*
    [Header("Feedback Visual")]
    [SerializeField] private Slider _barraProgresso;
    [SerializeField] private TextMeshProUGUI _textoStatus;

    private void AtualizarUI(float progresso)
    {
        _barraProgresso.value = progresso;
        _textoStatus.text = $"Carregando: {Mathf.Round(progresso * 100)}%";
    }
    */
}
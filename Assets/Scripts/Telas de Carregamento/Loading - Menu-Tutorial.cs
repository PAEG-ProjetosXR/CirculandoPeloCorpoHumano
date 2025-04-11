using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controla a transição entre o menu principal e a cena de tutorial
/// com tela de carregamento personalizada
/// </summary>
public class LoadingMenuTutorial : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Carregamento
    //-----------------------------
    [Header("Configurações de Loading")]
    [SerializeField] private RawImage _telaLoading;        // Componente da tela de carregamento
    [SerializeField] private float _duracaoMinima = 2f;   // Tempo mínimo de exibição do loading

    //-----------------------------
    // Constantes
    //-----------------------------
    private const int CENA_TUTORIAL = 9; // Índice da cena de tutorial

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Start() 
    {
        IniciarTransicaoTutorial();
    }

    //-----------------------------
    // Controle de Carregamento
    //-----------------------------

    /// <summary>
    /// Inicia o processo de transição para a cena de tutorial
    /// </summary>
    private void IniciarTransicaoTutorial() 
    {
        _telaLoading.gameObject.SetActive(true);
        StartCoroutine(CarregarCenaTutorial());
    }

    //-----------------------------
    // Corrotinas
    //-----------------------------

    /// <summary>
    /// Gerencia o carregamento assíncrono da cena de tutorial
    /// </summary>
    private IEnumerator CarregarCenaTutorial() 
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(CENA_TUTORIAL);
        operacao.allowSceneActivation = false;

        float tempoRestante = _duracaoMinima;

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

    //-----------------------------
    // Métodos Opcionais (para implementação futura)
    //-----------------------------

    /*
    [Header("Elementos Adicionais")]
    [SerializeField] private Slider _barraProgresso;
    [SerializeField] private TextMeshProUGUI _textoDica;
    
    private void AtualizarUI(float progresso)
    {
        if (_barraProgresso != null)
            _barraProgresso.value = progresso;
    }
    */
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia a tela de Game Over, exibindo pontuação e tempo de jogo,
/// além de controlar a transição de volta ao menu principal.
/// </summary>
public class SceneGameOverManager : MonoBehaviour 
{
    //-----------------------------
    // Referências UI (Configuradas no Inspector)
    //-----------------------------
    [SerializeField] private TextMeshProUGUI _timerGameText;  // Exibe o tempo total de jogo
    [SerializeField] private TextMeshProUGUI _scoreGameText;  // Exibe a pontuação final

    //-----------------------------
    // Métodos Unity
    //-----------------------------

    /// <summary>
    /// Ao iniciar, busca os dados do GameManager e atualiza a UI.
    /// </summary>
    private void Start()
    {
        UpdateGameOverUI();
    }

    //-----------------------------
    // Lógica de UI
    //-----------------------------

    /// <summary>
    /// Atualiza os textos de tempo e pontuação com dados do GameManager.
    /// Logs erros se componentes essenciais não forem encontrados.
    /// </summary>
    private void UpdateGameOverUI() 
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager não encontrado!");
            return;
        }

        int totalScore = GameManager.Instance.GetPontos();
        _scoreGameText.text = $"PONTOS: {totalScore}";

        if (_timerGameText != null) 
    {
            float totalTimeSpent = GameManager.Instance.GetTotalTime();
            _timerGameText.text = $"TEMPO: {Mathf.FloorToInt(totalTimeSpent)}";
        }
        else 
        {
            Debug.LogError("TimerGameText não atribuído no Inspector!");
        }
    }

    //-----------------------------
    // Navegação de Cenas
    //-----------------------------

    /// <summary>
    /// Inicia a transição de volta ao menu principal.
    /// </summary>
    public void ReturnToMenu() 
    {
        StartCoroutine(LoadMenuAfterDelay());
    }

    /// <summary>
    /// Carrega uma cena intermediária (ID:7) antes de retornar ao menu (ID:0).
    /// </summary>
    private IEnumerator LoadMenuAfterDelay() 
    {
        SceneManager.LoadScene(7);  // Cena de transição/loading
        yield return new WaitForSeconds(3f);  // Tempo fixo para demonstração
        SceneManager.LoadScene(0);  // Menu principal
    }
}
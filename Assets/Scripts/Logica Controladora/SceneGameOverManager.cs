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
    [SerializeField] private TextMeshProUGUI _result1Text;    // Exibe o resultado do 1º colocado
    [SerializeField] private TextMeshProUGUI _result2Text;    // Exibe o resultado do 2º colocado
    [SerializeField] private TextMeshProUGUI _result3Text;    // Exibe o resultado do 3º colocado
    [SerializeField] private TextMeshProUGUI _result4Text;    // Exibe o resultado do 4º colocado
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
        float totalTimeSpent = GameManager.Instance.GetTotalTime();

        // Atualiza os resultados dos jogadores
        if (_result1Text != null) 
        {
            //Aqui os jogadores ainda estão com nomes genéricos
            _result1Text.text = $"1º: Jogador 1 / Tempo: {Mathf.FloorToInt(totalTimeSpent)} / Pontuação: {totalScore}";
            _result2Text.text = $"2º: Jogador 2 / Tempo: {GetRandomTime()} / Pontuação: {GetRandomScore(totalScore)}";
            _result3Text.text = $"3º: Jogador 3 / Tempo: {GetRandomTime()} / Pontuação: {GetRandomScore(totalScore)}";
            _result4Text.text = $"4º: Jogador 4 / Tempo: {GetRandomTime()} / Pontuação: {GetRandomScore(totalScore)}";
        }
        else 
        {
            Debug.LogError("Result1Text não atribuído no Inspector!");
        }

        _scoreGameText.text = $"PONTOS: {totalScore}";

        if (_timerGameText != null) 
        {
            _timerGameText.text = $"TEMPO: {Mathf.FloorToInt(totalTimeSpent)}";
        }
        else 
        {
            Debug.LogError("TimerGameText não atribuído no Inspector!");
        }
    }

    private int GetRandomTime() 
    {
        return 600 + Random.Range(0, 1320);  // Simula um tempo aleatório
    }

    private int GetRandomScore(int totalScore) 
    {
        if (totalScore < 10) //Jogador não acertou nenhuma questão
        {
            return 0;
        }
        return (Random.Range(0, totalScore - 10) / 10) * 10;  // Simula uma pontuação aleatória
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
    /// Carrega uma cena intermediária (ID:6) antes de retornar ao menu (ID:0).
    /// </summary>
    private IEnumerator LoadMenuAfterDelay() 
    {
        SceneManager.LoadScene(6);  // Cena de transição/loading
        yield return new WaitForSeconds(3f);  // Tempo fixo para demonstração
        SceneManager.LoadScene(0);  // Menu principal
    }
}
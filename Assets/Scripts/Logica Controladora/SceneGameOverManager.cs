using System.Collections;
using System.Collections.Generic;
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
    // Classes auxiliares
    //-----------------------------
    public class Resultado
    {
        public string playerName;
        public int playerScore;
        public int playerTime;
    }

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
            List<Resultado> resultados = new List<Resultado>();
            GetRandomResults(totalScore, resultados);
            int i = 0;
            //Aqui os jogadores ainda estão com nomes genéricos
            _result1Text.text = $"1º: Jogador 1 / Tempo: {Mathf.FloorToInt(totalTimeSpent)} / Pontuação: {totalScore}";
            _result2Text.text = $"2º: {resultados[0].playerName} / Tempo: {resultados[0].playerTime} / Pontuação: {resultados[0].playerScore}";
            _result3Text.text = $"3º: {resultados[1].playerName} / Tempo: {resultados[1].playerTime} / Pontuação: {resultados[1].playerScore}";
            _result4Text.text = $"4º: {resultados[2].playerName} / Tempo: {resultados[2].playerTime} / Pontuação: {resultados[2].playerScore}";
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

    //Método para obter resultados aleatórios para os jogadores "genericos"
    private void GetRandomResults(int totalScore, List<Resultado> resultados)
    {
        Resultado resultado1 = new Resultado();
        Resultado resultado2 = new Resultado();
        Resultado resultado3 = new Resultado();

        resultado1.playerName = "Jogador 2";
        resultado1.playerScore = GetRandomScore(totalScore);
        resultado1.playerTime = GetRandomTime();
        resultado2.playerName = "Jogador 3";
        resultado2.playerScore = GetRandomScore(totalScore);
        resultado2.playerTime = GetRandomTime();
        resultado3.playerName = "Jogador 4";
        resultado3.playerScore = GetRandomScore(totalScore);
        resultado3.playerTime = GetRandomTime();

        // Adiciona os resultados à lista de resultados
        resultados.Add(resultado1);
        resultados.Add(resultado2);
        resultados.Add(resultado3);

        // Ordena a lista de resultados com base na pontuação, com desempate baseado no tempo
        OrderRandomResults(resultados);
    }

    //Método para ordenar os resultados aleatórios gerados em GetRandomResults
    private void OrderRandomResults(List<Resultado> resultados)
    {
        resultados.Sort((a, b) =>
        {
            int comparison = b.playerScore.CompareTo(a.playerScore);    //Ordena em ordem decrescente de pontuação
            if (comparison != 0)
                return comparison;
            return a.playerTime.CompareTo(b.playerTime);    //Em caso de empate, ordena em ordem crescente de tempo
        });
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
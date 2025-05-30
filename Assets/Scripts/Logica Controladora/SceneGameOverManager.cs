using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class Resultado
{
  private string _nome;
  private int _pontos;
  private int _tempo;
  public string Nome
  {
    get { return _nome; }
    set { Nome = value; }
  }
  public int Pontos
  {
    get { return _pontos; }
    set { Pontos = value; }
  }
  public int Tempo
  {
    get { return _tempo; }
    set { Tempo = value; }
  }
}

class ObjetoPontuacao
{
  public TextMeshProUGUI textStatus;
}

public class SceneGameOverManager : MonoBehaviour
{
  //-----------------------------
  // Referências UI (Configuradas no Inspector)
  //-----------------------------
  [SerializeField] private TextMeshProUGUI _timerGameText;  // Exibe o tempo total de jogo
  [SerializeField] private TextMeshProUGUI _scoreGameText;  // Exibe a pontuação final
  // [SerializeField] private List<Resultado> _resultados;
  [SerializeField] private IntegerScriptableObject _pontosSO;

  private void Start()
  {
    UpdateGameOverUI();
  }

  private void UpdateGameOverUI()
  {
    if (GameManager.Instance == null)
    {
      Debug.LogError("GameManager não encontrado!");
      return;
    }

    int totalScore = GameManager.Instance.GetPontos();
    float totalTimeSpent = GameManager.Instance.GetTotalTime();

    // foreach (Resultado resultado in _resultados)
    // {
    //   // mostrar _resultados
    // }
  }

  public void AtualizarResultados()
  {
    // utilizar timestamp, nome da equipe e nome do jogador para atualizar seus resultados
    // com os assets de tempo e pontos 
  }

  public void ObterResultados()
  {

  }

  public void ReturnToMenu()
  {
    StartCoroutine(LoadMenuAfterDelay());
  }

  private IEnumerator LoadMenuAfterDelay()
  {
    SceneManager.LoadScene(6);  // Cena de transição/loading
    yield return new WaitForSeconds(3f);  // Tempo fixo para demonstração
    SceneManager.LoadScene(0);  // Menu principal
  }
}
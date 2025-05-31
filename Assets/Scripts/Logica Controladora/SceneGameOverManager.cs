using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
  [SerializeField] private TextMeshProUGUI _timerGameText;
  [SerializeField] private TextMeshProUGUI _scoreGameText;
  [SerializeField] private List<TextMeshProUGUI> _textsPontuacoes;

  [SerializeField] private IntegerScriptableObject _pontosSO;
  [SerializeField] private FloatScriptableObject _tempoSO;
  [SerializeField] private StringScriptableObject _collectionNameSO;
  [SerializeField] private StringArrayScriptableObject _documentsSO;

  private SetGameData setGameData;

  private void Start()
  {
    setGameData = new SetGameData();
    UpdateGameOverUI();
    AtualizarResultados();
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

    foreach (TextMeshProUGUI pontuacao in _textsPontuacoes)
    {

    }
  }

  public void AtualizarResultados()
  {
    string _gamePath;
    GameData _gameData;
    for (int i = 0; i < _documentsSO.Value.Length; i++)
    {
      _gamePath = $"{_collectionNameSO}/{_documentsSO.Value[i]}";
      _gameData = new GameData
      {
        Nomes = new string[0],
        Equipe = "",
        Pontos = _pontosSO.Value,
        Tempo = (int)_tempoSO.Value
      };
      setGameData.HandleUpdate(_gamePath, _gameData);
    }
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
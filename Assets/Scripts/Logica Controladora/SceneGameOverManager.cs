using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Firebase.Firestore;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System;

class Resultado
{
  private string _nomeEquipe;
  private int _pontos;
  private int _tempo;
  public string NomeEquipe
  {
    get { return _nomeEquipe; }
    set { _nomeEquipe = value; }
  }
  public int Pontos
  {
    get { return _pontos; }
    set { _pontos = value; }
  }
  public int Tempo
  {
    get { return _tempo; }
    set { _tempo = value; }
  }
}

[Serializable]
public class Pontuacao
{
  public TextMeshProUGUI textNome;
  public TextMeshProUGUI textPontosTempo;
}

public class SceneGameOverManager : MonoBehaviour
{
  [SerializeField] private List<Pontuacao> _pontuacoes;
  [SerializeField] private Pontuacao _pontuacaoLocal;

  [SerializeField] private IntegerScriptableObject _pontosSO;
  [SerializeField] private FloatScriptableObject _tempoSO;
  [SerializeField] private StringScriptableObject _collectionNameSO;
  [SerializeField] private StringArrayScriptableObject _documentsSO;

  private SetGameData setGameData;

  private void Start()
  {
    setGameData = new SetGameData();
    AtualizarResultados();
    UpdateGameOverUI();
  }

  private async void UpdateGameOverUI()
  {
    Dictionary<int, Resultado> _resultados = await ObterResultados();
    int[] _arrayPontosPorTempo = _resultados.Keys.ToArray();
    Array.Sort(_arrayPontosPorTempo);
    Array.Reverse(_arrayPontosPorTempo);
    int _contadorDeColocados = 0;

    GameData _resultadoLocal = await setGameData.LoadFromCloud($"{_collectionNameSO.Value}/{_documentsSO.Value[0]}");
    _pontuacaoLocal.textNome.text = _resultadoLocal.Equipe.Equals("")
      ? _resultadoLocal.Nomes[0]
      : _resultadoLocal.Equipe;
    _pontuacaoLocal.textPontosTempo.text = $"{_resultadoLocal.Pontos}/{_resultadoLocal.Tempo}";

    Debug.Log("Equipe: " + _pontuacaoLocal.textNome.text + " \nPontos: " + _pontuacaoLocal.textPontosTempo.text);

    foreach (Pontuacao pontuacao in _pontuacoes)
    {
      if (_contadorDeColocados < _arrayPontosPorTempo.Length)
      {
        Resultado _resultadoAtual = _resultados[_arrayPontosPorTempo[_contadorDeColocados]];
        pontuacao.textNome.text = _resultadoAtual.NomeEquipe;
        pontuacao.textPontosTempo.text = $"{_resultadoAtual.Pontos}/{_resultadoAtual.Tempo}";
        _contadorDeColocados++;

        Debug.Log("Equipe: " + pontuacao.textNome.text + " \nPontos: " + pontuacao.textPontosTempo.text);
      }
    }
  }

  public void AtualizarResultados()
  {
    string _gamePath;
    GameData _gameData;
    for (int i = 0; i < _documentsSO.Value.Length; i++)
    {
      _gamePath = $"{_collectionNameSO.Value}/{_documentsSO.Value[i]}";
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

  private async Task<Dictionary<int, Resultado>> ObterResultados()
  {
    Dictionary<int, Resultado> _resultados = new Dictionary<int, Resultado>();
    GameData _documentoAtual;
    var _documents = await setGameData.LoadDocumentsFromCollectionFromCloud(_collectionNameSO.Value);
    foreach (DocumentSnapshot documentSnapshot in _documents)
    {
      _documentoAtual = documentSnapshot.ConvertTo<GameData>();
      int _pontosPorTempo = _documentoAtual.Tempo == 0 ? 0 : _documentoAtual.Pontos / _documentoAtual.Tempo;
      if (!_resultados.ContainsKey(_pontosPorTempo))
        _resultados.Add(
          _pontosPorTempo,
          new Resultado
          {
            NomeEquipe = _documentoAtual.Equipe.Equals("") ? _documentoAtual.Nomes[0] : _documentoAtual.Equipe,
            Pontos = _documentoAtual.Pontos,
            Tempo = _documentoAtual.Tempo
          });
    }
    return _resultados;
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
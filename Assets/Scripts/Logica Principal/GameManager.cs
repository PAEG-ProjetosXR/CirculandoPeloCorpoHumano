using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;

  private string _statusGame;
  private float _tempo;
  private float _tempoPorQuestao;
  private int _indiceQuestaoAtual;
  private int _totalQuestoes;
  private bool _targetIdentificado;
  private bool _questaoMultiplaEscolha;
  private bool _botoesHabilitados;
  private bool _imageBoxQuestaoImageTargetHabilitada = false;
  private int _paginaAtualQuestaoImageTarget;
  private int _totalQuestoesPorSecao;
  private int _quantidadeQuestoesMultiplaEscolhaPorSecao;

  private int[] _pontosPorQuestao;

  [Header("Configurações de UI")]
  [SerializeField] private TextMeshProUGUI _textQuestaoMultiplaEscolha;
  [SerializeField] private TextMeshProUGUI _textQuestaoImageTarget;
  [SerializeField] private TextMeshProUGUI _textPaginaAtualQuestaoImageTarget;
  [SerializeField] private TextMeshProUGUI _textPontos;
  [SerializeField] private TextMeshProUGUI _textTempo;
  [SerializeField] private GameObject _imageBoxQuestaoMultiplaEscolha;
  [SerializeField] private GameObject _imageBoxQuestaoImageTarget;
  [SerializeField] private GameObject _buttonToggleQuestaoImageTarget;
  [SerializeField] private IntegerScriptableObject _pontosSO;
  [SerializeField] private FloatScriptableObject _tempoSO;
  private GameObject _telaCarregamento;
  private TextMeshProUGUI _textoCarregamento;
  [SerializeField] private GameObject _fundo;

  [Header("Configurações de Botões")]
  [SerializeField] private GameObject[] _botoes;
  [SerializeField] private Color _corAcerto = Color.green;
  [SerializeField] private Color _corErro = Color.red;

  [Header("Configurações de Áudio")]
  [SerializeField] private AudioSource _audioSource;
  [SerializeField] private AudioClip _somAcerto;
  [SerializeField] private AudioClip _somErro;

  [Header("Banco de Questões")]
  [SerializeField] private List<QuestaoImageTarget> _questoesImageTarget;
  [SerializeField] private List<QuestaoMultiplaEscolha> _questoesMultiplaEscolha;
  [SerializeField] private List<GameObject> _imageTargets;
  private GameObject _targetAtual;

  private List<List<int>> _indicesRandomizados;

  private IEnumerator _corrotinaTempo;

  [System.Serializable]
  public class QuestaoMultiplaEscolha
  {
    public string pergunta;
    public string[] alternativas;
    public int indiceRespostaCorreta;
  }

  [System.Serializable]
  public class QuestaoImageTarget
  {
    public string[] perguntaFracionada;
  }

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  private void Start()
  {
    _paginaAtualQuestaoImageTarget = 0;
    _tempoPorQuestao = 4f;
    IniciarJogo();
    ResetarCoresBotoes();
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += AoCarregarCena;
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= AoCarregarCena;
  }


  private void AoCarregarCena(Scene cena, LoadSceneMode modo)
  {
    if (cena.buildIndex == 1)
    {
      IniciarJogo();
    }
  }


  private void IniciarJogo()
  {
    _quantidadeQuestoesMultiplaEscolhaPorSecao = _questoesMultiplaEscolha.Count / _questoesImageTarget.Count;
    _totalQuestoesPorSecao = (_quantidadeQuestoesMultiplaEscolhaPorSecao + 1);
    ResetarJogo();
    RandomizarQuestoesPorSecao();
    MostrarProximaQuestao();
  }

  private void ResetarJogo()
  {
    _pontosSO.Value = 0;
    _indiceQuestaoAtual = -1;
    _totalQuestoes = _questoesImageTarget.Count + _questoesMultiplaEscolha.Count;
    _tempo = _tempoPorQuestao;
    _tempoSO.Value = 0f;
    _statusGame = "Play";
    _targetIdentificado = false;
    _questaoMultiplaEscolha = false;
    _botoesHabilitados = true;

    _pontosPorQuestao = new int[_questoesImageTarget.Count];
    for (int i = 0; i < _pontosPorQuestao.Length; i++)
    {
      _pontosPorQuestao[i] = 0;
    }

    AtualizarHUD();
  }

  private void RandomizarQuestoesPorSecao()
  {
    _indicesRandomizados = new List<List<int>>();

    for (int secao = 0; secao < _questoesMultiplaEscolha.Count / _quantidadeQuestoesMultiplaEscolhaPorSecao; secao++)
    {
      List<int> indicesSecao = new List<int>();
      for (int i = 0; i < _quantidadeQuestoesMultiplaEscolhaPorSecao; i++)
      {
        indicesSecao.Add(secao * _quantidadeQuestoesMultiplaEscolhaPorSecao + i);
      }

      for (int i = 0; i < indicesSecao.Count; i++)
      {
        int indiceRandomico = Random.Range(i, indicesSecao.Count);
        int temp = indicesSecao[i];
        indicesSecao[i] = indicesSecao[indiceRandomico];
        indicesSecao[indiceRandomico] = temp;
      }

      _indicesRandomizados.Add(indicesSecao);
    }
  }

  public void MostrarProximaQuestao()
  {
    if (_indiceQuestaoAtual >= 0 && _indiceQuestaoAtual < _totalQuestoes)
    {
      float tempoGasto = _tempoPorQuestao - _tempo;
      if (tempoGasto > 0 && tempoGasto <= _tempoPorQuestao)
      {
        _tempoSO.Value += tempoGasto;
        Debug.Log($"Tempo gasto na questão {_indiceQuestaoAtual}: {tempoGasto} segundos");
      }
    }

    if (_indiceQuestaoAtual < _totalQuestoes - 1)
    {
      _indiceQuestaoAtual++;
      _questaoMultiplaEscolha = _indiceQuestaoAtual % _totalQuestoesPorSecao != 0;

      if (_questaoMultiplaEscolha)
      {
        ConfigurarQuestaoMultiplaEscolha();
      }
      else
      {
        ConfigurarQuestaoImageTarget();
      }
    }
    else
    {
      FinalizarJogo();
      return;
    }

    _tempo = _tempoPorQuestao;
    _targetIdentificado = false;
    IniciarContagemRegressiva();
    AtualizarHUD();
  }

  private void ConfigurarQuestaoMultiplaEscolha()
  {
    int indiceSecao = _indiceQuestaoAtual / _totalQuestoesPorSecao;
    int indiceQuestaoNaSecao = (_indiceQuestaoAtual % _totalQuestoesPorSecao) - 1;

    if (indiceSecao < _indicesRandomizados.Count && indiceQuestaoNaSecao < _quantidadeQuestoesMultiplaEscolhaPorSecao)
    {
      int indiceQuestao = _indicesRandomizados[indiceSecao][indiceQuestaoNaSecao];

      if (indiceQuestao < _questoesMultiplaEscolha.Count)
      {
        if (_fundo != null) _fundo.SetActive(true);

        _textQuestaoMultiplaEscolha.text = _questoesMultiplaEscolha[indiceQuestao].pergunta;
        MostrarBotoesQuestaoMultiplaEscolha();
        MostrarPerguntaMultiplaEscolha();
        EsconderBotaoToggleQuestaoImageTarget();
        EsconderPerguntaImageTarget();
        AtualizarBotoesMultiplaEscolha(indiceQuestao);
        _botoesHabilitados = true;
      }
    }
  }

  private void ConfigurarQuestaoImageTarget()
  {
    if (_fundo != null) _fundo.SetActive(false);

    EsconderBotoesQuestaoMultiplaEscolha();
    EsconderPerguntaMultiplaEscolha();
    MostrarBotaoToggleQuestaoImageTarget();
    if (_targetAtual != null) Destroy(_targetAtual);
    _targetAtual = Instantiate(_imageTargets[_indiceQuestaoAtual / _totalQuestoesPorSecao], Vector3.zero, Quaternion.identity);
  }

  private void FinalizarJogo()
  {
    _statusGame = "GameOver";
    CarregarCenaComTelaCarregamento(5); // Cena de Game Over
  }

  public void TargetIdentificado()
  {
    if (!_targetIdentificado && !_questaoMultiplaEscolha)
    {
      _targetIdentificado = true;
      PararContagemRegressiva();

      if (_audioSource != null) _audioSource.Play();

      int questaoAtual = _indiceQuestaoAtual / _totalQuestoesPorSecao;
      if (_pontosPorQuestao[questaoAtual] < 10)
      {
        _pontosPorQuestao[questaoAtual] += 10;
        _pontosSO.Value += 10;
        AtualizarHUD();
        Debug.Log($"Pontos ganhos para a questão {questaoAtual}: {_pontosPorQuestao[questaoAtual]}");
      }
      else
      {
        Debug.Log("Pontos máximos já alcançados para essa questão.");
      }

      StartCoroutine(EsperarEProximaQuestao(3f));
    }
  }

  public void VerificarResposta(int indiceBotao)
  {
    if (_questaoMultiplaEscolha && _botoesHabilitados)
    {
      _botoesHabilitados = false;
      PararContagemRegressiva();

      int indiceSecao = _indiceQuestaoAtual / _totalQuestoesPorSecao;
      int indiceQuestaoNaSecao = _indiceQuestaoAtual % _totalQuestoesPorSecao - 1;

      if (indiceSecao < _indicesRandomizados.Count && indiceQuestaoNaSecao < _quantidadeQuestoesMultiplaEscolhaPorSecao)
      {
        int indiceQuestao = _indicesRandomizados[indiceSecao][indiceQuestaoNaSecao];

        if (indiceQuestao < _questoesMultiplaEscolha.Count)
        {
          if (indiceBotao == _questoesMultiplaEscolha[indiceQuestao].indiceRespostaCorreta)
          {
            AumentarPontuacao(10);
            TocarSom(_somAcerto);
          }
          else
          {
            TocarSom(_somErro);
          }

          AtualizarCoresBotoes(_questoesMultiplaEscolha[indiceQuestao].indiceRespostaCorreta);
          StartCoroutine(EsperarEProximaQuestao(2f));
        }
      }
    }
  }

  private void AtualizarBotoesMultiplaEscolha(int indiceQuestao)
  {
    if (indiceQuestao < _questoesMultiplaEscolha.Count)
    {
      for (int i = 0; i < _botoes.Length; i++)
      {
        if (i < _questoesMultiplaEscolha[indiceQuestao].alternativas.Length)
        {
          TextMeshProUGUI textoBotao = _botoes[i].GetComponentInChildren<TextMeshProUGUI>();
          if (textoBotao != null) textoBotao.text = _questoesMultiplaEscolha[indiceQuestao].alternativas[i];
        }
      }
    }
  }

  private IEnumerator EsperarEProximaQuestao(float delay)
  {
    yield return new WaitForSeconds(delay);
    ResetarCoresBotoes();
    MostrarProximaQuestao();
  }

  private void TocarSom(AudioClip clip)
  {
    if (_audioSource != null && clip != null)
    {
      Debug.Log($"Reproduzindo som: {clip.name}");
      _audioSource.PlayOneShot(clip);
    }
    else
    {
      Debug.LogWarning("AudioSource ou AudioClip não configurado!");
    }
  }

  private void AtualizarCoresBotoes(int indiceBotaoCorreto)
  {
    for (int i = 0; i < _botoes.Length; i++)
    {
      _botoes[i].GetComponent<Image>().color =
          (i == indiceBotaoCorreto) ? _corAcerto : _corErro;
    }
  }

  private void ResetarCoresBotoes()
  {
    foreach (var botao in _botoes)
    {
      botao.GetComponent<Image>().color = Color.white;
    }
  }

  public void AumentarPontuacao(int valor)
  {
    _pontosSO.Value += valor;
    AtualizarHUD();
  }

  private void AtualizarHUD()
  {
    if (_textPontos != null)
      _textPontos.text = $"PONTOS: {_pontosSO.Value}";

    if (_textTempo != null)
      _textTempo.text = $"TEMPO: {Mathf.CeilToInt(_tempo)}";
  }

  private void IniciarContagemRegressiva()
  {
    PararContagemRegressiva();
    _corrotinaTempo = ContadorTempo();
    StartCoroutine(_corrotinaTempo);
  }

  private void PararContagemRegressiva()
  {
    if (_corrotinaTempo != null)
    {
      StopCoroutine(_corrotinaTempo);
      _corrotinaTempo = null;
    }
  }

  public void PararContagemRegressivaJogo()
  {
    PararContagemRegressiva();
  }

  private IEnumerator ContadorTempo()
  {
    while (_tempo > 0 && _statusGame == "Play" && !_targetIdentificado)
    {
      yield return new WaitForSeconds(1f);
      _tempo -= 1f;
      AtualizarHUD();
    }

    if (_tempo <= 0)
    {
      Debug.Log("Tempo esgotado! Avançando para a próxima questão.");
      StartCoroutine(EsperarEProximaQuestao(1f));
    }
  }

  public void CarregarCenaComTelaCarregamento(int indiceCena)
  {
    StartCoroutine(CarregarCenaAssincrona(indiceCena));
  }

  private IEnumerator CarregarCenaAssincrona(int indiceCena)
  {
    if (_telaCarregamento != null)
    {
      _telaCarregamento.SetActive(true);
      _textoCarregamento.text = "Carregando...";
    }

    //yield return new WaitForSeconds(0.1f);

    AsyncOperation carregamento = SceneManager.LoadSceneAsync(indiceCena);
    while (!carregamento.isDone)
    {
      yield return null;
    }

    if (_telaCarregamento != null)
    {
      _telaCarregamento.SetActive(false);
    }
  }

  public void SalvarJogo()
  {
    PlayerPrefs.SetInt("Pontos", _pontosSO.Value);
    PlayerPrefs.SetFloat("TotalTimeSpent", _tempoSO.Value);
    PlayerPrefs.Save();
    Debug.Log("Jogo salvo!");
  }

  public float GetTempoTotal()
  {
    return _tempoSO.Value;
  }

  public int GetPontos()
  {
    return _pontosSO.Value;
  }

  public bool IsTargetIdentificado()
  {
    return _targetIdentificado;
  }

  public float GetTotalTime() => _tempoSO.Value;

  public bool IsTargetIdentified() => _targetIdentificado;

  public void TargetIdentified() => TargetIdentificado();

  public void SaveGame() => SalvarJogo();

  public int GetPoints() => _pontosSO.Value;

  private void UpdateTextsQuestaoImageTarget()
  {
    _textQuestaoImageTarget.text =
      _questoesImageTarget[_indiceQuestaoAtual / _totalQuestoesPorSecao]
      .perguntaFracionada[_paginaAtualQuestaoImageTarget];
    _textPaginaAtualQuestaoImageTarget.text =
      _paginaAtualQuestaoImageTarget + 1 +
      "/" +
      _questoesImageTarget[_indiceQuestaoAtual / _totalQuestoesPorSecao].perguntaFracionada.Length;
  }

  private void MostrarBotoesQuestaoMultiplaEscolha()
  {
    foreach (var botao in _botoes)
    {
      botao.SetActive(true);
    }
  }

  private void MostrarBotaoToggleQuestaoImageTarget()
  {
    _buttonToggleQuestaoImageTarget.SetActive(true);
  }

  private void MostrarPerguntaMultiplaEscolha()
  {
    _imageBoxQuestaoMultiplaEscolha.SetActive(true);
  }

  public void MostrarEsconderPerguntaImageTarget()
  {
    _imageBoxQuestaoImageTargetHabilitada = !_imageBoxQuestaoImageTargetHabilitada;
    _imageBoxQuestaoImageTarget.SetActive(_imageBoxQuestaoImageTargetHabilitada);
    UpdateTextsQuestaoImageTarget();
  }

  public void EsconderPerguntaImageTarget()
  {
    _imageBoxQuestaoImageTargetHabilitada = false;
    _imageBoxQuestaoImageTarget.SetActive(_imageBoxQuestaoImageTargetHabilitada);
    _paginaAtualQuestaoImageTarget = 0;
  }

  private void EsconderBotoesQuestaoMultiplaEscolha()
  {
    foreach (var botao in _botoes)
    {
      botao.SetActive(false);
    }
  }

  private void EsconderBotaoToggleQuestaoImageTarget()
  {
    _buttonToggleQuestaoImageTarget.SetActive(false);
  }

  private void EsconderPerguntaMultiplaEscolha()
  {
    _imageBoxQuestaoMultiplaEscolha.SetActive(false);
  }

  public void handlePaginaSeguinte()
  {
    if (
      _imageBoxQuestaoImageTargetHabilitada &&
      _paginaAtualQuestaoImageTarget < _questoesImageTarget[_indiceQuestaoAtual / _totalQuestoesPorSecao].perguntaFracionada.Length - 1)
    {
      _paginaAtualQuestaoImageTarget++;
      UpdateTextsQuestaoImageTarget();
    }
  }

  public void handlePaginaAnterior()
  {
    if (_imageBoxQuestaoImageTargetHabilitada && _paginaAtualQuestaoImageTarget > 0)
    {
      _paginaAtualQuestaoImageTarget--;
      UpdateTextsQuestaoImageTarget();
    }
  }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using TMPro;

[Serializable]
public class CardNome
{
  public GameObject imageBoxCardNome;
  public TextMeshProUGUI textNome;
}

public enum DadosAceitosNoInput
{
  JOGADOR = 1,
  EQUIPE = 2,
  CODIGO = 3
}

public class TeamManager : MonoBehaviour
{
  [SerializeField] public TextMeshProUGUI textEnunciado;
  [SerializeField] public TextMeshProUGUI textMensagemInformativa;
  [SerializeField] public GameObject imageBoxMensagemInformativa;
  [SerializeField] public TMP_InputField inputFieldUnico;
  [SerializeField] public GameObject buttonAdicionar;
  [SerializeField] public GameObject buttonConfirmar;
  [SerializeField] public List<CardNome> cardsJogadores;
  [SerializeField] public CardNome cardEquipe;
  [SerializeField] public CardNome cardCodigo;

  [SerializeField] public StringScriptableObject collectionNameSO;
  [SerializeField] public StringArrayScriptableObject documentsSO;


  private int _quantidadeJogadoresAtual;
  private int _dadoEsperadoNoInput;
  private SetGameData setGameData;
  private const int CENA_JOGO = 4;
  private const int CENA_MENU = 1;

  public void Start()
  {
    _quantidadeJogadoresAtual = 0;
    _dadoEsperadoNoInput = (int)DadosAceitosNoInput.JOGADOR;
    setGameData = new SetGameData();
    inputFieldUnico.characterLimit = 9;
  }

  public void Adicionar()
  {
    if (inputFieldUnico.text.Equals(""))
    {
      ExibirMensagem("Adicione um valor na caixa de texto");
      return;
    }
    if (_dadoEsperadoNoInput == (int)DadosAceitosNoInput.JOGADOR)
    {
      AdicionarJogador();
    }
    else if (_dadoEsperadoNoInput == (int)DadosAceitosNoInput.EQUIPE)
    {
      AdicionarEquipe();
    }
    else if (_dadoEsperadoNoInput == (int)DadosAceitosNoInput.CODIGO)
    {
      AdicionarCodigo();
      EncerrarInput();
    }
    inputFieldUnico.text = "";
  }

  public void AdicionarJogador()
  {
    if (_quantidadeJogadoresAtual == 0) buttonConfirmar.SetActive(true);
    if (_quantidadeJogadoresAtual < cardsJogadores.Count)
    {
      cardsJogadores.ElementAt(_quantidadeJogadoresAtual).imageBoxCardNome.SetActive(true);
      cardsJogadores.ElementAt(_quantidadeJogadoresAtual).textNome.transform.gameObject.SetActive(true);
      cardsJogadores.ElementAt(_quantidadeJogadoresAtual).textNome.text = inputFieldUnico.text;
      _quantidadeJogadoresAtual++;
    }
    if (_quantidadeJogadoresAtual == cardsJogadores.Count) Confirmar();
  }

  public void AdicionarEquipe()
  {
    cardEquipe.imageBoxCardNome.SetActive(true);
    cardEquipe.textNome.text = inputFieldUnico.text;
    Confirmar();
  }

  public void AdicionarCodigo()
  {
    cardCodigo.imageBoxCardNome.SetActive(true);
    cardCodigo.textNome.transform.gameObject.SetActive(true);
    cardCodigo.textNome.text = inputFieldUnico.text.ToUpper();
  }

  public void EncerrarInput()
  {
    inputFieldUnico.enabled = false;
    inputFieldUnico.transform.gameObject.SetActive(false);
    buttonAdicionar.SetActive(false);
    buttonConfirmar.SetActive(true);
    textEnunciado.text = "Tudo pronto! Pressione 'Confirmar' para iniciar o jogo!";
  }

  public void ExibirMensagem(string mensagem)
  {
    if (mensagem.Length <= 42)
    {
      imageBoxMensagemInformativa.SetActive(true);
      textMensagemInformativa.text = mensagem;
    }
  }
  public void FecharMensagem()
  {
    imageBoxMensagemInformativa.SetActive(false);
    textMensagemInformativa.text = "";
  }

  public void PrepararSalvarDados()
  {
    long _timestamp = Stopwatch.GetTimestamp();
    List<string> _nomesJogadores = new();
    List<string> _documentsPaths = new();

    bool _existeEquipe = cardEquipe.textNome.text.Equals("")
      ? false
      : true;

    GameData _gameData;
    string _gamePath;
    string _documentName;

    string _dataAtual = $"{DateTime.Today.ToString("d").Replace("/", "-")}";
    string _codigoSessao = cardCodigo.textNome.text;
    setGameData.NomeCollection = $"{_dataAtual}-{_codigoSessao}";
    collectionNameSO.Value = setGameData.NomeCollection;
    foreach (CardNome card in cardsJogadores)
      if (!card.textNome.text.Equals(""))
        _nomesJogadores.Add(card.textNome.text);

    foreach (string jogador in _nomesJogadores)
    {
      _documentName = _existeEquipe
        ? $"{jogador}-{cardEquipe.textNome.text}-{_timestamp}"
        : $"{jogador}-{_timestamp}";

      _documentsPaths.Add(_documentName);

      _gamePath = $"{setGameData.NomeCollection}/{_documentName}";
      _gameData = new GameData
      {
        Nomes = _nomesJogadores.ToArray(),
        Equipe = cardEquipe.textNome.text ?? "",
        Pontos = 0,
        Tempo = 0,
      };
      setGameData.SaveToCloud(_gamePath, _gameData);
    }
    documentsSO.Value = _documentsPaths.ToArray();
  }

  public void Confirmar()
  {
    if (_dadoEsperadoNoInput == (int)DadosAceitosNoInput.JOGADOR)
    {
      if (_quantidadeJogadoresAtual == 1)
      {
        _dadoEsperadoNoInput = (int)DadosAceitosNoInput.CODIGO;
        textEnunciado.text = "Informe o código da sessão";
        inputFieldUnico.characterLimit = 5;
      }
      else
      {
        _dadoEsperadoNoInput = (int)DadosAceitosNoInput.EQUIPE;
        textEnunciado.text = "Informe o nome da sua equipe!";
        inputFieldUnico.characterLimit = 13;
        buttonConfirmar.SetActive(false);
      }
    }
    else if (_dadoEsperadoNoInput == (int)DadosAceitosNoInput.EQUIPE)
    {
      _dadoEsperadoNoInput = (int)DadosAceitosNoInput.CODIGO;
      textEnunciado.text = "Informe o código da sessão";
      inputFieldUnico.characterLimit = 5;
    }
    else if (_dadoEsperadoNoInput == (int)DadosAceitosNoInput.CODIGO)
    {
      if (_quantidadeJogadoresAtual > 0)
        PrepararSalvarDados();
      SceneManager.LoadScene(CENA_JOGO);
    }
  }
  public void Cancelar()
  {
    SceneManager.LoadScene(CENA_MENU);
  }
}

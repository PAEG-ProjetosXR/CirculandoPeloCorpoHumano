using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
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
    setGameData.CodigoSessao = cardCodigo.textNome.text;
    List<string> nomesJogadores = new List<string>();
    if (_quantidadeJogadoresAtual > 0)
      foreach (CardNome card in cardsJogadores)
        if (!card.textNome.text.Equals(""))
          nomesJogadores.Add(card.textNome.text);

    string nomeEquipe = cardEquipe != null
      ? cardEquipe.textNome.text
      : "";
    setGameData.HandleSave(
      nomesJogadores.ToArray(),
      nomeEquipe,
      0, 0);
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
      PrepararSalvarDados();
      SceneManager.LoadScene(CENA_JOGO);
    }
  }
  public void Cancelar()
  {
    SceneManager.LoadScene(CENA_MENU);
  }
}

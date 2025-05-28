using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Linq;

[System.Serializable]
public class CardNome
{
  public GameObject imageBoxCardNome;
  public TextMeshProUGUI textNome;
}

public class TeamManager : MonoBehaviour
{
  [SerializeField] public TextMeshProUGUI textEnunciado;
  [SerializeField] public TextMeshProUGUI textMensagemInformacao;
  [SerializeField] public TMP_InputField inputJogadorOuEquipe;
  [SerializeField] public GameObject buttonAdicionar;
  [SerializeField] public List<CardNome> cardsJogadores;
  [SerializeField] public CardNome cardEquipe;
  private int _quantidadeJogadoresAtual;
  private bool _inputNoModoEquipe;
  private SetGameData setGameData;
  private const int CENA_JOGO = 4;
  private const int CENA_MENU = 8;

  public void Start()
  {
    _quantidadeJogadoresAtual = 0;
    _inputNoModoEquipe = false;
    setGameData = new();
  }

  public void AdicionarJogadorOuEquipe()
  {
    if (!_inputNoModoEquipe)
    {
      if (_quantidadeJogadoresAtual < cardsJogadores.Count)
      {
        cardsJogadores.ElementAt(_quantidadeJogadoresAtual).imageBoxCardNome.SetActive(true);
        cardsJogadores.ElementAt(_quantidadeJogadoresAtual).textNome.text = inputJogadorOuEquipe.text;
        _quantidadeJogadoresAtual++;

        if (_quantidadeJogadoresAtual == cardsJogadores.Count) buttonAdicionar.SetActive(false);
      }
    }
    else
    {
      cardEquipe.imageBoxCardNome.SetActive(true);
      cardEquipe.textNome.text = inputJogadorOuEquipe.text;

      inputJogadorOuEquipe.enabled = false;
      inputJogadorOuEquipe.transform.gameObject.SetActive(false);
      buttonAdicionar.SetActive(false);

      textEnunciado.text = "Tudo pronto! Pressione 'Confirmar' para iniciar o jogo!";
    }
    inputJogadorOuEquipe.text = "";
  }

  public void ExibirMensagem(string mensagem)
  {
    // setactive no gameobject _textMensagem
  }

  public void Confirmar()
  {
    if (_inputNoModoEquipe || _quantidadeJogadoresAtual == 1)
    {
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
      SceneManager.LoadScene(CENA_JOGO);
    }
    else
    {
      if (_quantidadeJogadoresAtual < 1)
      {
        ExibirMensagem("É necessário adicionar pelo menos um usuário!");
      }
      else if (_quantidadeJogadoresAtual > 1)
      {
        _inputNoModoEquipe = true;
        textEnunciado.text = "Informe o nome da sua equipe!";
        buttonAdicionar.SetActive(true);
      }
    }
  }
  public void Cancelar()
  {
    SceneManager.LoadScene(CENA_MENU);
  }
}

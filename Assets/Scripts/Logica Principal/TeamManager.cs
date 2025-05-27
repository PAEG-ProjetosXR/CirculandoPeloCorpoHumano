using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class TeamManager : MonoBehaviour
{
  [SerializeField] public GameObject imageBoxPrincipal;
  [SerializeField] public TextMeshProUGUI textEnunciado;
  [SerializeField] public TextMeshProUGUI textMensagemInformacao;
  [SerializeField] public TMP_InputField inputJogadorOuEquipe;
  [SerializeField] public GameObject buttonAdicionarAdicionarJogador;
  [SerializeField] public List<CardNome> cardsJogadores;
  [SerializeField] public CardNome cardEquipe;
  private int _quantidadeJogadoresAtual = 0;
  private bool _toggleInputEquipe = false;
  public void AdicionarJogadorOuEquipe()
  {
    if (!_toggleInputEquipe)
    {
      if (_quantidadeJogadoresAtual < cardsJogadores.Count)
      {
        cardsJogadores.ElementAt(_quantidadeJogadoresAtual).imageBoxCardNome.SetActive(true);
        cardsJogadores.ElementAt(_quantidadeJogadoresAtual).textNome.text = inputJogadorOuEquipe.text;
        inputJogadorOuEquipe.text = "";
        _quantidadeJogadoresAtual++;

        if (_quantidadeJogadoresAtual == cardsJogadores.Count) buttonAdicionarAdicionarJogador.SetActive(false);
      }
    }
    else
    {
      cardEquipe.imageBoxCardNome.SetActive(true);
      cardEquipe.textNome.text = inputJogadorOuEquipe.text;
    }
  }

  public void ExibirMensagem(string mensagem)
  {
    // setactive no gameobject _textMensagem
  }

  public void Confirmar()
  {
    if (_quantidadeJogadoresAtual < 1)
    {
      // exibir mensagem
    }
    if (_quantidadeJogadoresAtual == 1)
    {
      // ir para outra tela
    }
    else if (_quantidadeJogadoresAtual > 1)
    {
      _toggleInputEquipe = true;
      textEnunciado.text = "Informe o nome da sua equipe!";
      buttonAdicionarAdicionarJogador.SetActive(true);
    }
  }

  public void Cancelar()
  {

  }
}

[System.Serializable]
public class CardNome
{
  public GameObject imageBoxCardNome;
  public TextMeshProUGUI textNome;
}
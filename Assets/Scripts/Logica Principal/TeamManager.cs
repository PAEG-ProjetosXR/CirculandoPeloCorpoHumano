using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TeamManager
{
  [SerializeField] public GameObject _imageBoxContainer;
  [SerializeField] public TextMeshProUGUI _textEnunciado;
  [SerializeField] public TextMeshProUGUI _textMensagem;
  [SerializeField] public TMP_InputField _inputNomeJogador;
  [SerializeField] public List<GameObject> _imageBoxListJogadores;
  private string _jogadores;

  public void AdicionarJogador()
  {
    // verificar se _jogadores está cheio. Se sim: ExibirMensagem("Nao pode haver mais que {_imageBoxListJogadores.size} jogadores!");
    // adicionar o nome à lista de jogadores
    // Limpar texto do input  
    // SetActive(true) em uma das caixinhas de jogadores, com o
    // texto atual de _inputNomeJogador;
  }

  public void ExibirMensagem(string mensagem)
  {
    // setactive no gameobject _textMensagem
  }

  public void Confirmar()
  {
    // se _jogadores.size > 1, pedir nome da equipe
  }

  public void Cancelar()
  {

  }
}
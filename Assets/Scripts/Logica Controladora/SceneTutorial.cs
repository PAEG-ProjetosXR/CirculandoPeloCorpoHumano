using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controla o fluxo do tutorial, incluindo navegação entre imagens
/// e transição para o jogo ou menu principal.
/// </summary>
public class SceneTutorial : MonoBehaviour
{
  //-----------------------------
  // Configurações (Inspector)
  //-----------------------------
  [SerializeField] private Sprite[] _imagensTutorial;  // Array com todas as imagens do tutorial
  [SerializeField] private Image _imagemExibida;      // Componente que exibe a imagem atual
  [SerializeField] private float _delayBotao = 0.5f;  // Delay para ações com feedback
  [SerializeField] private DictionaryExibicaoPorPagina _dictionaryExibicaoPorPagina;
  Dictionary<int, GameObject[]> componentesPorPagina;
  GameObject[] componentesPaginaAtual = new GameObject[0];

  //-----------------------------
  // Estado Interno
  //-----------------------------
  private int _indiceImagemAtual = 0;  // Controla a imagem sendo exibida

  [System.Serializable]
  public class DictionaryExibicaoPorPagina
  {
    [SerializeField]
    public DictionaryItemExibicaoPorPagina[] _itemExibicaoPorPagina;

    public Dictionary<int, GameObject[]> ToDictionary()
    {
      Dictionary<int, GameObject[]> newDictionary = new Dictionary<int, GameObject[]>();
      foreach (var item in _itemExibicaoPorPagina)
      {
        newDictionary.Add(item._pagina, item._componentes);
      }
      return newDictionary;
    }
  }

  [System.Serializable]
  public class DictionaryItemExibicaoPorPagina
  {
    [SerializeField]
    public int _pagina;
    [SerializeField]
    public GameObject[] _componentes;
  }

  //-----------------------------
  // Inicialização
  //-----------------------------

  private void Start()
  {
    componentesPorPagina = _dictionaryExibicaoPorPagina.ToDictionary();
    AtualizarImagem();
  }

  //-----------------------------
  // Controles de Navegação
  //-----------------------------

  /// <summary>
  /// Avança para a próxima imagem do tutorial (sem delay)
  /// </summary>
  public void AvancarImagem()
  {
    if (_indiceImagemAtual < _imagensTutorial.Length - 1)
    {
      _indiceImagemAtual++;
      AtualizarImagem();
    }
  }

  /// <summary>
  /// Volta para a imagem anterior do tutorial (sem delay)
  /// </summary>
  public void VoltarImagem()
  {
    if (_indiceImagemAtual > 0)
    {
      _indiceImagemAtual--;
      AtualizarImagem();
    }
  }

  //-----------------------------
  // Ações Principais (com delay)
  //-----------------------------

  /// <summary>
  /// Retorna ao menu principal após o delay configurado
  /// </summary>
  public void VoltarAoMenu()
  {
    StartCoroutine(CarregarCenaComDelay(8));  // Cena 8 = Menu
  }

  /// <summary>
  /// Inicia o jogo principal após o delay configurado
  /// </summary>
  public void IniciarJogo()
  {
    StartCoroutine(CarregarCenaComDelay(4));  // Cena 4 = Jogo
  }

  //-----------------------------
  // Métodos Internos
  //-----------------------------

  /// <summary>
  /// Carrega uma cena específica após o delay
  /// </summary>
  private IEnumerator CarregarCenaComDelay(int indiceCena)
  {
    yield return new WaitForSeconds(_delayBotao);
    SceneManager.LoadScene(indiceCena);
  }

  /// <summary>
  /// Atualiza a imagem exibida e controla a visibilidade do botão Iniciar
  /// </summary>
  private void AtualizarImagem()
  {
    _imagemExibida.sprite = _imagensTutorial[_indiceImagemAtual];
    if (componentesPaginaAtual != null && componentesPaginaAtual.Length > 0)
      for (int objeto = 0; objeto < componentesPaginaAtual.Length; objeto++)
        componentesPaginaAtual[objeto].SetActive(false);

    if (componentesPorPagina.ContainsKey(_indiceImagemAtual))
    {
      componentesPaginaAtual = componentesPorPagina[_indiceImagemAtual];
      for (int objeto = 0; objeto < componentesPaginaAtual.Length; objeto++)
        componentesPaginaAtual[objeto].SetActive(true);
    }
    else componentesPaginaAtual = null;

  }
}
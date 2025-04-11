using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla a reprodução de vídeos aleatórios durante o carregamento
/// e transição para a próxima cena
/// </summary>
public class RandomLoadingVideo : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Vídeo
    //-----------------------------
    [Header("Configurações de Vídeo")]
    [SerializeField] private VideoPlayer[] _videoPlayers;     // Array de VideoPlayers disponíveis
    [SerializeField] private float _delayPosVideo = 1f;       // Delay após término do vídeo
    [SerializeField] private int _cenaDestino = 2;           // Índice da cena de destino

    //-----------------------------
    // Variáveis de Estado
    //-----------------------------
    private VideoPlayer _videoAtual;                          // Referência ao vídeo sendo exibido

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Start() 
    {
        IniciarVideoAleatorio();
    }

    //-----------------------------
    // Controle de Vídeo
    //-----------------------------

    /// <summary>
    /// Seleciona e reproduz um vídeo aleatório da lista
    /// </summary>
    private void IniciarVideoAleatorio() 
    {
        DesativarTodosVideos();

        int indiceAleatorio = Random.Range(0, _videoPlayers.Length);
        _videoAtual = _videoPlayers[indiceAleatorio];
        
        ConfigurarVideoPlayer(_videoAtual);
        _videoAtual.Play();
    }

    /// <summary>
    /// Desativa todos os VideoPlayers inicialmente
    /// </summary>
    private void DesativarTodosVideos() 
    {
        foreach (var player in _videoPlayers)
        {
            player.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Configura o VideoPlayer selecionado
    /// </summary>
    private void ConfigurarVideoPlayer(VideoPlayer player) 
    {
        player.gameObject.SetActive(true);
        player.loopPointReached += AoFinalizarVideo;
    }

    //-----------------------------
    // Eventos
    //-----------------------------

    /// <summary>
    /// Callback quando o vídeo termina de reproduzir
    /// </summary>
    private void AoFinalizarVideo(VideoPlayer vp) 
    {
        Invoke(nameof(CarregarProximaCena), _delayPosVideo);
    }

    //-----------------------------
    // Navegação de Cena
    //-----------------------------

    /// <summary>
    /// Carrega a cena de destino após o término do vídeo
    /// </summary>
    private void CarregarProximaCena() 
    {
        SceneManager.LoadScene(_cenaDestino);
    }

    //-----------------------------
    // Cleanup
    //-----------------------------

    private void OnDestroy() 
    {
        if (_videoAtual != null)
        {
            _videoAtual.loopPointReached -= AoFinalizarVideo;
        }
    }

    //-----------------------------
    // Métodos Opcionais (Região para implementação futura)
    //-----------------------------
    /*
    [Header("Configurações Adicionais")]
    [SerializeField] private TextMeshProUGUI _textoCarregamento;
    [SerializeField] private Image _barraProgresso;

    private void AtualizarUIProgresso(float progresso)
    {
        _barraProgresso.fillAmount = progresso;
        _textoCarregamento.text = $"Carregando: {Mathf.Round(progresso * 100)}%";
    }
    */
}
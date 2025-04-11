using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla a exibição do vídeo de splash screen com opção de skip automático
/// </summary>
public class SplashVideo : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Vídeo
    //-----------------------------
    [Header("Configurações do Vídeo")]
    [SerializeField] private VideoPlayer _videoPlayer;        // Componente de vídeo
    [SerializeField] private float _tempoSkip = 5f;          // Tempo máximo de exibição
    [SerializeField] private int _cenaDestino = 1;           // Cena do menu principal

    //-----------------------------
    // Estado do Vídeo
    //-----------------------------
    private float _tempoDecorrido = 0f;
    private bool _videoFinalizado = false;

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Start() 
    {
        ConfigurarVideoPlayer();
        _videoPlayer.Play();
    }

    //-----------------------------
    // Controle de Vídeo
    //-----------------------------

    /// <summary>
    /// Configura o componente VideoPlayer
    /// </summary>
    private void ConfigurarVideoPlayer() 
    {
        if (_videoPlayer == null)
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        _videoPlayer.loopPointReached += AoFinalizarVideo;
    }

    //-----------------------------
    // Atualização
    //-----------------------------

    private void Update() 
    {
        if (!_videoFinalizado)
        {
            _tempoDecorrido += Time.deltaTime;

            if (_tempoDecorrido >= _tempoSkip)
            {
                CarregarCenaMenu();
            }
        }
    }

    //-----------------------------
    // Eventos
    //-----------------------------

    /// <summary>
    /// Callback quando o vídeo termina naturalmente
    /// </summary>
    private void AoFinalizarVideo(VideoPlayer vp) 
    {
        _videoFinalizado = true;
        CarregarCenaMenu();
    }

    //-----------------------------
    // Navegação de Cena
    //-----------------------------

    /// <summary>
    /// Carrega a cena do menu principal
    /// </summary>
    private void CarregarCenaMenu() 
    {
        SceneManager.LoadScene(_cenaDestino);
    }

    //-----------------------------
    // Cleanup
    //-----------------------------

    private void OnDestroy() 
    {
        if (_videoPlayer != null)
        {
            _videoPlayer.loopPointReached -= AoFinalizarVideo;
        }
    }

    //-----------------------------
    // Métodos Opcionais (Implementação Futura)
    //-----------------------------
    /*
    [Header("Interação do Usuário")]
    [SerializeField] private GameObject _botaoSkip;
    [SerializeField] private float _tempoAparecerBotao = 2f;

    public void OnSkipPressed()
    {
        CarregarCenaMenu();
    }
    */
}
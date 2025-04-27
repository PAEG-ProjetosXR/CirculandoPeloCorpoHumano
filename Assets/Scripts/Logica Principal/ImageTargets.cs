using UnityEngine;

/// <summary>
/// Controla o comportamento dos Image Targets no jogo, incluindo:
/// - Reprodução de áudio quando identificado
/// - Comunicação com o GameManager
/// </summary>
public class ImageTargets : MonoBehaviour 
{
    //-----------------------------
    // Configurações de Áudio
    //-----------------------------
    [Header("Configurações de Áudio")]
    [SerializeField] private AudioClip _audioTarget;  // Áudio a ser reproduzido quando o target é identificado
    private AudioSource _audioSource;                // Componente para reprodução do áudio

    //-----------------------------
    // Inicialização
    //-----------------------------

    /// <summary>
    /// Configura o componente AudioSource no início
    /// </summary>
    private void Start() 
    {
        ConfigurarAudioSource();
    }

    //-----------------------------
    // Métodos Públicos
    //-----------------------------

    /// <summary>
    /// Chamado quando o target é identificado pelo AR
    /// </summary>
    public void GoNextQuestion() 
    {
        ReproduzirAudioTarget();
        NotificarGameManager();
    }

    //-----------------------------
    // Métodos Privados
    //-----------------------------

    /// <summary>
    /// Configura o componente AudioSource, adicionando se necessário
    /// </summary>
    private void ConfigurarAudioSource() 
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) 
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("Componente AudioSource adicionado ao target.");
        }
    }

    /// <summary>
    /// Reproduz o áudio associado ao target
    /// </summary>
    private void ReproduzirAudioTarget() 
    {
        if (_audioTarget != null) 
        {
            _audioSource.clip = _audioTarget;
            _audioSource.Play();
        }
        else 
        {
            Debug.LogWarning("Nenhum AudioClip atribuído ao target no Inspector.");
        }
    }

    /// <summary>
    /// Notifica o GameManager sobre a identificação do target
    /// </summary>
    private void NotificarGameManager() 
    {
        if (GameManager.Instance == null) 
        {
            Debug.LogError("GameManager não encontrado na cena.");
            return;
        }

        if (GameManager.Instance.IsTargetIdentified()) 
        {
            Debug.LogWarning("Target já foi identificado anteriormente.");
            return;
        }

        GameManager.Instance.TargetIdentified();
    }
}
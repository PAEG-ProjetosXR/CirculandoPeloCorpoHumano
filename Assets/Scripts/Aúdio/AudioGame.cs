using UnityEngine;

/// <summary>
/// Controla a reprodução de áudio em um GameObject, carregando um AudioClip da pasta Resources.
/// </summary>
public class PlayAudio : MonoBehaviour 
{
    //-----------------------------
    // Componentes de Áudio
    //-----------------------------
    private AudioSource _audioSource;  // Referência ao componente AudioSource

    //-----------------------------
    // Métodos Unity
    //-----------------------------

    /// <summary>
    /// Inicializa o áudio no início do jogo.
    /// </summary>
    private void Start() 
    {
        InitializeAudio();
    }

    //-----------------------------
    // Lógica de Áudio
    //-----------------------------

    /// <summary>
    /// Configura o AudioSource e carrega o arquivo de áudio.
    /// Logs de erro são gerados se o AudioSource ou o arquivo não forem encontrados.
    /// </summary>
    private void InitializeAudio() 
    {
        // Busca o componente AudioSource anexado ao GameObject
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null) 
        {
            Debug.LogError("AudioSource não encontrado! Adicione um AudioSource ao GameObject.");
            return;
        }

        LoadAndPlayAudioClip();
    }

    /// <summary>
    /// Carrega o AudioClip da pasta Resources e inicia a reprodução.
    /// </summary>
    private void LoadAndPlayAudioClip() 
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Audio/SeuAudio");

        if (audioClip != null) 
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
        else 
        {
            Debug.LogError("Falha ao carregar áudio. Verifique: " +
                           "\n1. Caminho correto (pasta Resources/Audio/)" +
                           "\n2. Nome do arquivo (case-sensitive)" +
                           "\n3. Extensão do arquivo (não incluir .wav/.mp3 no caminho)");
        }
    }
}
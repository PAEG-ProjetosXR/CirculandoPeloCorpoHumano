using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Gerencia todas as interações do menu, incluindo transições de cena,
/// abertura de URLs e fornece feedback visual com delays configuráveis.
/// </summary>
public class SceneMenuManager : MonoBehaviour 
{
    //-----------------------------
    // Configurações (Inspector)
    //-----------------------------
    [SerializeField] private float _delayBotao = 0.5f;  // Tempo de delay para feedback visual nos botões

    //-----------------------------
    // Constantes
    //-----------------------------
    private const string URL_INSTAGRAM = "https://www.instagram.com/labpeem";
    private const string URL_YOUTUBE = "https://youtube.com/@labpeem?si=90b1K1I6JiDOzU9T";
    private const string URL_DOCUMENTO = "https://docs.google.com/document/d/1Phe72vCEwR2hdSF-3QvBxamerGJeoMtz/edit";
    
    private const int CENA_JOGO = 4;      // Índice da cena principal do jogo
    private const int CENA_TUTORIAL = 7;  // Índice da cena de tutorial

    //-----------------------------
    // Métodos Públicos (Eventos de UI)
    //-----------------------------

    /// <summary>
    /// Inicia a cena do jogo principal após o delay do botão.
    /// Destrói a instância do GameManager se existir.
    /// </summary>
    public void IniciarJogo() 
    {
        StartCoroutine(CarregarCenaComDelay(CENA_JOGO));
    }

    /// <summary>
    /// Carrega a cena de tutorial após o delay do botão.
    /// </summary>
    public void AbrirTutorial() 
    {
        StartCoroutine(CarregarCenaComDelay(CENA_TUTORIAL));
    }

    /// <summary>
    /// Abre a página do Instagram no navegador padrão após delay.
    /// </summary>
    public void AbrirInstagram() 
    {
        StartCoroutine(AbrirURLComDelay(URL_INSTAGRAM));
    }

    /// <summary>
    /// Abre o canal do YouTube no navegador padrão após delay.
    /// </summary>
    public void AbrirYouTube() 
    {
        StartCoroutine(AbrirURLComDelay(URL_YOUTUBE));
    }

    /// <summary>
    /// Abre o documento do Google no navegador padrão após delay.
    /// </summary>
    public void AbrirDocumento() 
    {
        StartCoroutine(AbrirURLComDelay(URL_DOCUMENTO));
    }

    //-----------------------------
    // Gerenciamento de Cenas
    //-----------------------------

    /// <summary>
    /// Carrega uma cena específica após o delay configurado.
    /// </summary>
    /// <param name="indiceCena">Índice da cena a ser carregada</param>
    private IEnumerator CarregarCenaComDelay(int indiceCena) 
    {
        yield return new WaitForSeconds(_delayBotao);
        
        // Caso especial: Reinicia o estado do jogo ao começar novo jogo
        if (indiceCena == CENA_JOGO && GameManager.Instance != null) 
        {
            Destroy(GameManager.Instance.gameObject);
        }
        
        SceneManager.LoadScene(indiceCena);
    }

    //-----------------------------
    // Manipulação de URLs
    //-----------------------------

    /// <summary>
    /// Abre uma URL após o delay configurado.
    /// </summary>
    /// <param name="url">URL completa para abrir</param>
    private IEnumerator AbrirURLComDelay(string url) 
    {
        yield return new WaitForSeconds(_delayBotao);
        AbrirURL(url);
    }

    /// <summary>
    /// Abre uma URL externa com tratamento de erros.
    /// </summary>
    /// <param name="url">URL válida</param>
    private void AbrirURL(string url) 
    {
        try 
        {
            Application.OpenURL(url);
            Debug.Log($"URL aberta com sucesso: {url}");
        }
        catch (System.Exception e) 
        {
            Debug.LogError($"Falha ao abrir URL: {url}\nErro: {e.Message}");
        }
    }
}
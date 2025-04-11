using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Controla a navegação de retorno no jogo, incluindo:
/// - Voltar ao menu principal
/// - Abrir formulários externos
/// - Sair do jogo
/// </summary>
public class SceneReturnManager : MonoBehaviour 
{
    //-----------------------------
    // Configurações (Inspector)
    //-----------------------------
    
    [Header("Configurações de Cenas")]
    [SerializeField] private int _cenaMenuPrincipal = 0;  // Índice da cena do menu
    [SerializeField] private string _urlFormularioAvaliacao = "https://docs.google.com/forms/d/e/1FAIpQLSdAFitGP2hh5ZlRKyZTmtA6q8FPZXFcDQyhVIMyUYpw8StlMg/viewform?usp=dialog";

    [Header("Configurações de Botões")]
    [SerializeField] private float _delayBotao = 0.5f;  // Tempo de feedback visual para os botões

    //-----------------------------
    // Métodos Públicos (Eventos de UI)
    //-----------------------------

    /// <summary>
    /// Volta ao menu principal após o delay configurado
    /// </summary>
    public void VoltarAoMenu() 
    {
        StartCoroutine(AcaoComDelay(() => SceneManager.LoadScene(_cenaMenuPrincipal)));
    }

    /// <summary>
    /// Sai do jogo (ou do play mode no editor) após delay
    /// </summary>
    public void SairDoJogo() 
    {
        StartCoroutine(AcaoComDelay(() => 
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }));
    }

    /// <summary>
    /// Abre o formulário de avaliação no navegador após delay
    /// </summary>
    public void AbrirFormularioAvaliacao() 
    {
        StartCoroutine(AcaoComDelay(() => 
        {
            try 
            {
                Application.OpenURL(_urlFormularioAvaliacao);
                Debug.Log($"Formulário aberto: {_urlFormularioAvaliacao}");
            }
            catch (System.Exception e) 
            {
                Debug.LogError($"Falha ao abrir formulário: {e.Message}");
            }
        }));
    }

    //-----------------------------
    // Utilitários
    //-----------------------------

    /// <summary>
    /// Corrotina genérica para adicionar delay a ações de UI
    /// </summary>
    /// <param name="acao">Ação a ser executada após o delay</param>
    private IEnumerator AcaoComDelay(System.Action acao) 
    {
        yield return new WaitForSeconds(_delayBotao);
        acao?.Invoke();
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla ações de botões no menu do jogo, incluindo navegação entre cenas
/// e salvamento de progresso.
/// </summary>
public class MenuButtonController : MonoBehaviour 
{
    //-----------------------------
    // Constantes
    //-----------------------------
    private const int MAIN_MENU_SCENE_INDEX = 0;  // Índice da cena do menu principal

    //-----------------------------
    // Métodos Públicos (UI Events)
    //-----------------------------

    /// <summary>
    /// Carrega a cena do menu principal após salvar o progresso atual.
    /// Chamado via evento de clique de botão na UI.
    /// </summary>
    public void LoadMenuScene() 
    {
        SaveGameProgress();
        ReturnToMainMenu();
    }

    //-----------------------------
    // Lógica de Navegação
    //-----------------------------

    /// <summary>
    /// Retorna ao menu principal.
    /// </summary>
    private void ReturnToMainMenu() 
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    //-----------------------------
    // Lógica de Progresso
    //-----------------------------

    /// <summary>
    /// Salva o estado atual do jogo se o GameManager existir.
    /// </summary>
    private void SaveGameProgress() 
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.SaveGame();
        }
    }
}
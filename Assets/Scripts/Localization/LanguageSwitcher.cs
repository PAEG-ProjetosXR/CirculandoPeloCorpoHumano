using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings; // Essencial para acessar as configurações de localização

public class LanguageSwitcher : MonoBehaviour
{
    // Esta será a função pública que o seu botão irá chamar
    public void ToggleLocale()
    {
        // Garante que o sistema de localização já foi inicializado
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            // Se não estiver pronto, espere um pouco e tente novamente ou simplesmente retorne.
            // Para um botão, retornar é seguro.
            return;
        }

        // Pega a lista de Locales disponíveis que você configurou no projeto
        var locales = LocalizationSettings.AvailableLocales.Locales;

        // Pega o Locale selecionado no momento
        var currentLocale = LocalizationSettings.SelectedLocale;

        // Encontra o índice do Locale atual na lista
        int currentIndex = locales.IndexOf(currentLocale);

        // Calcula o próximo índice, dando a volta para o início se chegar ao fim da lista
        int nextIndex = (currentIndex + 1) % locales.Count;

        // Define o Locale selecionado para o próximo da lista
        LocalizationSettings.SelectedLocale = locales[nextIndex];

        Debug.Log($"Idioma alterado para: {LocalizationSettings.SelectedLocale.LocaleName}");
    }
}
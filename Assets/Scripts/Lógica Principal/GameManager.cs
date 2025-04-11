using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Classe principal que gerencia toda a lógica do jogo, incluindo:
/// - Pontuação e tempo
/// - Controle de questões (múltipla escolha e image targets)
/// - Navegação entre cenas
/// - Sistema de salvamento
/// </summary>
public class GameManager : MonoBehaviour
{
    //-----------------------------
    // Singleton Instance
    //-----------------------------
    public static GameManager Instance;

    //-----------------------------
    // Variáveis de Estado do Jogo
    //-----------------------------
    private int _pontos;                     // Pontuação total do jogador
    private string _statusGame;              // Estado atual ("Play", "GameOver")
    private float _tempo;                    // Tempo restante por questão
    private float _tempoTotal;               // Tempo acumulado no jogo
    private int _indiceQuestaoAtual;         // Índice da questão atual
    private int _totalQuestoes;              // Quantidade total de questões
    private bool _targetIdentificado;        // Flag para controle de image targets
    private bool _questaoMultiplaEscolha;    // Tipo da questão atual
    private bool _botoesHabilitados;         // Controle de interação com botões

    //-----------------------------
    // Pontuação por Questão
    //-----------------------------
    private int[] _pontosPorQuestao;         // Pontos ganhos por questão de image target

    //-----------------------------
    // Referências de UI
    //-----------------------------
    [Header("Configurações de UI")]
    [SerializeField] private TextMeshProUGUI _textoQuestao;
    [SerializeField] private TextMeshProUGUI _textoPontos;
    [SerializeField] private TextMeshProUGUI _textoTempo;
    private GameObject _telaCarregamento;
    private TextMeshProUGUI _textoCarregamento;
    [SerializeField] private GameObject _fundo;

    //-----------------------------
    // Botões e Cores
    //-----------------------------
    [Header("Configurações de Botões")]
    [SerializeField] private GameObject[] _botoes;
    [SerializeField] private Color _corAcerto = Color.green;
    [SerializeField] private Color _corErro = Color.red;

    //-----------------------------
    // Áudio
    //-----------------------------
    [Header("Configurações de Áudio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _somAcerto;
    [SerializeField] private AudioClip _somErro;

    //-----------------------------
    // Questões e Targets
    //-----------------------------
    [Header("Banco de Questões")]
    [SerializeField] private List<string> _questoes;
    [SerializeField] private List<QuestaoMultiplaEscolha> _questoesMultiplaEscolha;
    [SerializeField] private List<GameObject> _imageTargets;
    private GameObject _targetAtual;

    //-----------------------------
    // Randomização
    //-----------------------------
    private List<List<int>> _indicesRandomizados;

    //-----------------------------
    // Corrotinas
    //-----------------------------
    private IEnumerator _corrotinaTempo;

    //-----------------------------
    // Classes Auxiliares
    //-----------------------------
    [System.Serializable]
    public class QuestaoMultiplaEscolha
    {
        public string pergunta;
        public string[] alternativas;
        public int indiceRespostaCorreta;
    }

    //-----------------------------
    // Inicialização
    //-----------------------------

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        IniciarJogo();
        ResetarCoresBotoes();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    //-----------------------------
    // Controle de Cenas
    //-----------------------------

    private void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        if (cena.buildIndex == 1) // Cena do jogo
        {
            IniciarJogo();
        }
    }

    //-----------------------------
    // Lógica Principal do Jogo
    //-----------------------------

    /// <summary>
    /// Inicia ou reinicia o jogo com os valores padrão
    /// </summary>
    private void IniciarJogo()
    {
        ResetarJogo();
        RandomizarQuestoesPorSecao();
        MostrarProximaQuestao();
    }

    /// <summary>
    /// Reseta todas as variáveis de estado do jogo
    /// </summary>
    private void ResetarJogo()
    {
        _pontos = 0;
        _indiceQuestaoAtual = -1;
        _totalQuestoes = _questoes.Count + _questoesMultiplaEscolha.Count;
        _tempo = 60f;
        _tempoTotal = 0f;
        _statusGame = "Play";
        _targetIdentificado = false;
        _questaoMultiplaEscolha = false;
        _botoesHabilitados = true;

        // Inicializa pontos por questão
        _pontosPorQuestao = new int[_questoes.Count];
        for (int i = 0; i < _pontosPorQuestao.Length; i++)
        {
            _pontosPorQuestao[i] = 0;
        }

        AtualizarHUD();
    }

    /// <summary>
    /// Randomiza a ordem das questões de múltipla escolha por seção
    /// </summary>
    private void RandomizarQuestoesPorSecao()
    {
        _indicesRandomizados = new List<List<int>>();

        for (int secao = 0; secao < _questoesMultiplaEscolha.Count / 4; secao++)
        {
            List<int> indicesSecao = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                indicesSecao.Add(secao * 4 + i);
            }

            // Embaralha as questões dentro da seção
            for (int i = 0; i < indicesSecao.Count; i++)
            {
                int indiceRandomico = Random.Range(i, indicesSecao.Count);
                int temp = indicesSecao[i];
                indicesSecao[i] = indicesSecao[indiceRandomico];
                indicesSecao[indiceRandomico] = temp;
            }

            _indicesRandomizados.Add(indicesSecao);
        }
    }

    //-----------------------------
    // Controle de Questões
    //-----------------------------

    /// <summary>
    /// Avança para a próxima questão ou finaliza o jogo
    /// </summary>
    public void MostrarProximaQuestao()
    {
        // Calcula tempo gasto na questão anterior
        if (_indiceQuestaoAtual >= 0 && _indiceQuestaoAtual < _totalQuestoes)
        {
            float tempoGasto = 60f - _tempo;
            if (tempoGasto > 0 && tempoGasto <= 60f)
            {
                _tempoTotal += tempoGasto;
                Debug.Log($"Tempo gasto na questão {_indiceQuestaoAtual}: {tempoGasto} segundos");
            }
        }

        if (_indiceQuestaoAtual < _totalQuestoes - 1)
        {
            _indiceQuestaoAtual++;
            _questaoMultiplaEscolha = (_indiceQuestaoAtual % 5 != 0);

            if (_questaoMultiplaEscolha)
            {
                ConfigurarQuestaoMultiplaEscolha();
            }
            else
            {
                ConfigurarQuestaoImageTarget();
            }
        }
        else
        {
            FinalizarJogo();
            return;
        }

        _tempo = 60f;
        _targetIdentificado = false;
        IniciarContagemRegressiva();
        AtualizarHUD();
    }

    private void ConfigurarQuestaoMultiplaEscolha()
    {
        int indiceSecao = (_indiceQuestaoAtual / 5);
        int indiceQuestaoNaSecao = (_indiceQuestaoAtual % 5 - 1);

        if (indiceSecao < _indicesRandomizados.Count && indiceQuestaoNaSecao < 4)
        {
            int indiceQuestao = _indicesRandomizados[indiceSecao][indiceQuestaoNaSecao];

            if (indiceQuestao < _questoesMultiplaEscolha.Count)
            {
                if (_fundo != null) _fundo.SetActive(true);

                _textoQuestao.text = _questoesMultiplaEscolha[indiceQuestao].pergunta;
                MostrarBotoes();
                AtualizarBotoes(indiceQuestao);
                _botoesHabilitados = true;
            }
        }
    }

    private void ConfigurarQuestaoImageTarget()
    {
        if (_fundo != null) _fundo.SetActive(false);

        _textoQuestao.text = _questoes[_indiceQuestaoAtual / 5];
        EsconderBotoes();
        if (_targetAtual != null) Destroy(_targetAtual);
        _targetAtual = Instantiate(_imageTargets[_indiceQuestaoAtual / 5], Vector3.zero, Quaternion.identity);
    }

    private void FinalizarJogo()
    {
        _statusGame = "GameOver";
        CarregarCenaComTelaCarregamento(5); // Cena de Game Over
    }

    //-----------------------------
    // Controle de Image Targets
    //-----------------------------

    /// <summary>
    /// Chamado quando o jogador identifica um image target
    /// </summary>
    public void TargetIdentificado()
    {
        if (!_targetIdentificado && !_questaoMultiplaEscolha)
        {
            _targetIdentificado = true;
            PararContagemRegressiva();

            if (_audioSource != null) _audioSource.Play();

            int questaoAtual = _indiceQuestaoAtual / 5;
            if (_pontosPorQuestao[questaoAtual] < 10)
            {
                _pontosPorQuestao[questaoAtual] += 10;
                _pontos += 10;
                AtualizarHUD();
                Debug.Log($"Pontos ganhos para a questão {questaoAtual}: {_pontosPorQuestao[questaoAtual]}");
            }
            else
            {
                Debug.Log("Pontos máximos já alcançados para essa questão.");
            }

            StartCoroutine(EsperarEProximaQuestao(3f));
        }
    }

    //-----------------------------
    // Controle de Respostas
    //-----------------------------

    /// <summary>
    /// Verifica a resposta selecionada em questões de múltipla escolha
    /// </summary>
    public void VerificarResposta(int indiceBotao)
    {
        if (_questaoMultiplaEscolha && _botoesHabilitados)
        {
            _botoesHabilitados = false;
            PararContagemRegressiva();

            int indiceSecao = (_indiceQuestaoAtual / 5);
            int indiceQuestaoNaSecao = (_indiceQuestaoAtual % 5 - 1);

            if (indiceSecao < _indicesRandomizados.Count && indiceQuestaoNaSecao < 4)
            {
                int indiceQuestao = _indicesRandomizados[indiceSecao][indiceQuestaoNaSecao];

                if (indiceQuestao < _questoesMultiplaEscolha.Count)
                {
                    if (indiceBotao == _questoesMultiplaEscolha[indiceQuestao].indiceRespostaCorreta)
                    {
                        AumentarPontuacao(10);
                        TocarSom(_somAcerto);
                    }
                    else
                    {
                        TocarSom(_somErro);
                    }

                    AtualizarCoresBotoes(_questoesMultiplaEscolha[indiceQuestao].indiceRespostaCorreta);
                    StartCoroutine(EsperarEProximaQuestao(2f));
                }
            }
        }
    }

    //-----------------------------
    // Métodos Auxiliares
    //-----------------------------

    private void AtualizarBotoes(int indiceQuestao)
    {
        if (indiceQuestao < _questoesMultiplaEscolha.Count)
        {
            for (int i = 0; i < _botoes.Length; i++)
            {
                if (i < _questoesMultiplaEscolha[indiceQuestao].alternativas.Length)
                {
                    TextMeshProUGUI textoBotao = _botoes[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (textoBotao != null) textoBotao.text = _questoesMultiplaEscolha[indiceQuestao].alternativas[i];
                }
            }
        }
    }

    private IEnumerator EsperarEProximaQuestao(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetarCoresBotoes();
        MostrarProximaQuestao();
    }

    private void TocarSom(AudioClip clip)
    {
        if (_audioSource != null && clip != null)
        {
            Debug.Log($"Reproduzindo som: {clip.name}");
            _audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioSource ou AudioClip não configurado!");
        }
    }

    private void AtualizarCoresBotoes(int indiceBotaoCorreto)
    {
        for (int i = 0; i < _botoes.Length; i++)
        {
            _botoes[i].GetComponent<Image>().color = 
                (i == indiceBotaoCorreto) ? _corAcerto : _corErro;
        }
    }

    private void ResetarCoresBotoes()
    {
        foreach (var botao in _botoes)
        {
            botao.GetComponent<Image>().color = Color.white;
        }
    }

    public void AumentarPontuacao(int valor)
    {
        _pontos += valor;
        AtualizarHUD();
    }

    private void AtualizarHUD()
    {
        if (_textoPontos != null)
            _textoPontos.text = $"PONTOS: {_pontos}";

        if (_textoTempo != null)
            _textoTempo.text = $"TEMPO: {Mathf.CeilToInt(_tempo)}";
    }

    //-----------------------------
    // Controle de Tempo
    //-----------------------------

    private void IniciarContagemRegressiva()
    {
        PararContagemRegressiva();
        _corrotinaTempo = ContadorTempo();
        StartCoroutine(_corrotinaTempo);
    }

    private void PararContagemRegressiva()
    {
        if (_corrotinaTempo != null)
        {
            StopCoroutine(_corrotinaTempo);
            _corrotinaTempo = null;
        }
    }

    public void PararContagemRegressivaJogo()
    {
        PararContagemRegressiva();
    }

    private IEnumerator ContadorTempo()
    {
        while (_tempo > 0 && _statusGame == "Play" && !_targetIdentificado)
        {
            yield return new WaitForSeconds(1f);
            _tempo -= 1f;
            AtualizarHUD();
        }

        if (_tempo <= 0)
        {
            Debug.Log("Tempo esgotado! Avançando para a próxima questão.");
            StartCoroutine(EsperarEProximaQuestao(1f));
        }
    }

    //-----------------------------
    // Controle de Cenas
    //-----------------------------

    public void CarregarCenaComTelaCarregamento(int indiceCena)
    {
        StartCoroutine(CarregarCenaAssincrona(indiceCena));
    }

    private IEnumerator CarregarCenaAssincrona(int indiceCena)
    {
        if (_telaCarregamento != null)
        {
            _telaCarregamento.SetActive(true);
            _textoCarregamento.text = "Carregando...";
        }
        
        yield return new WaitForSeconds(0.5f);
        
        AsyncOperation carregamento = SceneManager.LoadSceneAsync(indiceCena);
        while (!carregamento.isDone)
        {
            yield return null;
        }
        
        if (_telaCarregamento != null)
        {
            _telaCarregamento.SetActive(false);
        }
    }

    //-----------------------------
    // Sistema de Salvamento
    //-----------------------------

    public void SalvarJogo()
    {
        PlayerPrefs.SetInt("Pontos", _pontos);
        PlayerPrefs.SetFloat("TotalTimeSpent", _tempoTotal);
        PlayerPrefs.Save();
        Debug.Log("Jogo salvo!");
    }

    //-----------------------------
    // Getters
    //-----------------------------

    public float GetTempoTotal()
    {
        return _tempoTotal;
    }

    public int GetPontos()
    {
        return _pontos;
    }

    public bool IsTargetIdentificado()
    {
        return _targetIdentificado;
    }

    //-----------------------------
    // Aliases para compatibilidade (sem alterar o resto do código)
    //-----------------------------

    /// <summary>
    /// Versão em inglês de GetTempoTotal para compatibilidade
    /// </summary>
    public float GetTotalTime() => _tempoTotal;

    /// <summary>
    /// Versão em inglês de IsTargetIdentificado para compatibilidade
    /// </summary>
    public bool IsTargetIdentified() => _targetIdentificado;

    /// <summary>
    /// Versão em inglês de TargetIdentificado para compatibilidade
    /// </summary>
    public void TargetIdentified() => TargetIdentificado();

    /// <summary>
    /// Versão em inglês de SalvarJogo para compatibilidade
    /// </summary>
    public void SaveGame() => SalvarJogo();

    /// <summary>
    /// Versão em inglês de GetPontos para compatibilidade
    /// </summary>
    public int GetPoints() => _pontos;

    //-----------------------------
    // Controle de UI
    //-----------------------------

    private void MostrarBotoes()
    {
        foreach (var botao in _botoes)
        {
            botao.SetActive(true);
        }
    }

    private void EsconderBotoes()
    {
        foreach (var botao in _botoes)
        {
            botao.SetActive(false);
        }
    }

    // No GameManager.cs, adicione isso:
    // Variável privada para armazenar as iniciais do jogador durante a sessão atual.
    // O uso de '_' no prefixo é uma convenção comum para campos privados.
    private string _playerInitials; // Variável para guardar as iniciais

    /// <summary>
    /// Método público para definir/salvar as iniciais do jogador.
    /// Pode ser chamado por outros scripts (ex: SceneGameOverManager).
    /// </summary>
    /// <param name="initials">Iniciais inseridas pelo jogador (3 letras)</param>
    public void SetPlayerInitials(string initials)
    {
    // Converte as iniciais para MAIÚSCULAS para padronização:
    // - Evita diferenças entre "aaa", "AAA", "Aaa"
    // - Melhora a consistência visual em placares
    _playerInitials = initials.ToUpper(); 
    
    // Debug opcional para verificação no Console:
    // - Útil durante desenvolvimento para garantir que o método está sendo chamado corretamente
    // - Pode ser removido na versão final do jogo
    Debug.Log("Iniciais salvas: " + _playerInitials); 
    
    // Exemplo de como persistir os dados entre sessões (descomente se necessário):
    // PlayerPrefs.SetString("PlayerInitials", _playerInitials); // Salva no sistema
    // PlayerPrefs.Save(); // Garante a escrita imediata
    // Observação: PlayerPrefs é ideal para dados simples, mas para sistemas complexos
    // (como placares com múltiplos registros), considere usar JSON ou um banco de dados.
    }

    /// <summary>
    /// Método público para recuperar as iniciais salvas.
    /// Permite que outros scripts acessem os dados sem manipular a variável diretamente.
    /// </summary>
    /// <returns>Iniciais em formato MAIÚSCULO ou string vazia se não definidas</returns>
    public string GetPlayerInitials()
    {
    // Retorna o valor armazenado (ou string vazia se nunca foi definido)
    return _playerInitials;
    
    // Versão alternativa com PlayerPrefs (caso tenha usado a persistência):
    // return PlayerPrefs.GetString("PlayerInitials", "AAA"); // "AAA" é valor padrão
    }
}
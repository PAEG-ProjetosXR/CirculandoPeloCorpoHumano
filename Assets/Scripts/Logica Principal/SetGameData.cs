using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;
using TMPro;
using System.Diagnostics;

public class SetGameData : MonoBehaviour
{
  [SerializeField] private string _standardDataPath;
  [SerializeField] public List<TextMeshProUGUI> _nomes;
  [SerializeField] public TextMeshProUGUI _equipe;
  [SerializeField] public TextMeshProUGUI _pontos;
  [SerializeField] public TextMeshProUGUI _tempo;

  private FirebaseFirestore _firestore;

  public void Start()
  {
    _firestore = FirebaseFirestore.DefaultInstance;
  }

  public void HandleSave()
  {
    string dataPath;
    List<string> arrayNomes = new List<string>();

    if (_nomes.Count > 0)
      foreach (TextMeshProUGUI textMeshProUGUI in _nomes) arrayNomes.Add(textMeshProUGUI.text);

    int pontos = int.Parse(_pontos.text);
    int tempo = int.Parse(_tempo.text);
    long currentTime = Stopwatch.GetTimestamp();

    for (int i = 0; i < arrayNomes.Count; i++)
    {
      dataPath = _equipe == null
        ? _standardDataPath + arrayNomes[i] + "-" + currentTime.ToString()
        : _standardDataPath + arrayNomes[i] + "-" + _equipe.text + "-" + currentTime.ToString();

      var gameData = new GameData
      {
        Nomes = arrayNomes.ToArray(),
        Pontos = pontos,
        Tempo = tempo,
        Equipe = _equipe.text
      };
      SaveToCloud(dataPath, gameData);
    }
  }
  private void SaveToCloud(string path, GameData data)
  {
    _firestore.Document(path).SetAsync(data);
  }
}
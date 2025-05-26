using UnityEngine;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SetGameData : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _standardDataPath;
  [SerializeField] public List<TextMeshProUGUI> _nomes;
  [SerializeField] public TextMeshProUGUI _equipe = "";
  [SerializeField] public TextMeshProUGUI _pontos = "0";
  [SerializeField] public TextMeshProUGUI _tempo = "0";

  private FirebaseFirestore _firestore;

  public void Start()
  {
    _firestore = FirebaseFirestore.DefaultInstance;
  }

  public void HandleSave()
  {
    var dataPath;
    var arrayNomes = _nomes.ToArray();
    int pontos = Int32.Parse(_pontos.text);
    int tempo = Int32.Parse(_tempo.text);

    string currentTime = GetTimestamp(DateTime.Now);

    for (int i = 0; i < arrayNomes.Length; i++)
    {
      dataPath = _equipe.text.Equals("")
        ? _standardDataPath.text + arrayNomes[i] + "-" + currentTime
        : _standardDataPath.text + arrayNomes[i] + "-" + _equipe.text + "-" + currentTime;

      var gameData = new GameData
      {
        Nomes = arrayNomes,
        Pontos = pontos,
        Tempo = tempo,
        Equipe = _equipe.text
      };
      SaveToCloud(dataPath, gameData)
    }
  }
  private void SaveToCloud(path, data)
  {
    _firestore.Document(path).SetAsync(data);
  }
}
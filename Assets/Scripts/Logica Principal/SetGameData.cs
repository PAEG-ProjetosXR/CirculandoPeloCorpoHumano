using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;
using TMPro;
using System.Diagnostics;
using System.Linq;

public class SetGameData : MonoBehaviour
{
  [SerializeField] public string _standardDataPath;
  private FirebaseFirestore _firestore;

  public void Start()
  {
    _firestore = FirebaseFirestore.DefaultInstance;
  }

  public void HandleSave(string[] nomes, string equipe, int pontos, int tempo)
  {
    string dataPath;
    long currentTime = Stopwatch.GetTimestamp();

    for (int i = 0; i < nomes.Length; i++)
    {
      dataPath = equipe.Equals("")
        ? _standardDataPath + nomes[i] + "-" + currentTime.ToString()
        : _standardDataPath + nomes[i] + "-" + equipe + "-" + currentTime.ToString();

      var gameData = new GameData
      {
        Nomes = nomes,
        Pontos = pontos,
        Tempo = tempo,
        Equipe = equipe
      };
      SaveToCloud(dataPath, gameData);
    }
  }

  private void SaveToCloud(string path, GameData data)
  {
    _firestore.Document(path).SetAsync(data);
  }
}
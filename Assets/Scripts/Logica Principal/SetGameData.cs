using Firebase.Firestore;
using System;
using System.Diagnostics;

public class SetGameData
{
  private string _standardDataPath;
  private FirebaseFirestore _firestore;

  public SetGameData()
  {
    Firestore = FirebaseFirestore.DefaultInstance;
    StandardDataPath = DateTime.Today.ToString("d").Replace("/", "-") + "/";
  }

  public string StandardDataPath
  {
    get { return _standardDataPath; }
    set
    {
      _standardDataPath = value;
    }
  }

  public FirebaseFirestore Firestore
  {
    get { return _firestore; }
    set
    {
      _firestore = value;
    }
  }

  public void HandleSave(string[] nomes, string equipe, int pontos, int tempo)
  {
    long currentTime = Stopwatch.GetTimestamp();
    for (int i = 0; i < nomes.Length; i++)
    {
      string dataPath = equipe.Equals("")
    ? StandardDataPath + nomes[i] + "-" + currentTime.ToString()
    : StandardDataPath + nomes[i] + "-" + equipe + "-" + currentTime.ToString();

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
using Firebase.Firestore;
using System;
using System.Diagnostics;

public class SetGameData
{
  private FirebaseFirestore _firestore;
  private string _nomeCollection;
  private string _codigoSessao;

#nullable enable
  public SetGameData()
  {
    CodigoSessao = "";
    Firestore = FirebaseFirestore.DefaultInstance;
    NomeCollection = $"{DateTime.Today.ToString("d").Replace("/", "-")}";
  }

  public string NomeCollection
  {
    get { return _nomeCollection; }
    set
    {
      _nomeCollection = value;
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

  public string CodigoSessao
  {
    get { return _codigoSessao; }
    set
    {
      _codigoSessao = value;
    }
  }

  public void HandleSave(string[] nomes, string equipe, int pontos, int tempo)
  {
    long currentTime = Stopwatch.GetTimestamp();
    for (int i = 0; i < nomes.Length; i++)
    {
      string dataPath = equipe.Equals("")
    ? $"{NomeCollection}-{CodigoSessao}/{nomes[i]}-{currentTime}"
    : $"{NomeCollection}-{CodigoSessao}/{nomes[i]}-{equipe}-{currentTime}";

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
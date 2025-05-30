using Firebase.Firestore;
using System;
using System.Collections.Generic;

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

  public void HandleSave(string[] nomes, string equipe, int pontos, int tempo, string timestamp)
  {
    for (int i = 0; i < nomes.Length; i++)
    {
      string _dataPath = equipe.Equals("")
    ? $"{NomeCollection}-{CodigoSessao}/{nomes[i]}-{timestamp}"
    : $"{NomeCollection}-{CodigoSessao}/{nomes[i]}-{equipe}-{timestamp}";

      var _gameData = new GameData
      {
        Nomes = nomes,
        Pontos = pontos,
        Tempo = tempo,
        Equipe = equipe
      };
      SaveToCloud(_dataPath, _gameData);
    }
  }

  public List<GameData> HandleLoad(string[] nomes, string equipe, string timestamp)
  {
    List<GameData> _documentsFound = new();
    for (int i = 0; i < nomes.Length; i++)
    {
      string _dataPath = equipe.Equals("")
      ? $"{NomeCollection}-{CodigoSessao}/{nomes[i]}-{timestamp}"
      : $"{NomeCollection}-{CodigoSessao}/{nomes[i]}-{equipe}-{timestamp}";

      _documentsFound.Add(LoadFromCloud(_dataPath));
    }
    return _documentsFound;
  }
  private void SaveToCloud(string path, GameData data)
  {
    _firestore.Document(path).SetAsync(data);
  }

  private GameData LoadFromCloud(string path)
  {
    GameData loadedData = new GameData();
    _firestore.Document(path).GetSnapshotAsync().ContinueWith(task =>
    {
      if (task.Result.Exists)
      {
        loadedData = task.Result.ConvertTo<GameData>();
      }
    });
    return loadedData;
  }
}
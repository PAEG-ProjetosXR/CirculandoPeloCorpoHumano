using Firebase.Firestore;

public class SetGameData
{
  private FirebaseFirestore _firestore;
  private string _nomeCollection;

#nullable enable
  public SetGameData()
  {
    Firestore = FirebaseFirestore.DefaultInstance;
    NomeCollection = "";
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

  public void HandleUpdate(string path, GameData data)
  {
    GameData _dadoSalvoAtual = LoadFromCloud(path);
    if (data.Nomes.Length == 0) data.Nomes = _dadoSalvoAtual.Nomes;
    if (data.Equipe.Equals("")) data.Equipe = _dadoSalvoAtual.Equipe;
    SaveToCloud(path, data);
  }

  public void SaveToCloud(string path, GameData data)
  {
    _firestore.Document(path).SetAsync(data);
  }

  public GameData LoadFromCloud(string path)
  {
    GameData _loadedData = new GameData();
    _firestore.Document(path).GetSnapshotAsync().ContinueWith(task =>
    {
      if (task.Result.Exists)
      {
        _loadedData = task.Result.ConvertTo<GameData>();
      }
    });
    return _loadedData;
  }
}
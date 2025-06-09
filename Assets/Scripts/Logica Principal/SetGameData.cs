using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;

public class SetGameData
{
  private FirebaseFirestore _firestore;
  private string _nomeCollection;

#nullable enable
  public SetGameData()
  {
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        var dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available) {
            Firestore = FirebaseFirestore.DefaultInstance;
            Debug.Log("Firebase pronto!");
        } else {
            Debug.LogError($"Firebase não disponível: {dependencyStatus}");
        }
    });
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

  public async Task HandleUpdate(string path, GameData data)
  {
    GameData _dadoSalvoAtual = await LoadFromCloud(path);
    if (data.Nomes.Length == 0) data.Nomes = _dadoSalvoAtual.Nomes;
    if (data.Equipe.Equals("")) data.Equipe = _dadoSalvoAtual.Equipe;
    await SaveToCloud(path, data);
  }

  public async Task SaveToCloud(string path, GameData data)
  {
    await _firestore.Document(path).SetAsync(data);
  }

  public async Task<IEnumerable<DocumentSnapshot>> LoadDocumentsFromCollectionFromCloud(string path)
  {
    CollectionReference _collection = _firestore.Collection(path);
    QuerySnapshot _collectionSnapshot = await _collection.GetSnapshotAsync();
    return _collectionSnapshot.Documents;
  }

  public async Task<GameData> LoadFromCloud(string path)
  {
    GameData _loadedData = new GameData();
    await _firestore.Document(path).GetSnapshotAsync().ContinueWithOnMainThread(task =>
    {
      if (task.Result.Exists)
      {
        _loadedData = task.Result.ConvertTo<GameData>();
      }
    });
    return _loadedData;
  }
}
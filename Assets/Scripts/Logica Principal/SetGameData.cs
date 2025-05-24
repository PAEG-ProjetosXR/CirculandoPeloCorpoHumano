using UnityEngine;
using Firebase.Firestore;

public class SetGameData : MonoBehaviour
{
  [SerializeField] private string _dataPath = "game_sheet/game01";

  private string _nome = "bruno";
  private int _pontos = 180;
  private int _tempo = 200;

  public void SaveToCloud()
  {
    var gameData = new GameData
    {
      Nome = _nome,
      Pontos = _pontos,
      Tempo = _tempo
    };
    var firestore = FirebaseFirestore.DefaultInstance;
    firestore.Document(_dataPath).SetAsync(gameData);
  }
}
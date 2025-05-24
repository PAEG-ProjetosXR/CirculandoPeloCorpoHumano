using Firebase.Firestore;

[FirestoreData]
public class GameData
{
  [FirestoreProperty]
  public string Nome { get; set; }
  [FirestoreProperty]
  public int Pontos { get; set; }
  [FirestoreProperty]
  public int Tempo { get; set; }
}
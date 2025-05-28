using Firebase.Firestore;

[FirestoreData]
public class GameData
{
  [FirestoreProperty]
  public string[] Nomes { get; set; }
  [FirestoreProperty]
  public int Pontos { get; set; }
  [FirestoreProperty]
  public int Tempo { get; set; }
  [FirestoreProperty]
  public string Equipe { get; set; }
}
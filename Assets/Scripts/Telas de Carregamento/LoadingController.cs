using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
  [SerializeField] public float _tempoDuracao;
  [SerializeField] public int _proximaCena;

  public void Start()
  {
    StartCoroutine(CarregarProximaCena());
  }

  private IEnumerator CarregarProximaCena()
  {
    yield return new WaitForSeconds(_tempoDuracao);

    SceneManager.LoadScene(_proximaCena);
  }

}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneGameOverManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _timerGameText;
  [SerializeField] private TextMeshProUGUI _scoreGameText;

  private void Start()
  {
    UpdateGameOverUI();
  }

  private void UpdateGameOverUI()
  {
    if (GameManager.Instance == null)
    {
      Debug.LogError("GameManager não encontrado!");
      return;
    }

    int totalScore = GameManager.Instance.GetPontos();
    _scoreGameText.text = $"PONTOS: {totalScore}";

    if (_timerGameText != null)
    {
      float totalTimeSpent = GameManager.Instance.GetTotalTime();
      _timerGameText.text = $"TEMPO: {Mathf.FloorToInt(totalTimeSpent)}";
    }
    else
    {
      Debug.LogError("TimerGameText não atribuído no Inspector!");
    }
  }

  public void ReturnToMenu()
  {
    StartCoroutine(LoadMenuAfterDelay());
  }

  private IEnumerator LoadMenuAfterDelay()
  {
    SceneManager.LoadScene(7);
    yield return new WaitForSeconds(3f);
    SceneManager.LoadScene(0);
  }
}
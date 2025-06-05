using UnityEngine;
#if UNITY_EDITOR
  using UnityEditor;
#endif

[CreateAssetMenu]
public class StringArrayScriptableObject : ScriptableObject
{
  [SerializeField]
  private string[] _value;
  public string[] Value
  {
    get { return _value; }
    set
    {
      _value = value;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
  }
}

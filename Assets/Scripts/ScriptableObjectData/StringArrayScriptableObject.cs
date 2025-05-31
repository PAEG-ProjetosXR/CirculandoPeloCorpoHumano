using UnityEngine;

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
      UnityEditor.EditorUtility.SetDirty(this);
    }
  }
}

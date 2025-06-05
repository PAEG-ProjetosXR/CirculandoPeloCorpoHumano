using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class StringScriptableObject : ScriptableObject
{
  [SerializeField]
  private string _value;
  public string Value
  {
    get { return _value; }
    set
    {
      _value = value;
      EditorUtility.SetDirty(this);
    }
  }
}

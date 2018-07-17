using UnityEngine;


[CreateAssetMenu(menuName = "Variables/StringVariable", order = 2)]
public class StringVariable : ScriptableObject
{
    [SerializeField]
    private string value = "";

    public string Value
    {
        get { return value; }
        set { this.value = value; }
    }

    public static implicit operator string(StringVariable stringVariable)
    {
        return stringVariable.Value;
    }


}

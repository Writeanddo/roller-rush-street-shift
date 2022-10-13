using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class levelSelectedSO : ScriptableObject
{
    private string _levelName = "";

    public string levelName
    {
        get { return _levelName; }
        set { _levelName = value; }
    }
}
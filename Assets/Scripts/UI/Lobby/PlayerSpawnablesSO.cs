using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSpawnablesSO : ScriptableObject
{
    private List<PlayerSpawnable> _playerSpawnables = new List<PlayerSpawnable>();

    public List<PlayerSpawnable> PlayerSpawnables
    {
        get { return _playerSpawnables ; }
        set { _playerSpawnables = value; }
    }
}

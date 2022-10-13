using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PlayersRankingSO : ScriptableObject
{
    private List<PlayerData> _playerDatas = new List<PlayerData>();
    private List<Material> _playerMaterials = new List<Material>();

    public List<PlayerData> PlayerDatas
    {
        get { return _playerDatas; }
        set { _playerDatas= value; }
    }

    public List<Material> PlayerMaterials
    {
        get { return _playerMaterials; }
        set { _playerMaterials = value; }
    }

    public void Add(PlayerData playerData, Material playerMaterial)
    {
        _playerDatas.Add(playerData);
        _playerMaterials.Add(playerMaterial);
    }

    public void Remove(PlayerData playerData, Material playerMaterial)
    {
        _playerDatas.Remove(playerData);
        _playerMaterials.Remove(playerMaterial);
    }

    public void Clear()
    {
        _playerDatas.Clear();
        _playerMaterials.Clear();
    }
}
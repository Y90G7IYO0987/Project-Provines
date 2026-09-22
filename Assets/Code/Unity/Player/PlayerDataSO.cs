using ProjectProvines.Core.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public GameObject Prefab;
    public PlayerUtils PlayerUtils;
}
using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/New Palette", order = 0)]
public class PaletteSO : ScriptableObject
{
    [field: SerializeField] public Color[] LevelColors { get; private set; }
    [field: SerializeField] public Color[] LevelOutlineColors { get; private set; }
}

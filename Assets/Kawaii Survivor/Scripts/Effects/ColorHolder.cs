using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    public static ColorHolder instance;


    [Header(" Elements ")]
    [SerializeField] private PaletteSO pallete;

    private void Awake()
    {
        if(instance == null) 
            instance = this;
        else
            Destroy(gameObject);
    }
    
    public static Color GetColor(int level)
    {
        level = Mathf.Clamp(level, 0, instance.pallete.LevelColors.Length);
        return instance.pallete.LevelColors[level];
    }    
    
    public static Color GetOutlineColor(int level)
    {
        level = Mathf.Clamp(level, 0, instance.pallete.LevelOutlineColors.Length);
        return instance.pallete.LevelOutlineColors[level];
    }
}

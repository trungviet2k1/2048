using UnityEngine;

[System.Serializable]
public class TitleStyle
{
    public int Number;
    public Color32 TitleColor;
    public Color32 TextColor;
}

public class TitleStyleHolder : MonoBehaviour
{
    public static TitleStyleHolder Instance { get; set; }

    public TitleStyle[] TitleStyles;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
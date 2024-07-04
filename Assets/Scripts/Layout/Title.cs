using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public bool mergedThisTurn = false;
    public int indRow;
    public int indCol;

    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            if (number == 0)
            {
                SetEmpty();
            }
            else
            {
                ApplyStyle(number);
                SetVisible();
            }
        }
    }

    private int number;
    private TextMeshProUGUI TitleText;
    private Image TitleImage;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        TitleText = GetComponentInChildren<TextMeshProUGUI>();
        TitleImage = transform.Find("NumberedCell").GetComponent<Image>();
    }

    public void PlayMergerAnimation()
    {
        animator.SetTrigger("Merge");
    }

    public void PlayAppearAnimation()
    {
        animator.SetTrigger("Appear");
    }

    void ApplyStyleFromHolder(int index)
    {
        TitleText.text = TitleStyleHolder.Instance.TitleStyles[index].Number.ToString();
        TitleText.color = TitleStyleHolder.Instance.TitleStyles[index].TextColor;
        TitleImage.color = TitleStyleHolder.Instance.TitleStyles[index].TitleColor;
    }

    void ApplyStyle(int num)
    {
        switch (num)
        {
            case 2:
                ApplyStyleFromHolder(0);
                break;
            case 4:
                ApplyStyleFromHolder(1);
                break;
            case 8:
                ApplyStyleFromHolder(2);
                break;
            case 16:
                ApplyStyleFromHolder(3);
                break;
            case 32:
                ApplyStyleFromHolder(4);
                break;
            case 64:
                ApplyStyleFromHolder(5);
                break;
            case 128:
                ApplyStyleFromHolder(6);
                break;
            case 256:
                ApplyStyleFromHolder(7);
                break;
            case 512:
                ApplyStyleFromHolder(8);
                break;
            case 1024:
                ApplyStyleFromHolder(9);
                break;
            case 2048:
                ApplyStyleFromHolder(10);
                break;
            case 4096:
                ApplyStyleFromHolder(11);
                break;
            default:
                Debug.Log("Check the numbers that you pass to ApplyStyle!");
                break;
        }
    }

    private void SetVisible()
    {
        TitleImage.enabled = true;
        TitleText.enabled = true;
    }

    private void SetEmpty()
    {
        TitleImage.enabled = false;
        TitleText.enabled = false;
    }
}
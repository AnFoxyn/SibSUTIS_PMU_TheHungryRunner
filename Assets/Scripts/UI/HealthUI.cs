using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Heart Objects")]
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Sprite heartSprite;

    private List<Image> hearts = new List<Image>();

    public void SetMaxHearts(int maxHearts)
    {
        foreach(Image heart in hearts)
            Destroy(heart.gameObject);

        hearts.Clear();

        for(int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartPrefab, transform);
            newHeart.sprite = heartSprite;
            newHeart.color = Color.white;
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = heartSprite;
                hearts[i].color = Color.white;
            }
            else
            {
                hearts[i].sprite = heartSprite;
                hearts[i].color = Color.black;
            }
        }
    }
}

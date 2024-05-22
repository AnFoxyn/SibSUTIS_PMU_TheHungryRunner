using UnityEngine;
using UnityEngine.UI;


public class BackgroundRandomGenerator : MonoBehaviour
{
    [Header("Raw Image")]
    [SerializeField] private RawImage image;

    [Space]
    [Header("Backgrounds")]
    public Texture[] Textures;

    private int random;
    private float x, y;

    void Start()
    {
        random = Random.Range(0, Textures.Length);
        image.texture = Textures[random];

        x = Random.Range(-1.0f, 1.0f);
        y = Random.Range(-1.0f, 1.0f);
    }

    void FixedUpdate()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.deltaTime, image.uvRect.size);
    }
}

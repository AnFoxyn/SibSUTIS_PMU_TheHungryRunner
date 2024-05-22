using UnityEngine;

public class PlayerRandomChanger : MonoBehaviour
{
    [Header("Player Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private int maxPlayers = 4;
    private int random;

    void Awake()
    {
        random = Random.Range(0, maxPlayers);
        animator.SetFloat("setPlayer", random);
    }
}

using System;
using UnityEngine;

public class Fruit : MonoBehaviour, IItem
{
    [Header("Fruit Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private int maxFruits = 8;
    private int random;

    public static event Action<int> OnFruitCollect;

    private void Awake()
    {
        random = UnityEngine.Random.Range(0, maxFruits);
        animator.SetFloat("setFruit", random);
    }

    public void Collect()
    {
        OnFruitCollect.Invoke(1);
        animator.SetTrigger("isCollected");
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}

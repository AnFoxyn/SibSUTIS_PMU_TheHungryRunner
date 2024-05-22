using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Trampoline : MonoBehaviour
{
    [Header("Trampoline Config")]
    [SerializeField] private Animator animator;
    [SerializeField] private float bounceForce = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandlePlayerBounce(collision.gameObject);
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb)
        {
            animator.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}

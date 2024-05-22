using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IItem fruit = collision.GetComponent<IItem>();
        if(fruit != null)
            fruit.Collect();
    }
}

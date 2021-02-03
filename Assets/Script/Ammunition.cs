using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        Character_Controller player = other.GetComponent<Character_Controller>();
        if (player != null){
            player.ChangeAmmo(7);
            Destroy(gameObject);
        }
    }
}

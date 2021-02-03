using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo_Display : MonoBehaviour
{
    public GameObject character;
    int ammo;
    void Update(){
        ammo = character.GetComponent<Character_Controller>().currentAmmo;
        GetComponent<Text>().text = ("x " + ammo.ToString());
    }
}

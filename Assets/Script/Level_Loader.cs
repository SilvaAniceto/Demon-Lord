using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Level_Loader : MonoBehaviour
{
    public Animator transition;
    // Update is called once per frame
    void Update(){
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)){
                LoadNextLevel(1,1);
            }
        }
    }

    public void LoadNextLevel(int index, float time){
       StartCoroutine(LoadLevel(index,time));
    }

    IEnumerator LoadLevel(int levelIndex, float wait){
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(wait);

        SceneManager.LoadScene(levelIndex);
    }
}

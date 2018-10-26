using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour {

	public void OnButtonLoadLevel(string levelName)
    {
        StartCoroutine(LoadGame(levelName));
    }

    private IEnumerator LoadGame(string strLevel)
    {
        Debug.Log("Loading Level");
        yield return SceneManager.LoadSceneAsync(strLevel);
        Debug.Log("Level Load complete");
    }
}

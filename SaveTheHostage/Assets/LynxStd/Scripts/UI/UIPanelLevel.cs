using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPanelLevel : MonoBehaviour {

    public Level level;
    public Text levelText;

    void Start()
    {
        if (!level.Unlocked)
        {
            GetComponent<Image>().color = Color.red;
        }
    }

    public void OnButtonLoadLevel()
    {
        if (!level.Unlocked)
            return;

        StartCoroutine(LoadGame(level.levelName));
    }

    private IEnumerator LoadGame(string strLevel)
    {
        Debug.Log("Loading Level");
        yield return SceneManager.LoadSceneAsync(strLevel);
        Debug.Log("Level Load complete");
    }
}

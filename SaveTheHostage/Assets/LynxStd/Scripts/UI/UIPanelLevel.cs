using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPanelLevel : MonoBehaviour {

    public Level level;
    public Text levelText;
    public List<Image> stars = new List<Image>();

    void Start()
    {
        if (!level.Unlocked)
        {
            GetComponent<Image>().color = Color.red;
        }

        if(stars.Count > 0)
        {
            for (int x=0; x < level.findItem; x++)
            {
                stars[x].enabled = true;
            }
        }

        string lvlName = level.levelName;
        if(lvlName != "")
            levelText.text = lvlName.Substring(5);
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

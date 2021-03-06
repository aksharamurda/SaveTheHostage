﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour {

    public static UIMainMenu instance;
    private GameObject panelZoneUI;
    private GameObject panelLevelUI;
    private GameObject textCoin;

    public GameObject prefabLevel;
    private PlayerProfile playerProfile;
    void Awake()
    {
        instance = this;
        panelZoneUI = GameObject.Find("PanelZoneUI");
        panelLevelUI = GameObject.Find("PanelLevelUI");
        textCoin = GameObject.Find("TextCoinUI");

        PlayerData.CreatePlayerProfile();

        playerProfile = PlayerData.GetPlayerProfile();
    }

    void Start()
    {
        panelZoneUI.SetActive(true);
        panelLevelUI.SetActive(false);


        textCoin.GetComponent<Text>().text = playerProfile.playerCoin.ToString();
    }

    public void SwitchZoneLevel(List<Level> levels)
    {
        panelZoneUI.SetActive(panelLevelUI.activeInHierarchy);
        panelLevelUI.SetActive(!panelZoneUI.activeInHierarchy);

        foreach (Transform child in panelLevelUI.transform.GetChild(0))
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int x=0; x < levels.Count; x++)
        {
            GameObject uiLevel = Instantiate(prefabLevel);
            uiLevel.transform.SetParent(panelLevelUI.transform.GetChild(0));
            uiLevel.transform.localScale = Vector3.one;
            uiLevel.GetComponent<UIPanelLevel>().level = levels[x];
        }
    }

	public void OnButtonBackToMenu()
    {
        panelZoneUI.SetActive(panelLevelUI.activeInHierarchy);
        panelLevelUI.SetActive(!panelZoneUI.activeInHierarchy);
    }
}

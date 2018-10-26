using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelZone : MonoBehaviour {

    public ZoneSettings zoneSettings;
    public Text textItem;

    [HideInInspector]
    public Zone zone;
    

    void Awake()
    {
        ZoneData.CreateZoneData(zoneSettings.zone);

        if (ZoneData.GetZoneData(zoneSettings.zone.zoneName) != null)
            zone = (ZoneData.GetZoneData(zoneSettings.zone.zoneName));
    }

    void Start()
    {
        float totalItem = 0;
        foreach (Level lvl in zone.levels)
        {
            totalItem += lvl.findItem;
        }
        textItem.text = totalItem + "/30";

    }

    public void OnButtonEnterZone()
    {
        UIMainMenu.instance.SwitchZoneLevel(zone.levels);
    }
}

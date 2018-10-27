using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelZone : MonoBehaviour {

    public ZoneSettings zoneSettings;
    public Text textItem;

    //[HideInInspector]
    public Zone zone;
    

    void Awake()
    {
        ZoneData.CreateZoneData(zoneSettings.zone);
        if (ZoneData.GetZoneData(zoneSettings.zone.zoneName) != null)
            zone = (ZoneData.GetZoneData(zoneSettings.zone.zoneName));

        Init();
    }

    void Init()
    {

        if (zone == null)
            return;

        float totalItem = 0;
        foreach (Level lvl in zone.levels)
        {
            totalItem += lvl.findItem;
        }

        textItem.text = totalItem + "/30";

        string idZone = zone.zoneName;
        idZone = idZone.Substring(4);

        int id = 0;
        int.TryParse(idZone, out id);

        if(id > 1)
        {
            string prevZoneName = "Zone" + (id - 1);
            
            Zone prevZone = new Zone();

            
            if (ZoneData.GetZoneData(prevZoneName) != null)
                prevZone = (ZoneData.GetZoneData(prevZoneName));

            if (prevZone.MissionComplete)
                zone.Unlocked = true;

            ZoneData.UpdateZoneData(zone);
            zone = (ZoneData.GetZoneData(zoneSettings.zone.zoneName));
        }

        

        if (!zone.Unlocked)
            GetComponent<Image>().color = Color.red;


    }

    public void OnButtonEnterZone()
    {
        if (!zone.Unlocked)
            return;

        UIMainMenu.instance.SwitchZoneLevel(zone.levels);
    }
}

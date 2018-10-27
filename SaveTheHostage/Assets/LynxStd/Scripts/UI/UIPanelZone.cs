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

    private void Start()
    {
        if (zone == null)
            return;

        if (zone.zoneName != zoneSettings.keyZone)
            return;

        Init();
    }

    void Init()
    {

        float totalItem = 0;
        foreach (Level lvl in zone.levels)
        {
            totalItem += lvl.findItem;
        }

        textItem.text = totalItem + "/" + zone.itemGoal;

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

            if (prevZone.missionComplete)
                zone.unlockedZone = true;

            ZoneData.UpdateZoneData(zone);
            zone = (ZoneData.GetZoneData(zoneSettings.zone.zoneName));
        }

        

        if (zone.unlockedZone)
            GetComponent<Image>().color = Color.white;


    }

    public void OnButtonEnterZone()
    {
        if (zone.zoneName != zoneSettings.keyZone)
            return;

        if (!zone.unlockedZone)
            return;

        UIMainMenu.instance.SwitchZoneLevel(zone.levels);
    }
}

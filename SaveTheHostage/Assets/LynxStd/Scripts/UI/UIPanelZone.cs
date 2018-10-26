using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelZone : MonoBehaviour {

    public ZoneSettings zoneSettings;
    [HideInInspector]
    public Zone zone;
    

    void Awake()
    {
        ZoneData.CreateZoneData(zoneSettings.zone);

        if (ZoneData.GetZoneData(zoneSettings.zone.zoneName) != null)
            zone = (ZoneData.GetZoneData(zoneSettings.zone.zoneName));
    }

    public void OnButtonEnterZone()
    {
        ZoneData.UpdateZoneData(zone);

        if (ZoneData.GetZoneData(zone.zoneName) != null)
        zone = ZoneData.GetZoneData(zone.zoneName);
    }
}

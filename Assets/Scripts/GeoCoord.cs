using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeoCoord
{
    public float latitude;
    public float longitude;

    public static float GetDistance(GeoCoord p1, GeoCoord p2)
    {
        float radius = 6371f;
        float dLat = Mathf.Deg2Rad * (p2.latitude - p1.latitude);
        float dLon = Mathf.Deg2Rad * (p2.longitude - p1.longitude);

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2)
                  + Mathf.Cos(Mathf.Deg2Rad * p1.latitude) * Mathf.Cos(Mathf.Deg2Rad * p2.latitude)
                  * Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return radius * c;
    }
}

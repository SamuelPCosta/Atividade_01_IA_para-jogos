using UnityEngine;
using System.Collections.Generic;

public class AreaManager : MonoBehaviour
{
    public List<Area> areas;
    public Transform player;

    private List<Area> activeAreas = new List<Area>();
    private int maxActiveAreas = 4;

    void Update()
    {
        foreach (var area in areas)
        {
            if (area.isActive && Vector2.Distance(player.position, area.transform.position) > 10f)
            {
                area.Deactivate();
                activeAreas.Remove(area);
            }
        }

        foreach (var area in areas)
        {
            if (!area.isActive && Vector2.Distance(player.position, area.transform.position) < 5f)
            {
                area.Activate();
                activeAreas.Add(area);
                if (activeAreas.Count > maxActiveAreas)
                {
                    activeAreas[0].Deactivate();
                    activeAreas.RemoveAt(0);
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AreaManager : MonoBehaviour
{
    public List<Area> areas;
    public Transform player;
    public int maxActiveAreas = 4;

    void Start()
    {
        UpdateActiveAreas();
    }

    void Update()
    {
        UpdateActiveAreas();
    }

    void UpdateActiveAreas()
    {
        if (areas == null || areas.Count == 0 || player == null) return;

        Vector2 playerPos = player.position;

        // ordena todas as areas pela distância do player
        var ordered = areas
            .Where(a => a != null)
            .OrderBy(a => Vector2.Distance(playerPos, a.transform.position))
            .ToList();

        var toActivate = new HashSet<Area>(ordered.Take(maxActiveAreas));

        foreach (var area in areas)
        {
            bool shouldBeActive = toActivate.Contains(area);
            if (area.isActive != shouldBeActive)
            {
                if (shouldBeActive) area.Activate();
                else area.Deactivate();
            }
        }
    }
}

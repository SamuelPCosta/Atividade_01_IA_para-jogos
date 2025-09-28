using UnityEngine;
using System.Collections.Generic;

public class Area : MonoBehaviour
{
    public List<Area> neighbors;
    public bool isActive = true;

    public void Activate()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
   public static PortalManager instance;

    public int requiredEnergy = 5;
    private int currentEnergy = 0;

    public Portal portal;
    public DoorFade door;

    void Awake()
    {
        instance = this;
    }

    public void CollectEnergy()
    {
        currentEnergy++;

        Debug.Log("Energy collected: " + currentEnergy);

        if (currentEnergy >= requiredEnergy)
        {
            portal.ActivatePortal();
            door.FadeOut();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleInfo : MonoBehaviour
{
    public GameObject BatteryInfo, AerosolInfo, KitchenInfo, PizzaInfo;
    private bool BatteryInfoActive, AerosolInfoActive, KitchenInfoActive, PizzaInfoActive = false;

    public void setBatteryInfo()
    {
    	if (!BatteryInfoActive)
    	{
    		BatteryInfo.SetActive(true);
    		BatteryInfoActive = true;
    	}
    }

    public void setBatteryCard()
    {
        if (BatteryInfoActive)
        {
            BatteryInfo.SetActive(false);
            BatteryInfoActive = false;
        }
    }

    public void setAerosolInfo()
    {
    	if (!AerosolInfoActive)
    	{
    		AerosolInfo.SetActive(true);
    		AerosolInfoActive = true;
    	}
    }

    public void setAerosolCard()
    {
        if (AerosolInfoActive)
        {
            AerosolInfo.SetActive(false);
            AerosolInfoActive = false;
        }
    }

    public void setKitchenInfo()
    {
    	if (!KitchenInfoActive)
    	{
    		KitchenInfo.SetActive(true);
    		KitchenInfoActive = true;
    	}
    }

    public void setKitchenCard()
    {
        if (KitchenInfoActive)
        {
            KitchenInfo.SetActive(false);
            KitchenInfoActive = false;
        }
    }

    public void setPizzaInfo()
    {
    	if (!PizzaInfoActive)
    	{
    		PizzaInfo.SetActive(true);
    		PizzaInfoActive = true;
    	}
    }

    public void setPizzaCard()
    {
        if (PizzaInfoActive)
        {
            PizzaInfo.SetActive(false);
            PizzaInfoActive = false;
        }
    }
}

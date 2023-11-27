using UnityEngine;

public class TruckData
{
    public static float GetFuelData()
    {
        return PlayerPrefs.GetFloat("FuelLevel", 1f);
    }


    public static void SetFuelData(float val)
    {
        PlayerPrefs.SetFloat("FuelLevel", val);
        CustomDebug.Log("Saved Fuel Data = " + PlayerPrefs.GetFloat("FuelLevel"));
    }
}
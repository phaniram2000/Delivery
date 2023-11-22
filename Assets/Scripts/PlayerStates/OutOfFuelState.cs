using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfFuelState : MonoBehaviour, ITruckState
{
    private Truck truck;

    public void EnterState(Truck truck)
    {
        this.truck = truck;
        // Implement actions when entering Idle state
    }

    public void UpdateState()
    {
        StopTruck();
//refuel
        Refuel();
    }

    public void Refuel()
    {
        if (Input.GetKey(KeyCode.F) && truck.FuelGage.value < 1)
        {
            truck.FuelGage.value += 1 * Time.deltaTime;
            truck.SliderColorDesider();
        }

        if (Input.GetKeyUp(KeyCode.F) &&truck.FuelGage.value > .3f)
        {
            var fuellevel = truck.FuelGage.value;
            TruckData.SetFuelData(fuellevel);
            truck.SwitchState(new DriveState());
        }
    }

    public void StopTruck()
    {
        truck.InvokeRepeating("DecelerateCar", .2f, 0.1f);
        truck.deceleratingCar = true;
    }

    public void ExitState()
    {
        // Implement actions when exiting Idle state
    }
}
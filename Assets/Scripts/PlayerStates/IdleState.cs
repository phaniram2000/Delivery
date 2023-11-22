using UnityEngine;

public class IdleState : MonoBehaviour, ITruckState
{
    private Truck truck;

    public void EnterState(Truck truck)
    {
        this.truck = truck;
        // Implement actions when entering Idle state
    }

    public void UpdateState()
    {
        truck.InvokeRepeating("DecelerateCar", 0f, 0.1f);
        truck.deceleratingCar = true;
    }

    public void ExitState()
    {
        
        // Implement actions when exiting Idle state
    }
}
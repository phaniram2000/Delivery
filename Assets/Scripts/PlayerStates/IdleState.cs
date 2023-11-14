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
        // Implement Idle state behavior
    }

    public void ExitState()
    {
        // Implement actions when exiting Idle state
    }
}
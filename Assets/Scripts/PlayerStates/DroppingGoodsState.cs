using UnityEngine;

public class DroppingGoodsState : MonoBehaviour, ITruckState
{
    private Truck truck;

    public void EnterState(Truck truck)
    {
        this.truck = truck;
    }

    public void UpdateState()
    {
        truck.InvokeRepeating("DecelerateCar", 0f, 0.1f);
        truck.deceleratingCar = true;
    }

    public void ExitState()
    {
        // Implement actions when exiting DroppingGoods state
    }
}
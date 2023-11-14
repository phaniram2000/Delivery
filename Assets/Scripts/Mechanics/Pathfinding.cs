using System;
using Dreamteck.Splines.Primitives;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
   public Transform Destinationobject;
   public LineRenderer LineRenderer;
   private NavMeshAgent m_Agenet;
   private NavMeshPath M_navMeshPath;

   private void Start()
   {
      m_Agenet = GetComponent<NavMeshAgent>();
      M_navMeshPath = new NavMeshPath();
   }

   private void Update()
   {
      if (M_navMeshPath != null)
         M_navMeshPath.ClearCorners();
      m_Agenet.CalculatePath(Destinationobject.transform.position, M_navMeshPath);
      LineRenderer.positionCount = M_navMeshPath.corners.Length;
      LineRenderer.SetPositions(M_navMeshPath.corners);
   }
}

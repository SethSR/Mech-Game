using UnityEngine;
using System;

[Serializable]
public abstract class SteeringBehavior {
  public abstract Vector3 Force { get; }
}
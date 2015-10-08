using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ActionDecider {
  public string              actionName;
  public SteeringBehavior    behavior;
  public List<Consideration> considerations;
}
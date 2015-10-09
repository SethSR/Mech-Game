using UnityEngine;
using System.Collections.Generic;

public class ActionDecider {
  public bool                enable = true;
  public string              actionName;
  public SteeringBehavior    behavior;
  public List<Consideration> considerations;
}
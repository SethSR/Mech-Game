using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ActionDecider {
  public string               actionName;
  public SteeringBehaviorType behaviorType;
  public List<Consideration>  considerations;

  public SteeringBehavior Behavior {
  	get {
  		switch (behaviorType) {
  			case SteeringBehaviorType.Wander:
  				return new Wander();
  			case SteeringBehaviorType.Seek:
  				return new Seek();
  			default:
  				return null;
  		}
  	}
  }
}

public enum SteeringBehaviorType {
	Wander,
	Seek,
}
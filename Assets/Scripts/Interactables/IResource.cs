using UnityEngine;
using System.Collections;

public class IResource : Interactable {
	public void use() {}
}

public enum ResourceType {
	Scrap,
	Metal,
	Crystal,
}
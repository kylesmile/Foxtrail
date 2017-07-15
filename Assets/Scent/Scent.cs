using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Scent : Smellable {
	public float despawnTime = 5.0f;
	private float creationFrame = 0.0f;

	// Use this for initialization
	void Start () {
		creationFrame = Time.frameCount;
		Destroy (gameObject, despawnTime);
	}

	public override float ScentStrength () {
		return creationFrame;
	}

	[DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
	static void DrawScentLocation (Scent scent, GizmoType _gizmoType) {
		Gizmos.DrawWireSphere (scent.transform.position, 0.5f);
	}
}

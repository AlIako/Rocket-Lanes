using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Obstacle), true)]
public class ColliderScalerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();

		Obstacle obstacle = (Obstacle)target;
		obstacle.AdaptColliderToSprite();
	}
}
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SnapSpriteScale), true)]
public class SnapSpriteScaleEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();

		SnapSpriteScale snapSpriteScale = (SnapSpriteScale)target;
		snapSpriteScale.Snap();
	}
}
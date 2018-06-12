using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenLabelInteger))]

public class TweenLabelIntegerEditor : UITweenerEditor
{
	public override void OnInspectorGUI()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(120f);

		TweenLabelInteger tw = target as TweenLabelInteger;
		GUI.changed = false;

		int from = EditorGUILayout.IntField("From", tw.from);
		int to = EditorGUILayout.IntField("To", tw.to);
		bool bold = EditorGUILayout.Toggle("Bold", tw.bold);

		if (from < 0) from = 0;
		if (to < 0) to = 0;

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			tw.bold = bold;
			NGUITools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
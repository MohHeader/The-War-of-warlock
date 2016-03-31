using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using System.Collections.Generic;
using skill;


[CreateAssetMenu( fileName = "技能树-3-", menuName = "Wuxingogo/技能树-3-" )]
public class SkillTree : XScriptableObject
{
	public int skillID = 0;
	public int animationType;
	public List<CastStruct> castStruct = new List<CastStruct>();

	#if UNITY_EDITOR
//	[HideInInspector]
	[SerializeField]
	protected List<Rect> RectCollection = new List<Rect>();
	[SerializeField]
	public List<string> GuidCollection = new List<string>();

	public Rect FindRect(StructBase dataStruct)
	{
		int index = -1;
		for( int pos = 0; pos < GuidCollection.Count; pos++ ) {
			//  TODO loop in GuidCollection.Count
			if(GuidCollection[pos] != null && GuidCollection[pos] == (dataStruct.Guid))
			{
				index = pos;
				break;
			}
		}

		if(index == -1)
		{
			GuidCollection.Add(dataStruct.GenerateGUID());
			RectCollection.Add(new Rect(0,0, 100, 100));
			MarkDirty();

			return RectCollection[RectCollection.Count - 1];
		}
		return RectCollection[index];
	}

	public void UpdateRect(StructBase dataStruct, Rect targetRect)
	{
		int index = -1;
		for( int pos = 0; pos < GuidCollection.Count; pos++ ) {
			//  TODO loop in GuidCollection.Count
			if(GuidCollection[pos] != null && GuidCollection[pos] == (dataStruct.Guid))
			{
				index = pos;
				break;
			}
		}
		RectCollection[index] = targetRect;
		MarkDirty();
	}

	public void RemoveRect(StructBase dataStruct)
	{
		int index = -1;
		for( int pos = 0; pos < GuidCollection.Count; pos++ ) {
			//  TODO loop in GuidCollection.Count
			if( GuidCollection[pos] != null && GuidCollection[pos] == ( dataStruct.Guid ) ) {
				index = pos;
				break;
			}
		}
		if( index != -1 ) {
			RectCollection.RemoveAt( index );
			GuidCollection.RemoveAt( index );
			MarkDirty();
		}
	}

	public void MarkDirty()
	{
		UnityEditor.EditorUtility.SetDirty(this);
	}
	#endif
}

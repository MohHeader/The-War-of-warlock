//
//  SkillStruct.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;
using wuxingogo.Runtime;
using System.Collections.Generic;
using skill;



public class SkillStruct : XScriptableObject, ISkillEditorMarker
{
	#region ISkillEditorMarker implementation

	public StructBase GetMarker()
	{
		return marker;
	}

	#endregion
	/// <summary>
	/// The marker will be serialized.
	/// </summary>
	public StructBase marker = null;
	public enum SkillType
	{
		Target,
		NoTarget,
		Move
	}

	[Disable] 
	public SkillType type = SkillStruct.SkillType.Target;

	public virtual void Init(ISkillReleaser skillTree, ISkillCanBeTarget target)
	{
		
	}

	public virtual void Init(ISkillReleaser skillTree, ISkillCanBeTarget source, ISkillCanBeTarget dest)
	{
		
	}

	public virtual void Init(ISkillReleaser skillTree, Transform noTarget)
	{
		
	}

	public virtual void Init(ISkillReleaser skillTree, ITargetBinder bindPos)
	{
		
	}

	//	public virtual void Init(ISkillReleaser skillTree, GameObject emancipator, ITargetMovement bindPos, GameObject self = null, ISkillCanBeTarget target = null)
	//	{
	//
	//	}

	public virtual bool isNull {
		get {
			return false;
		}
	}


	public void AlignTransfrom(Transform lhs, Transform rhs)
	{
		lhs.position = rhs.position;
		lhs.rotation = rhs.rotation;
	}

	#if UNITY_EDITOR
	[HideInInspector]
	public Rect Bounds = new Rect( 0, 0, 100, 100 );
	#endif

}



﻿//
//  ISkillReleaser.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using UnityEngine;


namespace skill
{

	public interface ISkillReleaser
	{
		MonoBehaviour GetBehaviour();

		ISkillCanBeTarget GetSelfTarget();

		bool isFriend(ISkillCanBeTarget other);
	}
}

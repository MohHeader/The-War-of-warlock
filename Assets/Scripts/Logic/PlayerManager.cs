using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using wuxingogo.tools;

namespace Scripts.Node
	{
	public class PlayerManager : SingletonMBT<PlayerManager>
	{
		public List<Warlock> warlockSet = new List<Warlock>();

		public void AddNode(Warlock warlock)
		{
			if (!warlockSet.Contains(warlock))
			{
				warlockSet.Add(warlock);
			}
		}

		public void SynchRotation(Warlock warlock)
		{
			
		}
	}
	}

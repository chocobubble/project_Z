// Charcter Stat Class
// 1. Health
// 2. Attack
// 3. Defense
// 4. Act Speed
// 5. Exp
// 6. Level 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
	public class CharacterStat 
	{
		private int health = 0;
		private int attack = 0;
		private int defense = 0;
		private float actSpeed = 0.0f;
		private int exp = 0;
		private int level = 0;
		public int Health
		{
			get { return health; }
			set { health = value; }
		}
		public int Attack
		{
			get { return attack; }
			set { attack = value; }
		}
		public int Defense
		{
			get { return defense; }
			set { defense = value; }
		}
		public float ActSpeed
		{
			get { return actSpeed; }
			set { actSpeed = value; }
		}

		public CharacterStat()
		{
			health = 100;
			attack = 3;
			defense = 0;
			actSpeed = 10.0f;
			exp = 0;
			level = 0;
		}
	}
}

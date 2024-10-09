using UnityEngine;

namespace Character 
{
	public class CharacterStat 
	{
		private int level;
		private int exp = 0;
		private int HP;
		private int currentHP;
		private int attack;

		public CharacterStat(int level, int HP, int attack)
		{
			this.level = level;
			this.HP = HP;
			this.currentHP = HP;
			this.attack = attack;
		}

		public CharacterStat(CharacterStat other)
		{
			this.level = other.level;
			this.exp = other.exp;
			this.HP = other.HP;
			this.currentHP = other.currentHP;
			this.attack = other.attack;
		}
	}
}
using UnityEngine;

namespace Character 
{
	public struct CharacterStat 
	{
		public int Level;
		public int Exp;
		public int HP;
		public int CurrentHP;
		public int Attack;

		public CharacterStat(int level, int HP, int attack)
		{
			this.Level = level;
			this.Exp = 0;
			this.HP = HP;
			this.CurrentHP = HP;
			this.Attack = attack;
		}

		public CharacterStat(CharacterStat other)
		{
			this.Level = other.Level;
			this.Exp = other.Exp;
			this.HP = other.HP;
			this.CurrentHP = other.CurrentHP;
			this.Attack = other.Attack;
		}

		public override readonly string ToString()
		{
			return $"Level: {Level}, Exp: {Exp}, HP: {HP}, CurrentHP: {CurrentHP}, Attack: {Attack}";
		}
	}
}
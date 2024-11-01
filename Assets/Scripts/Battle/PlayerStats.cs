using System;
using UnityEngine;

namespace Battle
{
	public class PlayerStats  
	{
		private int heart;
		private int gold;
		private int turn;
		private int winStreak;

		public int Heart 
		{
			get { return heart; }
			set 
			{
				if (heart != value)
				{
					heart = value;
					OnHeartChanged?.Invoke(heart);
				}
			}
		}

		public int Gold 
		{
			get { return gold; }
			set 
			{
				if (gold != value)
				{
					gold = value;
					OnGoldChanged?.Invoke(gold);
				}
			}
		}

		public int Turn 
		{
			get { return turn; }
			set 
			{
				if (turn != value)
				{
					turn = value;
					OnTurnChanged?.Invoke(turn);
				}
			}
		}

		public int WinStreak 
		{
			get { return winStreak; }
			set 
			{
				if (winStreak != value)
				{
					winStreak = value;
					OnWinStreakChanged?.Invoke(winStreak);
				}
			}
		}

		public Action<int> OnHeartChanged;
		public Action<int> OnGoldChanged;
		public Action<int> OnTurnChanged;
		public Action<int> OnWinStreakChanged;

		public PlayerStats(int heart, int gold, int turn, int winStreak)
		{
			this.heart = heart;
			this.gold = gold;
			this.turn = turn;
			this.winStreak = winStreak;
		}
	}
}
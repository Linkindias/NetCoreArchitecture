namespace WebApplication1
{
	public class MarioGameState
	{
		private abMarioState currentMario;
		public EnumMario mario;
		public int scroe;

		public MarioGameState()
		{
			currentMario = new NormalMario(this);
		}

		public void ChangeMario(abMarioState changeMario)
		{
			this.currentMario = changeMario;
			if (changeMario is FireMario) mario = EnumMario.fire;
			if (changeMario is NormalMario) mario = EnumMario.normal;
			if (changeMario is SuperMario) mario = EnumMario.super;
			if (changeMario is DeadMario) mario = EnumMario.dead;
		}

		public void GetSunFlower()
		{
			currentMario.GetSunFlower();
		}

		public void GetHarm()
		{
			currentMario.GetHarm();
		}

		public void GetMushroom()
		{
			currentMario.GetMushroom();
		}

		public void SetRevive()
		{
			currentMario.SetRevive();
		}
	}
}
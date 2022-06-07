namespace WebApplication1
{
	public class MarioGameState
	{
		private abMarioState currentMario;
		public EnumMario mario
		{
			get => currentMario.mario;
		}

		public int scroe
		{
			get => currentMario.scroe;
		}

		public MarioGameState()
		{
			currentMario = new NormalMario(this);
		}

		public void ChangeMario(abMarioState changeMario)
		{
			this.currentMario = changeMario;
			if (changeMario is FireMario) currentMario.mario = EnumMario.fire;
			if (changeMario is NormalMario) currentMario.mario = EnumMario.normal;
			if (changeMario is SuperMario) currentMario.mario = EnumMario.super;
			if (changeMario is DeadMario) currentMario.mario = EnumMario.dead;
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
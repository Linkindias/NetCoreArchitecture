namespace WebApplication1
{
	public class MarioGame
	{
		public EnumMario _mario;
		public int scroe;

		public MarioGame(EnumMario mario)
		{
			_mario = mario;
		}

		public void GetSunFlower()
		{
			if (_mario == EnumMario.normal || _mario == EnumMario.super)
			{
				scroe += 100;
				_mario = EnumMario.fire;
			}
		}

		public void GetHarm()
		{
			scroe -= 100;

			if (_mario == EnumMario.fire)
				_mario = EnumMario.super;
			else if (_mario == EnumMario.super)
				_mario = EnumMario.normal;
			else if (_mario == EnumMario.normal)
				_mario = EnumMario.dead;
		}

		public void GetMushroom()
		{
			if (_mario == EnumMario.normal)
			{
				scroe += 100;
				_mario = EnumMario.super;
			}
		}

		public void SetRevive()
		{
			if (_mario == EnumMario.dead)
			{
				scroe = 0;
				_mario = EnumMario.normal;
			}
		}
	}
}
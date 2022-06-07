namespace WebApplication1
{
	public abstract class abMarioState
	{
		protected MarioGameState mg;

		public  EnumMario mario;
		public  int scroe;

		protected abMarioState(MarioGameState mg) => this.mg = mg;

		public abstract void GetSunFlower();
		public abstract void GetMushroom();
		public abstract void GetHarm();
		public abstract void SetRevive();
	}
}
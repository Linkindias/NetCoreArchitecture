namespace WebApplication1
{
	public abstract class abMarioState
	{
		protected MarioGameState mg;

		protected abMarioState(MarioGameState mg) => this.mg = mg;

		public abstract void GetSunFlower();
		public abstract void GetMushroom();
		public abstract void GetHarm();
		public abstract void SetRevive();
	}
}
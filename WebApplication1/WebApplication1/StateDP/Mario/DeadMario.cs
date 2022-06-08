namespace WebApplication1
{
	public class DeadMario:abMarioState
	{

		public DeadMario(MarioGameState mg ):base(mg)
		{
			
		}
		public override void GetSunFlower()
		{
		}

		public override void GetMushroom()
		{
		}

		public override void GetHarm()
		{
		}

		public override void SetRevive()
		{
			base.mg.ChangeMario(new NormalMario(base.mg));
			base.mg.scroe = 0;
		}
	}
}
namespace WebApplication1
{
	public class FireMario:abMarioState
	{
		public FireMario(MarioGameState mg):base(mg)
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
			base.mg.ChangeMario(new SuperMario(base.mg));
			base.mg.scroe -= 100;
		}

		public override void SetRevive()
		{
		}
	}
}
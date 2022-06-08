namespace WebApplication1
{
	public class SuperMario: abMarioState
	{
		public SuperMario(MarioGameState mg) : base(mg)
		{
			
		}
		public override void GetSunFlower()
		{
			base.mg.ChangeMario(new FireMario(base.mg));
			base.mg.scroe += 100;
		}

		public override void GetMushroom()
		{
		}

		public override void GetHarm()
		{
			base.mg.ChangeMario(new NormalMario(base.mg));
			base.mg.scroe -= 100;
		}

		public override void SetRevive()
		{
		}
	}
}
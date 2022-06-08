namespace WebApplication1
{
	public class NormalMario : abMarioState
	{
		public NormalMario(MarioGameState mg) : base(mg)
		{
		}

		public override void GetSunFlower()
		{
			base.mg.ChangeMario(new FireMario(base.mg));
			base.mg.scroe += 100;
		}

		public override void GetMushroom()
		{
			base.mg.ChangeMario(new SuperMario(base.mg));
			base.mg.scroe += 100;
		}

		public override void GetHarm()
		{
			base.mg.ChangeMario(new DeadMario(base.mg));
			base.mg.scroe -= 100;
		}

		public override void SetRevive()
		{
		}
	}
}
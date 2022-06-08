namespace WebApplication1
{
	public class CandyMachine
	{
		private int _countCandy = 0;
		private EnumCandyType currentType = EnumCandyType.noCoin;

		public CandyMachine(int countCandy)
		{
			_countCandy = countCandy;
		}

		public string castCoin(EnumCandyType type)
		{
			if (type == EnumCandyType.hasCoin)
			{
				if (_countCandy > 0)
					return changeBar(type);
				else
					return rejectCoin(type);
			}
			return "";
		}

		public string rejectCoin(EnumCandyType type)
		{
			if (type == EnumCandyType.hasCoin)
			{
				if (_countCandy <= 0) return "無糖果";
			}
			return "";
		}

		public string changeBar(EnumCandyType type)
		{
			if (type == EnumCandyType.hasCoin)
				return sendCandy(type);
			return "";
		}

		public string sendCandy(EnumCandyType type)
		{
			if (type == EnumCandyType.hasCoin)
			{
				if (_countCandy > 0)
					return "收到糖果";
				else
					return "糖果賣完";
			}
			return "";
		}
	}
}
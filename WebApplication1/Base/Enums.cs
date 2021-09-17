namespace Base
{
    public class Enums
    {
	    public enum DataStatus
	    {
		    Disable = 0,
            Enable = 1,
            Lock = 2,
	    }

	    public enum Sex
	    {
            Male = 1,
            Female = 2
	    }

        public enum OperateEvent
        {
            Login = 1, //登入
            Logout = 2, //登出
            Add = 3, //新增
            Update = 4, //修改
            View = 5, //檢視
        }

        public enum IsolationLevel
        {
            //https://docs.microsoft.com/zh-tw/dotnet/api/system.transactions.isolationlevel?view=netcore-3.1
            Serializable, //在交易期間可以讀取 Volatile 資料，但無法修改該資料，且不能加入新資料。
            RepeatableRead, // 在交易期間可以讀取 Volatile 資料，但無法修改該資料。 在交易期間可以加入新資料
            ReadCommitted, // 在交易期間無法讀取 Volatile 資料，但可以修改該資料
            ReadUncommitted, // 在交易期間可以讀取和修改 Volatile 資料。
            Snapshot, // 可以讀取 Volatile 資料。 交易會在修改資料之前，先驗證在最初讀取資料後是否有另一個交易已變更該資料。 如果資料已更新，則會引發錯誤， 如此可允許交易回到先前所認可的資料值。
            //當您升級使用 Snapshot 隔離層級建立的交易時，會擲回具有錯誤訊息「無法升級 IsolationLevel 為快照的交易」的 InvalidOperationException。
            Chaos,//無法覆寫來自隔離程度更深之交易的暫止變更
            Unspecified, //使用與指定不同的隔離等級，但無法判斷該等級。 如果設定這個值，會擲回例外狀況
        }

        public enum CacheStatus
        {
            Sliding, //滑動
            Absolute, //絕對
        }

    }
}
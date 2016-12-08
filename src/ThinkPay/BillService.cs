
namespace ThinkPay
{
    public static class BillService
    {
        /// <summary>
        /// 表示当前的单据服务
        /// </summary>
        public static IBillService Current { get; private set; }

        /// <summary>
        /// 设置单据服务的处理程序
        /// </summary>
        public static void SetServiceProvider(BillServiceProvider newProvider)
        {
            Current = newProvider.Invoke();
        }
    }
}

using System;
using System.Threading;
using Microsoft.Practices.ServiceLocation;

namespace ThinkPay
{
    public sealed class BillService : IBillService
    {
        public readonly static BillService Instance = new BillService();

        private BillService()
        { }


        public static void SetServiceProvider(Func<IBillService> provider)
        {

            try {
                _cacheService = provider.Invoke();
            }
            catch (Exception) {
                throw;
            }
        }

        private IBillService GetService()
        {
            var instance = ServiceLocator.Current.GetInstance<IBillService>();
            if (instance == null) {
                throw new SystemException("Not found the implementation class of IBillService.");
            }

            return instance;
        }

        private static IBillService _cacheService = null;
        private IBillService CacheService
        {
            get
            {
                if (_cacheService != null)
                    return _cacheService;

                return Interlocked.CompareExchange<IBillService>(ref _cacheService, GetService(), null);
            }
        }

        public IPayment GetPaymentInfo(string paymentNo)
        {
            return CacheService.GetPaymentInfo(paymentNo);
        }

        public IRefund GetRefundInfo(string refundNo)
        {
            return CacheService.GetRefundInfo(refundNo);
        }

        public bool PaymentNofity(IPaymentNotify reply)
        {
            return CacheService.PaymentNofity(reply);
        }

        public bool RefundNotify(IRefundNotify reply)
        {
            return CacheService.RefundNotify(reply);
        }
    }
}

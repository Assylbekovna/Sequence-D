using System;
using System.Threading.Tasks;
using OnlineStoreSequence.Models;

namespace OnlineStoreSequence.Services
{
    public class PaymentResult
    {
        public bool IsSuccess { get; }
        public string TransactionId { get; }
        public string ErrorMessage { get; }
        
        public PaymentResult(bool isSuccess, string transactionId = null, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            TransactionId = transactionId;
            ErrorMessage = errorMessage;
        }
    }
    
    public class PaymentSystem
    {
        private Random _random;
        
        public PaymentSystem()
        {
            _random = new Random();
        }
        
        public async Task<PaymentResult> ProcessPaymentAsync(Order order, string cardNumber, string cardHolder, string expiryDate, string cvv)
        {
            Console.WriteLine($"   [PaymentSystem.ProcessPaymentAsync()] Начинаем обработку платежа");
            Console.WriteLine($"   [PaymentSystem] Заказ #{order.Id}, Сумма: {order.TotalAmount:C}");
            Console.WriteLine($"   [PaymentSystem] Проверяем данные карты...");
            
      
            await Task.Delay(1500);
            
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 16)
            {
                Console.WriteLine($"   [PaymentSystem]  Ошибка: Неверный номер карты");
                return new PaymentResult(false, errorMessage: "Неверный номер карты");
            }
            
            if (string.IsNullOrEmpty(cvv) || cvv.Length != 3)
            {
                Console.WriteLine($"   [PaymentSystem]  Ошибка: Неверный CVV код");
                return new PaymentResult(false, errorMessage: "Неверный CVV код");
            }
            
            
            bool isSuccess = _random.Next(100) < 80; 
            
            if (isSuccess)
            {
                var transactionId = $"TXN{_random.Next(100000, 999999)}";
                Console.WriteLine($"   [PaymentSystem]  Платеж успешно обработан");
                Console.WriteLine($"   [PaymentSystem] ID транзакции: {transactionId}");
                return new PaymentResult(true, transactionId);
            }
            else
            {
                Console.WriteLine($"   [PaymentSystem]  Платеж отклонен банком");
                return new PaymentResult(false, errorMessage: "Недостаточно средств на карте");
            }
        }
        
        public async Task<bool> RefundPaymentAsync(string transactionId, decimal amount)
        {
            Console.WriteLine($"   [PaymentSystem.RefundPaymentAsync()] Возврат средств");
            Console.WriteLine($"   [PaymentSystem] Транзакция: {transactionId}, Сумма: {amount:C}");
            
            await Task.Delay(1000);
            
            bool isSuccess = _random.Next(100) < 90; 
            Console.WriteLine($"   [PaymentSystem] {(isSuccess ? "✅" : "❌")} Возврат {(isSuccess ? "успешен" : "не удался")}");
            
            return isSuccess;
        }
    }
}

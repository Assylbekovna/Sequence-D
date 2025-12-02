using System;
using System.Threading.Tasks;
using OnlineStoreSequence.Models;

namespace OnlineStoreSequence.Services
{
    public class DeliveryResult
    {
        public bool IsScheduled { get; }
        public string TrackingNumber { get; }
        public DateTime EstimatedDelivery { get; }
        public string ErrorMessage { get; }
        
        public DeliveryResult(bool isScheduled, string trackingNumber = null, DateTime? estimatedDelivery = null, string errorMessage = null)
        {
            IsScheduled = isScheduled;
            TrackingNumber = trackingNumber;
            EstimatedDelivery = estimatedDelivery ?? DateTime.Now.AddDays(3);
            ErrorMessage = errorMessage;
        }
    }
    
    public class DeliverySystem
    {
        private Random _random;
        
        public DeliverySystem()
        {
            _random = new Random();
        }
        
        public async Task<DeliveryResult> ScheduleDeliveryAsync(Order order)
        {
            Console.WriteLine($"   [DeliverySystem.ScheduleDeliveryAsync()] Планируем доставку");
            Console.WriteLine($"   [DeliverySystem] Заказ #{order.Id}, Адрес: {order.ShippingAddress}");
            
    
            await Task.Delay(1000);
            
            if (string.IsNullOrEmpty(order.ShippingAddress))
            {
                Console.WriteLine($"   [DeliverySystem] ❌ Ошибка: Не указан адрес доставки");
                return new DeliveryResult(false, errorMessage: "Не указан адрес доставки");
            }
            
            var trackingNumber = $"TRK{_random.Next(100000000, 999999999)}";
            var estimatedDelivery = DateTime.Now.AddDays(_random.Next(2, 5));
            
            Console.WriteLine($"   [DeliverySystem] ✅ Доставка запланирована");
            Console.WriteLine($"   [DeliverySystem] Трек-номер: {trackingNumber}");
            Console.WriteLine($"   [DeliverySystem] Примерная дата: {estimatedDelivery:dd.MM.yyyy}");
            
            return new DeliveryResult(true, trackingNumber, estimatedDelivery);
        }
        
        public string TrackDelivery(string trackingNumber)
        {
            Console.WriteLine($"   [DeliverySystem.TrackDelivery()] Отслеживаем доставку #{trackingNumber}");
            
            string[] statuses = {
                "Заказ принят в обработку",
                "Заказ собран на складе",
                "Передан в службу доставки",
                "В пути к сортировочному центру",
                "Прибыл в сортировочный центр",
                "Отправлен в пункт выдачи",
                "Готов к выдаче",
                "Доставлен"
            };
            
            int statusIndex = _random.Next(statuses.Length);
            var deliveryDate = DateTime.Now.AddDays(_random.Next(1, 4));
            
            Console.WriteLine($"   [DeliverySystem] Статус: {statuses[statusIndex]}");
            
            return $"{statuses[statusIndex]}. Примерная дата доставки: {deliveryDate:dd.MM.yyyy}";
        }
        
        public async Task<bool> CancelDeliveryAsync(string trackingNumber)
        {
            Console.WriteLine($"   [DeliverySystem.CancelDeliveryAsync()] Отменяем доставку #{trackingNumber}");
            
            await Task.Delay(800);
            
            bool isSuccess = _random.Next(100) < 70; // 70% успеха
            Console.WriteLine($"   [DeliverySystem] {(isSuccess ? "✅" : "❌")} Доставка {(isSuccess ? "отменена" : "не может быть отменена")}");
            
            return isSuccess;
        }
    }
}

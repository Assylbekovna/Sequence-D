using System;
using System.Threading.Tasks;
using OnlineStoreSequence.Models;

namespace OnlineStoreSequence.Services
{
    public class NotificationService
    {
        public async Task SendOrderConfirmationAsync(User user, Order order)
        {
            Console.WriteLine($"   [NotificationService.SendOrderConfirmationAsync()] Отправляем подтверждение заказа");
            
   
            await Task.Delay(500);
            
            Console.WriteLine($"   [NotificationService] 📧 Email отправлен на: {user.Email}");
            Console.WriteLine($"   [NotificationService] Тема: Подтверждение заказа #{order.Id}");
            Console.WriteLine($"   [NotificationService] Сообщение: Ваш заказ на сумму {order.TotalAmount:C} успешно оформлен!");
            
            // Имитация отправки SMS
            await Task.Delay(300);
            
            Console.WriteLine($"   [NotificationService] 📱 SMS отправлено на: {user.Phone}");
            Console.WriteLine($"   [NotificationService] Сообщение: Заказ #{order.Id} оформлен. Сумма: {order.TotalAmount:C}");
        }
        
        public async Task SendShippingUpdateAsync(User user, Order order, string updateMessage)
        {
            Console.WriteLine($"   [NotificationService.SendShippingUpdateAsync()] Отправляем обновление о доставке");
            
            await Task.Delay(400);
            
            Console.WriteLine($"   [NotificationService] 📧 Email отправлен: {updateMessage}");
            Console.WriteLine($"   [NotificationService] Трек-номер: {order.TrackingNumber}");
        }
        
        public async Task SendPaymentFailedAsync(User user, Order order, string errorMessage)
        {
            Console.WriteLine($"   [NotificationService.SendPaymentFailedAsync()] Отправляем уведомление об ошибке оплаты");
            
            await Task.Delay(400);
            
            Console.WriteLine($"   [NotificationService] 📧 Email отправлен: Ошибка оплаты заказа #{order.Id}");
            Console.WriteLine($"   [NotificationService] Причина: {errorMessage}");
        }
    }
}

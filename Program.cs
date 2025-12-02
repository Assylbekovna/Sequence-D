using System;
using System.Threading.Tasks;
using OnlineStoreSequence.Models;
using OnlineStoreSequence.Services;

namespace OnlineStoreSequence
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА ИНТЕРНЕТ-МАГАЗИНА ===");
            Console.WriteLine("Процесс оформления заказа с диаграммой последовательности\n");
            
            try
            {
                var user = new User
                {
                    Id = 1,
                    Name = "Иван Иванов",
                    Email = "ivan@example.com",
                    Phone = "+7-999-123-45-67",
                    Address = "ул. Пушкина, д. 10"
                };
                
                Console.WriteLine($"Пользователь: {user.Name}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Телефон: {user.Phone}\n");
            
                var shoppingCart = new ShoppingCart(user.Id);
          
                var products = new[]
                {
                    new Product(1, "Ноутбук Lenovo IdeaPad", 45000, "15.6 дюймов, 8 ГБ ОЗУ", 10),
                    new Product(2, "Смартфон Samsung Galaxy", 35000, "6.5 дюймов, 128 ГБ", 15),
                    new Product(3, "Наушники Sony WH-1000XM4", 25000, "Беспроводные, шумоподавление", 20)
                };
              
                Console.WriteLine("1. ПОЛЬЗОВАТЕЛЬ -> КОРЗИНА: Добавление товаров");
                Console.WriteLine("   [User.AddToCart() вызывается]");
                
                shoppingCart.AddItem(products[0], 1);
                Console.WriteLine($"   Добавлен: {products[0].Name}");
                
                shoppingCart.AddItem(products[1], 1);
                Console.WriteLine($"   Добавлен: {products[1].Name}");
                
                shoppingCart.AddItem(products[2], 2);
                Console.WriteLine($"   Добавлен: {products[2].Name} x2");
              
                shoppingCart.DisplayCart();
            
                var paymentSystem = new PaymentSystem();
                var database = new DatabaseService();
                var deliverySystem = new DeliverySystem();
                var notificationService = new NotificationService();
              
                Console.WriteLine("\n2. ПОЛЬЗОВАТЕЛЬ -> КОРЗИНА: Оформление заказа");
                Console.WriteLine("   [User.Checkout() вызывается]");
                
                var orderProcessor = new OrderProcessor(
                    shoppingCart,
                    paymentSystem,
                    database,
                    deliverySystem,
                    notificationService
                );
                
              
                var orderResult = await orderProcessor.ProcessOrderAsync(user);
                
                if (orderResult.IsSuccess)
                {
                    Console.WriteLine("\n ЗАКАЗ УСПЕШНО ОФОРМЛЕН!");
                    Console.WriteLine($"Номер заказа: {orderResult.Order.Id}");
                    Console.WriteLine($"Трек-номер: {orderResult.Order.TrackingNumber}");
                    Console.WriteLine($"Статус: {orderResult.Order.Status}");
                }
                else
                {
                    Console.WriteLine($"\n Ошибка оформления заказа: {orderResult.ErrorMessage}");
                }
                
                await DemonstrateAdditionalOperations(user, orderResult.Order, database, deliverySystem);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Произошла ошибка: {ex.Message}");
            }
            
            Console.WriteLine("\n=== ПРОГРАММА ЗАВЕРШЕНА ===");
        }
        
        static async Task DemonstrateAdditionalOperations(
            User user, 
            Order order, 
            DatabaseService database, 
            DeliverySystem deliverySystem)
        {
            if (order == null) return;
            
            Console.WriteLine("\n--- ДОПОЛНИТЕЛЬНЫЕ ОПЕРАЦИИ ---");
          
            Console.WriteLine("\n3. ПОЛЬЗОВАТЕЛЬ -> БАЗА ДАННЫХ: Проверка статуса");
            var status = database.GetOrderStatus(order.Id);
            Console.WriteLine($"   Статус заказа #{order.Id}: {status}");
            
           
            Console.WriteLine("\n4. ПОЛЬЗОВАТЕЛЬ -> СИСТЕМА ДОСТАВКИ: Отслеживание");
            var trackingInfo = deliverySystem.TrackDelivery(order.TrackingNumber);
            Console.WriteLine($"   Информация о доставке: {trackingInfo}");
            
        
            Console.WriteLine("\n5. ДЕМОНСТРАЦИЯ: Отмена заказа");
            var cancelResult = await database.CancelOrderAsync(order.Id, "Изменение планов");
            Console.WriteLine($"   Результат отмены: {(cancelResult ? "Успешно" : "Не удалось")}");
        }
    }
}

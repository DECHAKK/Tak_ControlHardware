using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("--- เริ่มระบบสั่งซื้อสินค้า ---");
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            // จำลองข้อมูลคำสั่งซื้อ
            var orderId = "ORD-12345";
            var items = new List<string> { "Laptop", "Mouse", "Keyboard" };

            Console.WriteLine($"กำลังประมวลผลคำสั่งซื้อ {orderId}...");

            // 1. ทำงานแบบ Parallel: ตรวจสอบสต็อก และ ตรวจสอบที่อยู่จัดส่ง พร้อมกัน
            // (เพราะสองงานนี้ไม่ต้องรอกันและกัน)
            Task<bool> stockTask = CheckStockAsync(items);
            Task<bool> addressTask = ValidateAddressAsync();

            // รอให้ทั้งสองงานเสร็จสิ้น
            await Task.WhenAll(stockTask, addressTask);

            if (stockTask.Result && addressTask.Result)
            {
                Console.WriteLine("✅ สต็อกและที่อยู่ถูกต้อง");

                // 2. งานคำนวณภาษี (CPU-bound): ใช้ Task.Run เพื่อไม่ให้บล็อก Main Thread
                decimal totalAmount = await Task.Run(() => CalculateTotalWithTax(15000));
                Console.WriteLine($"💰 ยอดชำระสุทธิ: {totalAmount} บาท");

                // 3. งานตัดเงิน (I/O-bound): ต้องรอผลลัพธ์ก่อนถึงจะส่งเมลได้ (Sequential)
                bool paymentSuccess = await ProcessPaymentAsync(totalAmount);

                if (paymentSuccess)
                {
                    Console.WriteLine("✅ ชำระเงินสำเร็จ");

                    // 4. ส่งอีเมล (Fire and Forget หรือทำงานเบื้องหลัง)
                    // เราไม่จำเป็นต้องให้ลูกค้ารอจนเมลส่งเสร็จถึงจะเห็นหน้า "ขอบคุณ"
                    _ = SendEmailAsync(orderId); 
                    Console.WriteLine("📧 กำลังส่งอีเมลยืนยันในพื้นหลัง...");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ เกิดข้อผิดพลาด: {ex.Message}");
        }

        stopwatch.Stop();
        Console.WriteLine($"--- เสร็จสิ้นในเวลา: {stopwatch.ElapsedMilliseconds} ms ---");
        Console.WriteLine("หน้าจอแสดงผล: 'สั่งซื้อสินค้าสำเร็จ! กรุณารอรับอีเมลยืนยัน'");
        
        // รอสักครู่เพื่อให้ Task ส่งเมล (ที่รันเบื้องหลัง) ทำงานเสร็จก่อนปิดโปรแกรม
        await Task.Delay(2000);
    }

    // --- จำลอง Method การทำงานต่างๆ ---

    static async Task<bool> CheckStockAsync(List<string> items)
    {
        Console.WriteLine("Checking Stock...");
        await Task.Delay(1000); // จำลองการต่อ Database
        return true;
    }

    static async Task<bool> ValidateAddressAsync()
    {
        Console.WriteLine("Validating Address...");
        await Task.Delay(800); // จำลองการต่อ API ที่อยู่
        return true;
    }

    static decimal CalculateTotalWithTax(decimal amount)
    {
        Console.WriteLine("Calculating Tax (CPU heavy)...");
        // จำลองการคำนวณที่ซับซ้อน
        System.Threading.Thread.Sleep(500); 
        return amount * 1.07m;
    }

    static async Task<bool> ProcessPaymentAsync(decimal amount)
    {
        Console.WriteLine($"Processing Payment {amount}...");
        await Task.Delay(1500); // จำลองการต่อ Payment Gateway
        return true;
    }

    static async Task SendEmailAsync(string orderId)
    {
        await Task.Delay(1000); // จำลองการส่งเมล
        Console.WriteLine($"[Background] อีเมลสำหรับ {orderId} ถูกส่งเรียบร้อยแล้ว!");
    }
}

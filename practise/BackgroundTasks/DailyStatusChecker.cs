using Microsoft.AspNetCore.SignalR;
using practise.SignalR.Hubs;
using System;
using Microsoft.Data.SqlClient;
using System.Threading;
namespace practise.BackgroundTasks
{
    public class DailyStatusChecker
    {
        private readonly IHubContext<CheckboxHub> _hubContext;
        private readonly Timer _timer;

        public DailyStatusChecker(IHubContext<CheckboxHub> hubContext)
        {
            Console.WriteLine("DailyStatusChecker 已啟動！"); // 測試輸出
            _hubContext = hubContext;
            _timer = new Timer(async _ => await CheckDatabase(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private async Task CheckDatabase()
        {
            Console.WriteLine("CheckDatabase 執行中...");
            using (var connection = new SqlConnection("Server=LAPTOP-H7OIK5RV;Database=TDL;TrustServerCertificate=True;Trusted_Connection=True;"))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("EXEC dbo.ResetDailyItems", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dailyId = reader["Id"].ToString();
                            var itemStatus = reader["ItemStatus"].ToString();

                            Console.WriteLine($"🔄 發送到前端：{dailyId}, {itemStatus}"); // 測試 log

                            // 使用 SignalR 通知前端
                            await _hubContext.Clients.All.SendAsync("ReceiveItemStatusUpdate", dailyId, itemStatus);
                        }
                    }
                }
            }
        }
    }
}

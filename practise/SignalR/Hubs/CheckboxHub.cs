using Microsoft.AspNetCore.SignalR;
using practise.Models;
using System.Threading.Tasks;
namespace practise.SignalR.Hubs
{
    public class CheckboxHub : Hub
    {
        public async Task ToggleCheckbox(CheckboxUpdate data)
        {
            Console.WriteLine($"📥 收到 ToggleCheckbox: dailyId = {data.DailyId}, isChecked = {data.IsChecked}");

            // ✅ 廣播變更給所有其他用戶端
            await Clients.Others.SendAsync("ReceiveCheckboxUpdate", data.DailyId, data.IsChecked);
        }
    }
}

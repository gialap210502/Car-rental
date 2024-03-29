using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Car_rental.Data;
using Car_rental.Models;
using NuGet.Packaging.Signing;

namespace MySignalRApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly Car_rentalContext _context;

        public ChatHub(Car_rentalContext context)
        {
            _context = context;
        }
        public async Task SendMessage1(string user, string message)
        {
            string connectionId = Context.ConnectionId; // Get the connection ID of the current client
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        // Phương thức này được gọi khi một khách hàng gửi tin nhắn
        public async Task SendMessage(int conversationId, int userId, string userName, string content, string userImgString)
        {
            string connectionId = Context.ConnectionId;
            // Ở đây, bạn có thể thực hiện các xử lý cần thiết trước khi gửi tin nhắn.
            // Ví dụ: lưu tin nhắn vào cơ sở dữ liệu, kiểm tra quyền truy cập, vv.
            // Tạo một tin nhắn mới
            DateTime Timestamp = DateTime.Now;
            var message = new Message
            {
                ConversationID = conversationId,
                UserID = userId,
                Content = content,
                SentAt = Timestamp
            };

            // Thêm tin nhắn vào DbContext và lưu vào cơ sở dữ liệu
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            // Gửi tin nhắn tới tất cả các khách hàng trong cùng một phòng
            await Clients.Group(conversationId.ToString()).SendAsync("TakeMessage", userId, Timestamp, userName, content, userImgString);
        }

        // Đây là một phương thức để khách hàng tham gia một phòng (conversation)
        public async Task JoinConversation(int conversationId)
        {
            // Gọi phương thức Groups.AddToGroup để tham gia phòng
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }
    }
}

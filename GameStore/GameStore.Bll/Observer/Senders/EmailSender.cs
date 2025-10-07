using System.Net;
using System.Net.Mail;

namespace GameStore.Bll.Observer
{
    public class EmailSender : BaseSender
    {
        public override void Send(Manager manager, UserInfo userInfo, OrderModel order)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("pavel1997pileckii@gmail.com")); // manager email
            message.From = new MailAddress("paveltest02007@gmail.com");
            message.Subject = "GameStore";
            body += $"<p>User: {userInfo.Name} {userInfo.Surname} ({userInfo.Email}) have bought game. </p>" +
                    $"<p>Order date: {order.OrderDate} </p>" +
                    $"<p>IsPaid: {order.IsPaid} </p>" +
                    $"<p>Order details: </p>";
            foreach (var game in order.Games)
            {
                body += $"<p>({game.Key}) {game.Name} (Price: {game.Price}) </p>";
            }

            message.Body = string.Format(body, "GameStore", "paveltest02007@gmail.com");
            message.IsBodyHtml = true;


            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "paveltest02007@gmail.com",
                    Password = "!12345qwer"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }
    }
}
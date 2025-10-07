using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using GameStore.Bll.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GameStore.Web.Arrangements
{
    public class WorkWithFiles
    {
        public FileStreamResult CreateFilePdf(OrderDTO order, List<GameDTO> games)
        {
            var table = new PdfPTable(4);
            table.AddCell("Game");
            table.AddCell("Description");
            table.AddCell("Quantity");
            table.AddCell("Price");
            decimal sumPrice = 0;
            if (order != null)
            {
                foreach (var orders in order.OrderDetails)
                {
                    var game = games.FirstOrDefault(gam => gam.Key == orders.ProductID);
                    if (game != null)
                    {
                        table.AddCell(game.Name);
                        var descroptions = game.GameTranslates.Select(tr => tr.Description).ToList();
                        var temp = "EN \n" + descroptions[0];
                        table.AddCell(temp);
                        table.AddCell(orders.Quantity.ToString());
                        table.AddCell(game.Price.ToString());
                        sumPrice += game.Price * orders.Quantity;
                    }
                }
            }

            string text = order.OrderDate + "\n";
            text += "Bill: " + sumPrice;

            MemoryStream stream = new MemoryStream();
            using (var memoryStream = new MemoryStream())
            using (var document = new Document())
            {
                PdfWriter.GetInstance(document, memoryStream).CloseStream = false;

                document.Open();
                document.Add(new Paragraph("Games: \n"));
                document.Add(table);
                document.Add(new Paragraph(text));
                document.Close();

                memoryStream.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);

                return new FileStreamResult(stream, "application/pdf");
            }
        }
    }
}
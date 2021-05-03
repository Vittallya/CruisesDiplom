using DAL;
using DAL.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;

namespace BL
{
    public class WordService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapper;

        public string Message { get; private set; }

        public WordService(AllDbContext dbContext, MapperService mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public void ShowOrderContract(OrderDto orderDto, ValuteGetterService valuteGetter, string path)
        {
            Dictionary<string, string> stubs = new Dictionary<string, string>();


            DateTime endDate = orderDto.TourDto.StartDate.AddDays(orderDto.TourDto.DaysCount);
            StringBuilder insStr = new StringBuilder();
            

            stubs.Add("[Номер_заказа]", orderDto.Id.ToString());
            stubs.Add("[День]", orderDto.CreationDate.ToString("dd"));
            stubs.Add("[Месяц]", orderDto.CreationDate.ToString("MMMM"));
            stubs.Add("[Год]", orderDto.CreationDate.ToString("yyyy"));
            stubs.Add("[Дата_с]", orderDto.TourDto.StartDate.ToString("dd.MM.yyyy"));
            stubs.Add("[Дата_по]", endDate.ToString("dd.MM.yyyy"));
            stubs.Add("[Кол-во_дней]", orderDto.TourDto.DaysCount.ToString());
            stubs.Add("[Кол-во_туристов]", orderDto.PeopleCount.ToString());


            if (orderDto.HasIns)
            {
                insStr.AppendLine("Страховка: ");
                foreach (var ins in orderDto.InsuranceDtos)
                {
                    insStr.AppendLine(ins.Name);
                }
                stubs.Add("[Страховка]", insStr.ToString());
            }
            else 
                stubs.Add("[Страховка]", string.Empty);

            stubs.Add("[Общая_стоимость]", orderDto.FullCost.ToString("{0:N2}"));
            stubs.Add("[Общая_стоимостьUSD]", valuteGetter.GetUSDValue(orderDto.FullCost).ToString("{0:N2}"));
            stubs.Add("[Общая_стоимостьEUR]", valuteGetter.GetEuroValue(orderDto.FullCost).ToString("{0:N2}"));

            ShowDocument(stubs, path);

        }

        private bool ShowDocument(Dictionary<string, string> stubs, string path)
        {
            var wordApp = new word.Application();
            try
            {
                wordApp.Visible = false;


                var newDoc = wordApp.Documents.Add();
                newDoc.Content.InsertFile(path);
                newDoc.Content.Font.Name = "Times New Roman";
                Console.WriteLine(newDoc.Content.PageSetup.LeftMargin);
                newDoc.Content.PageSetup.LeftMargin = 56.7f;
                newDoc.Content.PageSetup.RightMargin = 56.7f;

                foreach ( var stub in stubs )
                {
                    if ( stub.Value != null && !string.IsNullOrWhiteSpace(stub.Value) )
                    {
                        ReplaceWordStub(stub.Key, stub.Value, newDoc);
                    }
                }
                wordApp.Visible = true;
            }
            catch ( Exception ex )
            {
                Message = ex.Message;
                return false;
            }
            finally
            {
                wordApp.Quit();
            }
            return true;
        }

        private void ReplaceWordStub(string stub, string value, word.Document document)
        {
            var range = document.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stub, ReplaceWith: value);
        }
    }
}

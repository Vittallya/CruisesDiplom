using DAL;
using DAL.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;

namespace BL
{
    public class WordService
    {
        public string Message { get; private set; }
        public void ShowOrderContract(OrderDto orderDto, ValuteGetterService valuteGetter, string path)
        {
            Dictionary<string, string> stubs = new Dictionary<string, string>();
            Dictionary<string, string[]> stubsColl = new Dictionary<string, string[]>();


            DateTime endDate = orderDto.TourDto.StartDate.AddDays(orderDto.TourDto.DaysCount);
            StringBuilder insStr = new StringBuilder();
            List<string> plList = new List<string>();
            List<string> insList = new List<string>();
            

            stubs.Add("[Номер_заказа]", orderDto.Id.ToString());
            stubs.Add("[День]", orderDto.CreationDate.ToString("dd"));
            stubs.Add("[Месяц]", orderDto.CreationDate.ToString("MMMM"));
            stubs.Add("[Год]", orderDto.CreationDate.ToString("yyyy"));
            stubs.Add("[ФИО]", orderDto.ClientDto.Name);
            stubs.Add("[Дата_с]", orderDto.TourDto.StartDate.ToString("dd.MM.yyyy"));
            stubs.Add("[Дата_по]", endDate.ToString("dd.MM.yyyy"));
            stubs.Add("[Кол-во_дней]", orderDto.TourDto.DaysCount.ToString());
            stubs.Add("[Кол-во_туристов]", orderDto.PeopleCount.ToString());


            var list = orderDto.PlacementDtos.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var pl = list[i];

                string doc = !pl.IsChildBefore14 ? pl.Pasport : pl.BirthDoc;
                string docName = !pl.IsChildBefore14 ? "паспорт" : "св-во. рождении";

                string line = $"{i+1}){pl.Fio}, {docName}: {doc}, каюта №: {pl.CabinDto.Id}, палуба: {pl.CabinDto.Deck}";
                plList.Add(line);
            }
            stubsColl.Add("[Туристы]", plList.ToArray());


            if (orderDto.HasIns)
            {
                var list1 = orderDto.InsuranceDtos.ToList();

                for (int i = 0; i < orderDto.InsuranceDtos.Count; i++)
                {
                    var ins = list1[i];
                    string line = $"{i + 1}) {ins.Name}";
                    insList.Add(line);
                }

            }
            else 
                insList.Add("нет");

            stubsColl.Add("[Страховка]", insList.ToArray());

            stubs.Add("[Общая_стоимость]", orderDto.FullCost.ToString("0.##"));
            stubs.Add("[Общая_стоимостьUSD]", valuteGetter.GetUSDValue(orderDto.FullCost).ToString("0.##"));
            stubs.Add("[Общая_стоимостьEUR]", valuteGetter.GetEuroValue(orderDto.FullCost).ToString("0.##"));

            ShowDocument(stubs, path, stubsColl);

        }
        public const int CUT_PART = 8;
        private bool ShowDocument(Dictionary<string, string> stubs, string path, Dictionary<string, string[]> stubsCollection = null)
        {
            var wordApp = new word.Application();
            try
            {
                wordApp.Visible = false;


                var newDoc = wordApp.Documents.Add();
                newDoc.Content.InsertFile(path);
                //newDoc.Content.Font.Name = "Times New Roman";
                //Console.WriteLine(newDoc.Content.PageSetup.LeftMargin);
                //newDoc.Content.PageSetup.LeftMargin = 56.7f;
                //newDoc.Content.PageSetup.RightMargin = 56.7f;

                foreach ( var stub in stubs )
                {   
                    ReplaceWordStub(stub.Key, stub.Value, newDoc);                    
                }

                if(stubsCollection != null)
                {
                    foreach(var stub in stubsCollection)
                    {
                        for (int i = 0; i < stub.Value.Length; i++)
                        {
                            StringBuilder sb = new StringBuilder();
                            if (i < stub.Value.Length - 1)
                            {
                                sb.AppendLine(stub.Value[i]);
                                sb.Append($"{stub.Key}");
                            }
                            else
                            {
                                sb.Append(stub.Value[i]);
                            }

                            ReplaceWordStub(stub.Key, sb.ToString(), newDoc);
                            sb.Clear();
                        }
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

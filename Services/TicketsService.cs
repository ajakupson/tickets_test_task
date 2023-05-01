using agileworks_1.Models;
using Newtonsoft.Json;

namespace agileworks_1.Services
{
    public class TicketsService
    {
        const string ticketsFilePath = "Data/tickets.json";
        public TicketsService() { }
        public List<Ticket> getTickets() 
        {
            List<Ticket> list = new List<Ticket>();
            if (File.Exists(ticketsFilePath))
            {
                using (StreamReader sr = new StreamReader(ticketsFilePath))
                {
                    string json = sr.ReadToEnd();
                    if (!String.IsNullOrEmpty(json))
                    {
                        list = JsonConvert.DeserializeObject<List<Ticket>>(json);
                    }
                }
            } else
            {
                FileStream fs = File.Create(ticketsFilePath); 
                fs.Close();
            }

            return list; 
        }

        public List<Ticket> createTicket(Ticket ticket)
        {
            List<Ticket> list = getTickets();

            int id = 1;
            if (list.Count != 0)
            {
                id = list.Last().Id + 1;
            }

            ticket.Id = id;
            ticket.IsClosed = false;
            list.Add(ticket);

            string jsonData = JsonConvert.SerializeObject(list);
            using (StreamWriter sw = new StreamWriter(ticketsFilePath))
            {
                sw.Write(jsonData); 
            }

            return list;
        }

        public Ticket closeTicket(int id)
        {
            List<Ticket> list = getTickets();
            foreach (Ticket ticket in list)
            {
                if (ticket.Id == id) {
                    ticket.IsClosed = true;

                    string jsonData = JsonConvert.SerializeObject(list);
                    using (StreamWriter sw = new StreamWriter(ticketsFilePath))
                    {
                        sw.Write(jsonData);
                    }

                    return ticket;
                }
            }

            return new Ticket();
        }
    }
}

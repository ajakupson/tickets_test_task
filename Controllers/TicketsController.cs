using agileworks_1.Models;
using agileworks_1.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace agileworks_1.Controllers
{
    [Route("[controller]")]
    public class TicketsController : Controller
    {
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ILogger<TicketsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ViewResult Home()
        {
            TicketsService ticketsService = new TicketsService();
            List<Ticket> ticketsList = ticketsService.getTickets();

            ViewBag.MenuSelection = "tickets";
            ViewBag.TicketsList = JsonSerializer.Serialize(ticketsList);
            return View("Tickets");
        }

        [HttpPost]
        public JsonResponse CreateTicket(Ticket ticketData)
        {
            JsonResponse response = new JsonResponse();

            ICollection<ValidationResult> results = null;
            if (!Helpers.ModelValidation.Validate(ticketData, out results))
            {
                response.ErrorMsg = "Data is not valid!";
                return response;
            }

            TicketsService ticketsService = new TicketsService();
            List<Ticket> ticketsList = ticketsService.createTicket(ticketData);

            response.Code = 200;
            response.IsSuccess = true;
            response.Data = JsonSerializer.Serialize(ticketsList);
            return response;
        }

        [Route("close/{ticketId}")]
        [HttpPost]
        public JsonResponse CloseTicket(int ticketId)
        {
            TicketsService ticketsService = new TicketsService();
            Ticket ticket = ticketsService.closeTicket(ticketId);

            JsonResponse response = new JsonResponse();
            if (ticket.Id != 0)
            {
                response.Code = 200;
                response.IsSuccess = true;
            } else
            {
                ticket.Id = ticketId;
                response.ErrorMsg = "Ticket not found!";
            }
            response.Data = JsonSerializer.Serialize(ticket);

            return response;
        }
    }
}
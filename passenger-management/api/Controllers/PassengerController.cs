using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly PassengerService _passengerService;

        public PassengerController(PassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        public ActionResult<List<Passenger>> Get()
        {
            return _passengerService.Get();
        }

        [HttpGet("all")]
        public ActionResult<List<Passenger>> GetAll()
        {
            return _passengerService.Get(true);
        }

        [HttpGet("{id:length(24)}", Name = "GetPassenger")]
        public ActionResult<Passenger> Get(string id)
        {
            var passenger = _passengerService.Get(id);

            if (passenger == null) return NotFound();

            return passenger;
        }

        [HttpPost]
        public async Task<ActionResult<Passenger>> Create(Passenger passenger)
        {
            await _passengerService.Create(passenger);

            return CreatedAtRoute("GetPassenger", new {id = passenger.Id}, passenger);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<Passenger>> Update(string id, Passenger passengerIn)
        {
            var passenger = _passengerService.Get(id);

            if (passenger == null) return NotFound();

            await _passengerService.Update(id, passengerIn);

            return _passengerService.Get(id);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<Passenger>> Delete(string id)
        {
            var passenger = _passengerService.Get(id);

            if (passenger == null) return NotFound();

            await _passengerService.Delete(passenger.Id);

            return _passengerService.Get(id, true);
        }
    }
}
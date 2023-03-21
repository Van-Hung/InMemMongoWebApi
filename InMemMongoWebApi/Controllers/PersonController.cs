using InMemMongoWebApi.Collections;
using InMemMongoWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InMemMongoWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PersonController : ControllerBase
	{
		private readonly IPersonRepository _personRepository;
		public PersonController(IPersonRepository personRepository)
		{
			_personRepository = personRepository;
		}
	
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var person = await _personRepository.GetAllAsync();
			return Ok(person);
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var person = await _personRepository.GetByIdAsync(id);
			if (person == null)
			{
				return NotFound();
			}

			return Ok(person);
		}

		[HttpPost]
		public async Task<IActionResult> Post(Person newPerson)
		{
			await _personRepository.CreateNewPersonAsync(newPerson);
			return CreatedAtAction(nameof(Get), new { id = newPerson.Id }, newPerson);
		}

		[HttpPut]
		public async Task<IActionResult> Put(Person updatePerson)
		{
			var person = await _personRepository.GetByIdAsync(updatePerson.Id);
			if (person == null)
			{
				return NotFound();
			}

			await _personRepository.UpdatePersonAsync(updatePerson);
			return NoContent();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string id)
		{
			var person = await _personRepository.GetByIdAsync(id);
			if (person == null)
			{
				return NotFound();
			}

			await _personRepository.DeletePersonAsync(id);
			return NoContent();
		}
	}
}

using InMemMongoWebApi.Collections;
using InMemMongoWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InMemMongoWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		public Settings Settings { get; }

		public UserController(IUserRepository userRepository, IOptionsSnapshot<Settings> options)
		{
			_userRepository = userRepository;
			Settings = options.Value;
		}
	
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var user = await _userRepository.GetAllAsync();
			return Ok(user);
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var user = await _userRepository.GetByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		[HttpPost]
		public async Task<IActionResult> Post(User newUser)
		{
			await _userRepository.CreateNewUserAsync(newUser);
			return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
		}

		[HttpPut]
		public async Task<IActionResult> Put(User updateUser)
		{
			var user = await _userRepository.GetByIdAsync(updateUser.Id);
			if (user == null)
			{
				return NotFound();
			}

			await _userRepository.UpdateUserAsync(updateUser);
			return NoContent();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userRepository.GetByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			await _userRepository.DeleteUserAsync(id);
			return NoContent();
		}
	}
}

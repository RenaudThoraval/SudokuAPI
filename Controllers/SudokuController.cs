using Microsoft.AspNetCore.Mvc;
using sudoku.Models;

namespace sudoku.Controllers
{
	[Route("api/sudoku")]
	public class SudokuController : Controller
	{
		#region Routes

		// POST api/sudoku/isValid
		[HttpPost("isValid")]
		public IActionResult IsGridValid([FromBody]SudokuGrid grid)
		{
			if (ModelState.IsValid)
			{
				return Ok(true);
			}
			else
			{
				return BadRequest(ModelState);
			}
		}

		// GET api/sudoku
		[HttpGet("")]
		public IActionResult GenerateGrid()
		{
			var helper = new SudokuHelper();

			return Ok(helper.GenerateValidGrid());
		}

		#endregion
	}
}

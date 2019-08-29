using System.ComponentModel.DataAnnotations;
using Sudoku.Helpers;

namespace Sudoku.Models
{
    [SudokuGridValuesValidation]
    public class SudokuGrid
    {
        [Required]
        [SudokuGridSizeValidation]
        public int[,] Values { get; set; }
    }

    #region Sudoku Grid validation attributes

    class SudokuGridSizeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var grid = (int[,])value;
                if (grid.GetLength(0) != 9 || grid.GetLength(1) != 9)
                {
                    return new ValidationResult("Grid must be a 9 x 9 matrix of numbers");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            catch (System.Exception)
            {
                return new ValidationResult("Grid must be a 9 x 9 matrix of numbers");
            }

        }
    }

    class SudokuGridValuesValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // The model properties are already checked. We can cast without any problem "value" param.
            var sudokuGrid = (SudokuGrid)value;
            var helper = new SudokuHelper(sudokuGrid.Values);

            if (helper.ValidateGridValues())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("The grid is not valid. Some values filled in are not correct.");
            }
        }

    }

    #endregion
}
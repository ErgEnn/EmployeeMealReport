using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using EmployeeMealReport.Pages;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EmployeeMealReport
{
	public class ExcelBuilder
	{
		private readonly DateTime startDate;
		private readonly DateTime endDate;
		private readonly IList<EmployeeDatepicker.DateTimeHolidays> dates;
		private readonly SortedSet<Employee> employees;

		public ExcelBuilder(DateTime startDate, DateTime endDate,IList<EmployeeDatepicker.DateTimeHolidays> dates,SortedSet<Employee> employees)
		{
			this.startDate = startDate;
			this.endDate = endDate;
			this.dates = dates;
			this.employees = employees;
		}

		public async Task<MemoryStream> GetXslAsStream()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			var package = new ExcelPackage();
			var sheet = package.Workbook.Worksheets.Add("Toidukorrad");
			AddReportDate(sheet);
			AddHeaders(sheet);
			int lastRow = AddData(sheet);
			AddFooter(sheet, lastRow);
			AddBorders(sheet);
			return await PackageToStream(package);
		}

		private void AddBorders(ExcelWorksheet sheet)
		{
			for (int row = 2; row <= employees.Count(employee => employee.CurrentMealDays(startDate, endDate).Any())+2; row++)
			{
				for (int col = 1; col < dates.Count+5; col++)
				{
					sheet.Cells[row,col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
				}
			}
		}

		private void AddFooter(ExcelWorksheet sheet, in int row)
		{
			var dateCount = dates.Count;

            sheet.Cells[row, 3 + dateCount].Formula =
                $"=SUM({GetCoord(3, 3 + dateCount)}:{GetCoord(row - 1, 3 + dateCount)})";
            sheet.Cells[row, 3 + dateCount + 1].Formula = $"=SUM({GetCoord(3, 3 + dateCount + 1)}:{GetCoord(row - 1, 3 + dateCount + 1)})";
            sheet.Cells[row, 3 + dateCount + 1].Style.Numberformat.Format = "0.00 €";
        }

		private int AddData(ExcelWorksheet sheet)
		{
			int row = 3;
			foreach (var employee in employees)
			{
				var mealDays = employee.CurrentMealDays(startDate, endDate).ToList();
				if(!mealDays.Any()) continue; // Ignore non-eating employees

				sheet.Cells[row, 1].Value = row-2;
				sheet.Cells[row, 2].Value = employee.LastFirstName;
				
				foreach (var mealDay in mealDays)
				{
					sheet.Cells[row, 2 + mealDay.Date.Day].Value = 1;
				}

				var dateCount = dates.Count;
                sheet.Cells[row, 2 + dateCount + 1].Formula = $"=SUM({GetCoord(row,3)}:{GetCoord(row,2 + dateCount)})";
                sheet.Cells[row, 2 + dateCount + 2].Formula = $"={GetCoord(row,2 + dateCount + 1)}*{Config.PricePerMeal}";
                sheet.Cells[row, 2 + dateCount + 2].Style.Numberformat.Format = "0.00 €";
                row++;
			}

			return row;
		}

		private void AddHeaders(ExcelWorksheet sheet)
		{
			int row = 2;
			sheet.Column(1).Width = 5.5;
			sheet.Cells[row,2].Value = "Nimi";
			sheet.Column(2).Width = 18;
			var dateCount = dates.Count();
			int colOffset = 0;
			foreach (var date in dates)
			{
				if (date.IsRed)
				{
					using (var range = sheet.Cells[2, 3 + colOffset, employees.Count(employee => employee.CurrentMealDays(startDate,endDate).Any()) + 2, 3 + colOffset])
					{
						range.Style.Fill.PatternType = ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.PaleVioletRed);
					}
				}
				sheet.Cells[row, 3 + colOffset].Value = date.Date.Day;
				sheet.Column(3 + colOffset).Width = 2.71;
				colOffset++;
			}
			sheet.Cells[row, 3 + dateCount].Value = "Kokku";
			sheet.Column(3 + dateCount).Width = 6;
			sheet.Cells[row, 3 + dateCount+1].Value = "Hind";
			sheet.Column(3 + dateCount + 1).Width = 7.7;
		}

		private void AddReportDate(ExcelWorksheet sheet)
		{
			sheet.Cells[1,1].Value = startDate.Year;
			sheet.Cells[1, 1].Style.Font.Bold = true;
			sheet.Cells[1,2].Value = MonthString();
			sheet.Cells[1, 2].Style.Font.Bold = true;
		}

		private string MonthString() {
			string[] months = new[] { "JAN", "VEB", "MÄR", "APR", "MAI", "JUN", "JUL", "AUG", "SEP", "OKT", "NOV", "DET" };
			return months[startDate.Month - 1];
		}

        private string GetCoord(int row, int col)
        {
            return ExcelCellAddress.GetColumnLetter(col) + row;
        }

		private async Task<MemoryStream> PackageToStream(ExcelPackage package)
		{
			var stream = new MemoryStream();
			await package.SaveAsAsync(stream);
			return stream;
		}
	}
}

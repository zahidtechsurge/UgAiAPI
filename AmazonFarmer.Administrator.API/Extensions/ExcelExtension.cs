using AmazonFarmer.Core.Application.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using AmazonFarmer.Core.Application.Exceptions;

namespace AmazonFarmer.Administrator.API.Extensions
{
    public class ExcelExtension
    {

        public ExcelExtension()
        {
        }
        public static ExcelPackage generateTable(List<dynamic> data, ExcelPackage package, string tabName)
        {
            if (data == null || data.Count == 0)
            {
                throw new AmazonFarmerException("");
            }
            // Add a new worksheet to the ExcelPackage
            var workSheet = package.Workbook.Worksheets.Add(tabName);
            var firstItem = data[0];
            var properties = firstItem.GetType().GetProperties();
            // Set the headers
            for (int i = 0; i < properties.Length; i++)
            {
                // Use Description attribute if available, otherwise use the property name
                workSheet.Cells[1, (i + 1)].Value = getDescription(properties[i]);
            }
            int row = 2;
            // Generate the table body using the data
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    // Retrieve the property value
                    var propertyValue = properties[j].GetValue(data[i], null);
                    // Check if the value is of type DateTime
                    if (propertyValue is DateTime)
                    {
                        // If it's a DateTime, you can format it or handle it differently
                        workSheet.Cells[row, (j + 1)].Value = ((DateTime)propertyValue).ToString("MMM dd, yyyy, hh:mm:ss tt"); // Example formatting
                    }
                    else
                    {
                        // For other types, handle accordingly
                        workSheet.Cells[row, (j + 1)].Value = propertyValue?.ToString() ?? ""; // Default to empty string if null
                    }
                    //workSheet.Cells[row, (j + 1)].Value = properties[j].GetValue(data[i], null).GetType() is DateTime ? properties[j].GetValue(data[i], null).ToString("yyyy-MM-dd") : properties[j].GetValue(data[i], null);
                }
                //workSheet.Cells[row, 15, row, 31].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                row++;
            }
            workSheet.Cells.AutoFitColumns();
            using (var range = workSheet.Cells[1, 1, 1, properties.Length])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(1, 200, 200, 200);
                range.Style.Font.Bold = true;
            }

            // Apply borders to the header row
            string modelRange = "A1:" + workSheet.Cells.Last().Address.ToString();
            var modelTable = workSheet.Cells[modelRange];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Medium;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Medium;

            // Return the ExcelPackage
            return package;
        }


        // Main method to generate the table dynamically for any class
        private string generateTable(List<dynamic> data)
        {
            if (data == null || data.Count == 0)
            {
                return "No Data Available";
            }

            string resp = "<table border='1'>";

            // Generate the table header using column names (property names or descriptions)
            resp += "<thead><tr>";

            // Get the properties of the first object (assuming all objects in the list have the same structure)
            var firstItem = data[0];
            var properties = firstItem.GetType().GetProperties();

            foreach (var property in properties)
            {
                // Use Description attribute if available, otherwise use the property name
                resp += $"<th>{getDescription(property)}</th>";
            }
            resp += "</tr></thead>";

            // Generate the table body using the data
            resp += "<tbody>";
            foreach (var row in data)
            {
                resp += "<tr>";

                // For each property in the row, get its value
                foreach (var property in properties)
                {
                    var value = property.GetValue(row, null);
                    resp += $"<td>{value}</td>";
                }

                resp += "</tr>";
            }
            resp += "</tbody>";

            resp += "</table>";

            return resp;
        }

        // Method to get the Description attribute of a property, if it exists
        private static string getDescription(System.Reflection.PropertyInfo property)
        {
            // Get the DescriptionAttribute from the property, if it exists
            var descriptionAttribute = property.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                               .OfType<DescriptionAttribute>()
                                               .FirstOrDefault();

            // Return the description if the attribute exists, otherwise return the property name
            return descriptionAttribute != null ? descriptionAttribute.Description : property.Name;
        }



    }
}

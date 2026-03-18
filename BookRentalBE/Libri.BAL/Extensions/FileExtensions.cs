using System.Globalization;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Xml.Serialization;
using Libri.BAL.Helpers;
using Libri.DAL.Models.Custom.CustomError;

namespace Libri.BAL.Extensions
{
    public static class FileExtensions
    {
        public static async Task<(List<ErrorDetails> validateErrors, List<T> entities)> ConvertFileDataToObjectModelAsync<T>(IFormFile file, FileValidation fileValidation, Dictionary<string, string> headerMapping) where T : new()
        {
            var entities = new List<T>();
            var validateErrors = new List<ErrorDetails>();

            if (file == null || file.Length == 0)
            {
                validateErrors.Add(new ErrorDetails
                {
                    Location = "File tải lên",
                    Details = "File chưa được tải lên."
                });
            }

            if (IsExcelFile(file.ContentType))
            {
                return await ConvertExcelDataToObjectModelAsync<T>(file, fileValidation, headerMapping);
            }
            else
            {
                validateErrors.Add(new ErrorDetails
                {
                    Location = "File tải lên", 
                    Details = "File không hợp lệ! Vui lòng nhập file excel."
                });
                
                return (validateErrors, entities);
            }
        }

        private static async Task<(List<ErrorDetails> validateErrors, List<T> entities)> ConvertExcelDataToObjectModelAsync<T>(IFormFile file, FileValidation fileValidation, Dictionary<string, string> headerMapping) where T : new()
        {
            var validateErrors = new List<ErrorDetails>();
            var entities = new List<T>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;
                    var missingColumns = new List<string>(fileValidation.RequiredColumns);
                    var checkedHeaders = new HashSet<string>();

                    for (int col = 1; col <= colCount; col++)
                    {
                        var header = worksheet.Cells[1, col].Text;
                        if (!checkedHeaders.Add(header))
                        {
                            validateErrors.Add(new ErrorDetails
                            {
                                Location = $"Cột '{header}'", 
                                Details = $"Cột '{header}' bị trùng"
                            });
                        }
                        missingColumns.Remove(header);
                    }

                    if (missingColumns.Any())
                    {
                        validateErrors.Add(new ErrorDetails 
                        {
                            Location = "File tải lên", 
                            Details = "Thiếu các cột: " + string.Join(", ", missingColumns)
                        });
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var rowData = new Dictionary<string, string>();

                        for (int col = 1; col <= colCount; col++)
                        {
                            var header = worksheet.Cells[1, col].Text;
                            var cellValue = worksheet.Cells[row, col].Text;
                            rowData[header] = cellValue;
                        }

                        var (rowErrors, entity) = ValidateRowData<T>(rowData, fileValidation, headerMapping, row);
                        validateErrors.AddRange(rowErrors);

                        if (!rowErrors.Any())
                        {
                            entities.Add(entity);
                        }

                    }
                }
            }

            return (validateErrors, entities);
        }

        private static (List<ErrorDetails> validateErrors, T entity) ValidateRowData<T>(IDictionary<string, string> rowData, FileValidation fileValidation, Dictionary<string, string> headerMapping, int rowIndex) where T : new()
        {
            var validateErrors = new List<ErrorDetails>();
            var entity = new T();
            var properties = typeof(T).GetProperties();

            foreach (var header in rowData.Keys)
            {
                var cellValue = rowData[header];
                var mappedHeader = headerMapping.ContainsKey(header) ? headerMapping[header] : header;

                if (fileValidation.ColumnDataTypes.ContainsKey(header))
                {
                    string expectedType = fileValidation.ColumnDataTypes[header];
                    bool isValid = ValidateDataType(cellValue, expectedType);

                    if (!isValid)
                    {
                        validateErrors.Add(new ErrorDetails
                        {
                            Location = $"Dòng {rowIndex}, Cột '{header}'", 
                            Details = $"Dữ liệu không hợp lệ. Yêu cầu dữ liệu: {expectedType}."
                            
                        });
                        continue;
                    }
                }

                if (fileValidation.Rules.ContainsKey(header))
                {
                    var (rule, description) = fileValidation.Rules[header];
                    bool isValid = rule(cellValue);

                    if (!isValid)
                    {
                        validateErrors.Add(new ErrorDetails
                        {
                            Location = $"Dòng {rowIndex}, Cột '{header}'", 
                            Details = $"Dữ liệu không đúng yêu cầu: '{description}'."
                        });
                        continue;
                    }
                }

                var prop = properties.FirstOrDefault(p =>
                    p.GetCustomAttributes(typeof(XmlElementAttribute), false)
                    .Cast<XmlElementAttribute>()
                    .Any(a => a.ElementName.Equals(mappedHeader, StringComparison.InvariantCultureIgnoreCase)))
                    ?? properties.FirstOrDefault(p =>
                    p.Name.Equals(mappedHeader.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase));

                if (prop != null)
                {
                    var typedValue = ConvertValue(prop.PropertyType, cellValue);
                    prop.SetValue(entity, typedValue);
                }
            }

            return (validateErrors, entity);
        }

        private static bool IsExcelFile(string contentType)
        {
            return contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                   contentType == "application/vnd.ms-excel";
        }

        private static object? ConvertValue(Type type, string value)
        {
            if (type == typeof(string))
                return value.Trim();

            if (type == typeof(int))
                return int.Parse(value);

            if (type == typeof(bool))
            {
                if (bool.TryParse(value, out var result))
                    return result;

                return value == "1";
            }

            if (type == typeof(decimal))
            {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                    return result;
            }

            if (type == typeof(DateTime))
            {
                if (DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    return result;
            }

            if (type.IsEnum)
                return Enum.Parse(type, value);

            return null;
        }

        private static bool ValidateDataType(string value, string expectedType)
        {
            switch (expectedType.ToLower())
            {
                case "chuỗi ký tự":
                    return true;
                case "số nguyên":
                    return int.TryParse(value, out _);
                case "số thập phân":
                    return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
                case "bool":
                    return bool.TryParse(value, out _) || value == "1" || value == "0";
                case "ngày tháng năm":
                    return DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                default:
                    return false;
            }
        }
    }
}

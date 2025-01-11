using System.Text.Json.Serialization;

namespace WebUI.Models.CompanyApi
{
    public class EmployeeListItemViewModel
    {
        [JsonPropertyName("businessEntityID")]
        public int BusinessEntityID { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }

        [JsonPropertyName("jobTitle")]
        public string? JobTitle { get; set; }

        [JsonPropertyName("department")]
        public string? Department { get; set; }

        [JsonPropertyName("shift")]
        public string? Shift { get; set; }

        [JsonPropertyName("managerName")]
        public string? ManagerName { get; set; }

        [JsonPropertyName("employmentStatus")]
        public string? EmploymentStatus { get; set; }


    }
}
using System;
using System.Data;
using System.Text.Json.Serialization;

namespace ebanx_api.AccountRecords
{
    public sealed class Account
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }
        public double Balance { get; set; }
    }

    public sealed class AccountDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Destination { get; set; }
        public double Amount { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Origin { get; set; }
    };

    public sealed class AccountReturn
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Account? Origin { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Account? Destination { get; set; }
    }
}


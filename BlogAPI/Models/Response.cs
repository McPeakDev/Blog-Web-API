using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogAPI.Models {
    #nullable enable
    /// <summary>
    /// The custom response object for the Web API.
    /// </summary>
    public class Response: IActionResult {

        /// <summary>
        /// The status code to respond with. Set to 200 by default.
        /// </summary>
        [JsonIgnore]
        public int StatusCode { get; set; } = 200;

        /// <summary>
        /// The message to send. Nullable.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The error to send. Nullable.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// The data to send. Nullable.
        /// </summary>
        public object? Data { get; set; }


        /// <summary>
        /// Allows for the StatusCode to be used for the response.
        /// </summary>
        public async Task ExecuteResultAsync(ActionContext context) {
            var objectResult = new ObjectResult(this) {
                StatusCode = StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}

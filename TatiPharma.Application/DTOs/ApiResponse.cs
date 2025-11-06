using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        // Success
        public static ApiResponse<T> SuccessResult(T data, string message = "Success")
            => new() { Success = true, Data = data, Message = message };

        // Error
        public static ApiResponse<T> ErrorResult(List<string> errors, string message = "Failed")
            => new() { Success = false, Errors = errors, Message = message };
    }
}

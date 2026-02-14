using System.Collections.Generic;

namespace TaskManagement.Application.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResult<T> Successful(T data) => new ServiceResult<T> { Success = true, Data = data };
        public static ServiceResult<T> Failed(params string[] errors) => new ServiceResult<T> { Success = false, Errors = new List<string>(errors) };
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResult Successful() => new ServiceResult { Success = true };
        public static ServiceResult Failed(params string[] errors) => new ServiceResult { Success = false, Errors = new List<string>(errors) };
    }
}
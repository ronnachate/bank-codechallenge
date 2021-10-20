using Newtonsoft.Json;

namespace CodeChallenge.DataObjects
{
    public class BaseResponse<T> where T : class
    {
        public string ResponseCode { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T ToSuccess(string message = "")
        {
            var res = new BaseResponse<T>()
            {
                IsSuccess = true,
                ResponseCode = Enumeration.ToResponseCode(BaseStatusEnum.Success),
                Message = message
            };
            return ToObject(res);
        }

        public T ToSuccess(string responseCode, string message = "")
        {
            var res = new BaseResponse<T>()
            {
                IsSuccess = true,
                ResponseCode = responseCode,
                Message = message
            };
            return ToObject(res);
        }

        public T ToFailed(string message = "")
        {
            var res = new BaseResponse<T>()
            {
                IsSuccess = false,
                ResponseCode = Enumeration.ToResponseCode(BaseStatusEnum.Failed),
                Message = message
            };
            return ToObject(res);
        }

        public T ToFailed(string responseCode, string message = "")
        {
            var res = new BaseResponse<T>()
            {
                IsSuccess = false,
                ResponseCode = responseCode,
                Message = message
            };
            return ToObject(res);
        }

        private T ToObject(object value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}

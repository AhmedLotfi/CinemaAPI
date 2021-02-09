namespace CinemaAPI.Utilites
{
    public class APIResponse
    {
        public int ResponseCode { get; private set; }

        public string ResponseMessage { get; private set; }

        public object Data { get; private set; }

        public static APIResponse GetAPIResponse(int responseCode, string responseMessage, object data)
        {
            return new APIResponse
            {
                Data = data,
                ResponseCode = responseCode,
                ResponseMessage = responseMessage
            };
        }
    }
}

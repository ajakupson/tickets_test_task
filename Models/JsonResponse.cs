namespace agileworks_1.Models
{
    public class JsonResponse
    {

        public int Code { get; set; } = 400;
        public bool IsSuccess { get; set; } = false;
        public string Data { get; set; } = "{}";
        public string ErrorMsg { get; set; } = "";
    }
}

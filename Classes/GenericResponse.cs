namespace MovieTheaterWS_v2.Classes
{
    public class GenericResponse<T> where T : class
    {
        public T? Data { get; set; }
        public string Message { get; set; }

        public GenericResponse() { 
            Message = string.Empty;
        }
    }
}

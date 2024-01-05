namespace RecipesApi.Models
{
    public class ResponseGeneric<T> where T : class
    {
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}

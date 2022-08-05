namespace API.Helpers
{

    public class RequestParms : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
        
    }
}
namespace black_follow.Entity;

public record AuditLog:BaseEntity<int>
{
    
   
        public int Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string User { get; set; }
    

}
using System.Reflection.Metadata;


public record JobResponse( int Id, List<string>AtsKeywords,string Description,string Url,string Role,DateOnly? ClosingDate,string Location);
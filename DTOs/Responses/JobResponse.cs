using System.Reflection.Metadata;


public record JobResponse( int Id,Blob Cv,Blob CoverLetter,string Description,string Url,string Role,DateOnly? ClosingDate,string Location);
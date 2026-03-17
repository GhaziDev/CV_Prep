using System.Reflection.Metadata;
namespace cv_prep.DTOs.Requests;




public record JobRequest(int Id,Blob Cv,Blob CoverLetter,string Description,string Url,string Role,DateOnly? ClosingDate,string Location,int scheduler);


using System.Reflection.Metadata;
using cv_prep.Models;
namespace cv_prep.DTOs.Requests;




public record JobRequest(int Id,List<string> AtsKeywords,string Description,string Url,string Role,DateOnly? ClosingDate,string Location,int user);


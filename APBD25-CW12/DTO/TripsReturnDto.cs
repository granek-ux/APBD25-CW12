namespace APBD25_CW12.DTO;

public class TripsReturnDto
{
    public int pageNum { get; set; }
    public int pageSize { get; set; }
    public int allPages { get; set; }
    public List<TripsDto> trips { get; set; }
}
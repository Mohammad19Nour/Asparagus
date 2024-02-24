namespace AsparagusN.Data.Entities;

public class OrderAssignment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int DriverId { get; set; }
    public DateTime AssignmentDate  { get; set; }
}
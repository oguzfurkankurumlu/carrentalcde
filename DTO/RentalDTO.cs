public class RentalDTO
{
    public int RentalID { get; set; }
    public int UserID { get; set; }
    public int CarID { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string RentalStatus { get; set; }

    // Pickup and Return office locations
    public string PickupOffice { get; set; }
    public string ReturnOffice { get; set; }

    // Rental and Return time
    public TimeSpan RentalTime { get; set; }  // Alış saati
    public TimeSpan ReturnTime { get; set; }  // Teslim saati
    public List<CarDTO> Cars { get; set; }
    public decimal TotalAmount => (ReturnDate - RentalDate).Days * (Cars.FirstOrDefault()?.PricePerDay ?? 0);
}

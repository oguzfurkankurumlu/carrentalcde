// public class Rental
// {
//     public int RentalID { get; set; }
//     public int UserID { get; set; }
//     public int CarID { get; set; }
//     public DateTime RentalDate { get; set; }
//     public DateTime ReturnDate { get; set; }
//     public string RentalStatus { get; set; }

//     // Pickup and Return office locations
//     public string PickupOffice { get; set; }  // Pickup Office (Alış ofisi)
//     public string ReturnOffice { get; set; }  // Return Office (Teslim ofisi)

//     // Rental and Return time (DateTime?)
//     public DateTime RentalTime { get; set; }  // Alış saati
//     public DateTime ReturnTime { get; set; }  // Teslim saati
// }

public class Rental
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
}
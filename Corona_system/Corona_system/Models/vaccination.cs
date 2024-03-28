namespace Corona_system.Models
{
    public class vaccination
    {
        public int Id { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string VaccineManufacturer { get; set; }
        public int ClientId { get; set; }
    }
}

namespace SimplyFlyServer.Models
{
    public class User
    {
        public enum Usertype
        {
            Passenger,
            FlightOwner,
            Admin
        }
        public int UserId {  get; set; }
        public string UserName { get; set; }=string.Empty;
        public string FirstName {  get; set; }=string.Empty;
        public string LastName { get; set; } =string.Empty;
        public byte[] Password { get; set; }
        public byte[] HashKey {  get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
        public Usertype Role { get; set; } = Usertype.Passenger;
        public DateTime CreatedDate { get; set; }
        public Airline AirlineAssociated { get; set; } = null;



    }
}

namespace SimplyFlyServer.Models
{
	public class Passenger
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Gender { get; set; }
		public string Nationality { get; set; }
		public string PassportNumber { get; set; }
		public DateTime PassportExpiryDate { get; set; }
		public string CountryOfIssue { get; set; }
		public string MealPreference { get; set; }
	}
}

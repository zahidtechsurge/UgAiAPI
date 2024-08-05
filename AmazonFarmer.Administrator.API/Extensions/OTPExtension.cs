namespace AmazonFarmer.Administrator.API.Extensions
{
    public class OTPExtension
    {
        private static readonly Random random = new Random(); 
        public static string GenerateOTP()
        {
            // Generate a random 4-digit number
            int otp = random.Next(1000, 9999);

            // Convert the number to a string and return
            return otp.ToString("D4");
        }
    }
}

namespace ESA.Core.Utils
{
    public class Commons
    {
        public static int CalculateAge(DateTime? birthdate)
        {
            var today = DateTime.Today;
            if (birthdate == null)
            {
                return 0;
            }
            // Calculate the age.
            var age = today.Year - birthdate.Value.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthdate.Value.Date > today.AddYears(-age)) age--;

            return age;
        }

        public static CancellationTokenSource GetCancellationToken(int seconds)
        {
            var ct = new CancellationTokenSource();
            ct.CancelAfter(TimeSpan.FromSeconds(seconds));
            return ct;
        }
    }
}

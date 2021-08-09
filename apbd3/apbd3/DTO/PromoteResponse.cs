using apbd3.Models;

namespace apbd3.DTO
{
    public class PromoteResponse
    {
        public PromoteResponse(Enrollment enrollment)
        {
            Studies = enrollment.StartDate;
            Semester = enrollment.Semester;
        }

        private string Studies { get; }
        private int Semester { get; }
    }
}
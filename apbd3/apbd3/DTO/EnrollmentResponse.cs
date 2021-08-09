using apbd3.Models;

namespace apbd3.DTO
{
    public class EnrollmentResponse
    {
        public EnrollmentResponse(Enrollment enrollment)
        {
            Idenrollment = enrollment.IdEnrollment;
            Semester = enrollment.Semester;
            IdStudy = enrollment.IdStudy;
            StartDate = enrollment.StartDate;
        }

        private int Idenrollment { get; }
        private int Semester { get; }
        private int IdStudy { get; }
        private string StartDate { get; }
    }
}
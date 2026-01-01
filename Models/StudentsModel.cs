namespace Version_Mark_3.Models
{
    public class StudentsModel
    {
        public int? StudentID { get; set; }
        public string StudentName { get; set; }
        public int StudentAge { get; set; }
        public int StudentMob { get; set; }
        public int AdminID { get; set; }

        public List<AdminModel> Adminlist { get; set; }

    }
}

namespace SD_330_W22SD_Assignment.Models.ViewModels
{
    public class QuestionDetailsViewModel
    {
        public Question Question { get; set; }
        public List<Answer> Answers { get; set; }

        public QuestionDetailsViewModel(Question question, List<Answer> answers)
        {
            Question = question;
            Answers = answers;
        }
    }
}

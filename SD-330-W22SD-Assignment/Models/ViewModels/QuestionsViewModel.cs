namespace SD_330_W22SD_Assignment.Models.ViewModels
{
    public class QuestionsViewModel
    {
        public List<Question> Questions { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }

        public QuestionsViewModel(List<Question> questions, int currentPage, int maxPage)
        {
            Questions = questions;
            CurrentPage = currentPage;
            MaxPage = maxPage;
        }
    }
}

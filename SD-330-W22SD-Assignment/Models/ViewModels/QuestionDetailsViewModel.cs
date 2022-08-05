using System.Text;

namespace SD_330_W22SD_Assignment.Models.ViewModels
{
    public class QuestionDetailsViewModel
    {
        public Question Question { get; set; }
        public int VoteCount { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Tag> Tags { get; set; }

        public string TagsString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var tag in Tags)
                {
                    sb.Append(tag.Name + ", ");
                }
                return sb.ToString();
            }
        }

        public QuestionDetailsViewModel(Question question, int voteCount, List<Answer> answers, List<Tag> tags)
        {
            Question = question;
            VoteCount = voteCount;
            Answers = answers;
            Tags = tags;
        }
    }
}

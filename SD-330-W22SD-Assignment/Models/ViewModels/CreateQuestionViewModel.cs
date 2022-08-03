using Microsoft.AspNetCore.Mvc.Rendering;

namespace SD_330_W22SD_Assignment.Models.ViewModels
{
    public class CreateQuestionViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public MultiSelectList Tags { get; set; }

        public CreateQuestionViewModel(string title, string body, List<Tag> tags)
        {
            Title = title;
            Body = body;
            Tags = new MultiSelectList(tags, "Id", "Name");
        }
    }
}

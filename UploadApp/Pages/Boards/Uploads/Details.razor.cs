
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using UploadApp.Models.BUploads;

namespace UploadApp.Pages.Boards.Uploads
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IUploadRepository UploadRepositoryAsyncReference { get; set; }

        protected BoardUploads model = new BoardUploads();

        protected string content = ""; 

        protected override async Task OnInitializedAsync()
        {
            model = await UploadRepositoryAsyncReference.GetByIdAsync(Id);
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content); 

        }
    }
}

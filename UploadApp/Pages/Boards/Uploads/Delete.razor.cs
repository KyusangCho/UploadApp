using UploadApp.Models.BUploads;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace UploadApp.Pages.Boards.Uploads
{
    public partial class Delete
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IUploadRepository UploadRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected BoardUploads model = new BoardUploads();

        protected string content = "";

        protected override async Task OnInitializedAsync()
        {
            model = await UploadRepositoryAsyncReference.GetByIdAsync(Id);
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);

        }

        protected async void DeleteClick()
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Id}번 글을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await UploadRepositoryAsyncReference.DeleteAsync(Id);
                NavigationManagerReference.NavigateTo("/Boards/Uploads"); 
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alarm", "취소되었습니다."); 
            }
        }

        protected override void OnInitialized()
        {

        }
    }
}

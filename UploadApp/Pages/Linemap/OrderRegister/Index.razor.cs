﻿using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.ProgressBar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UploadApp.Models.BNotices;

namespace UploadApp.Pages.Linemap.OrderRegister
{
    public partial class Index
    {
        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected List<BoardNotices> models;

        protected BeanyPager.BeanyPagerBase pager = new BeanyPager.BeanyPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0, 
            PageSize = 2, 
            PagerButtonCount = 5, 
        };

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(this.searchQuery))
            {
                await DisplayData(); 
            }
            else
            {
                await SearchData(); 
            }

        }

        private async Task DisplayData()
        {
            //await Task.Delay(3000); 
            var resultSet = await NoticeRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = resultSet.TotalRecords; 
            models = resultSet.Records.ToList();
        }

        protected void NameClick(int id)
        {
            NavigationManagerReference.NavigateTo($"/Boards/Notices/Details/{id}"); 
        }

        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            if (string.IsNullOrEmpty(this.searchQuery))
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }

            StateHasChanged(); 
        }

        private string searchQuery; 

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged(); 
        }

        private async Task SearchData()
        {
            //await Task.Delay(3000); 
            var resultSet = await NoticeRepositoryAsyncReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
            pager.RecordCount = resultSet.TotalRecords;
            models = resultSet.Records.ToList();
        }
    }
}

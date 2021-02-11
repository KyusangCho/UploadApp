﻿using UploadApp.Pages.Boards.Uploads.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UploadApp.Models.BUploads;

namespace UploadApp.Pages.Boards.Uploads
{
    public partial class Manage
    {
        [Parameter]
        public int ParentId { get; set; } = 0; 

        [Inject]
        public IUploadRepository UploadRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        public EditorForm EditorFormReference { get; set; }
        public DeleteDialog DeleteDialogReference { get; set; }

        protected List<BoardUploads> models;

        /// <summary>
        /// 공지사항으로 올리기 폼을 띄울건지 여부 
        /// </summary>
        public bool IsInlineDialogShow { get; set; } = false; 

        protected BoardUploads model = new BoardUploads(); 

        protected BeanyPager.BeanyPagerBase pager = new BeanyPager.BeanyPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 10,
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
            if (ParentId == 0)
            {
                //await Task.Delay(3000); 
                var resultSet = await UploadRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
            else
            {
                var resultSet = await UploadRepositoryAsyncReference.GetAllByParentIdAsync(pager.PageIndex, pager.PageSize, ParentId);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
        }

        private async Task SearchData()
        {
            if (ParentId == 0)
            {
                var resultSet = await UploadRepositoryAsyncReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
            else
            {
                var resultSet = await UploadRepositoryAsyncReference.SearchAllByParentIdAsync(pager.PageIndex, pager.PageSize, this.searchQuery, ParentId);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
        }

        protected void NameClick(int id)
        {
            NavigationManagerReference.NavigateTo($"/Boards/Uploads/Details/{id}");
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

        }

        public string EditorFormTitle { get; set; } = "CREATE"; 

        protected void ShowEditorForm()
        {
            EditorFormTitle = "CREATE";
            this.model = new BoardUploads(); 
            EditorFormReference.Show(); 
        }

        protected void EditBy(BoardUploads model)
        {
            EditorFormTitle = "EDIT";
            this.model = model; 
            EditorFormReference.Show(); 
        }

        protected void DeleteBy(BoardUploads model)
        {
            this.model = model;
            DeleteDialogReference.Show(); 
        }
        protected void ToggleBy(BoardUploads model)
        {
            this.model = model;
            IsInlineDialogShow = true; 
        }

        protected async void CreateOrEdit()
        {
            EditorFormReference.Hide();
            await DisplayData();
            
        }

        protected async void DeleteClick()
        {
            await UploadRepositoryAsyncReference.DeleteAsync(this.model.Id);
            DeleteDialogReference.Hide();
            this.model = new BoardUploads(); 
            await DisplayData();
        }

        protected void ToggleClose()
        {
            IsInlineDialogShow = false;
            this.model = new BoardUploads(); 
        }

        protected async void ToggleClick()
        {
            this.model.IsPinned = (this.model?.IsPinned == true) ? false : true; 

            await UploadRepositoryAsyncReference.EditAsync(this.model);
            IsInlineDialogShow = false; 
            this.model = new BoardUploads();
            await DisplayData();
        }

        private string searchQuery; 

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged(); 
        }
    }
}

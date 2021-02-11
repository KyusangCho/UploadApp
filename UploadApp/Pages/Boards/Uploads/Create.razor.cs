﻿using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
using System.Collections.Generic;
using UploadApp.Models.BUploads;

namespace UploadApp.Pages.Boards.Uploads
{
    public partial class Create
    {
        [Inject]
        public IUploadRepository UploadRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected BoardUploads model = new BoardUploads();

        public string ParentId { get; set; }

        protected int[] parentIds = { 1, 2, 3 }; 

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId; 
            await UploadRepositoryAsyncReference.AddAsync(model);
            NavigationManagerReference.NavigateTo("/Boards/Uploads"); 
        }

        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.StrikeThrough },
            new ToolbarItemModel() { Command = ToolbarCommand.FontName },
            new ToolbarItemModel() { Command = ToolbarCommand.FontSize },
            new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
            new ToolbarItemModel() { Command = ToolbarCommand.BackgroundColor },
            new ToolbarItemModel() { Command = ToolbarCommand.LowerCase },
            new ToolbarItemModel() { Command = ToolbarCommand.UpperCase },
            new ToolbarItemModel() { Command = ToolbarCommand.SuperScript },
            new ToolbarItemModel() { Command = ToolbarCommand.SubScript },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.OrderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.UnorderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.Outdent },
            new ToolbarItemModel() { Command = ToolbarCommand.Indent },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
            new ToolbarItemModel() { Command = ToolbarCommand.Image },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
            new ToolbarItemModel() { Command = ToolbarCommand.Print },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo }
        };
    }
}
